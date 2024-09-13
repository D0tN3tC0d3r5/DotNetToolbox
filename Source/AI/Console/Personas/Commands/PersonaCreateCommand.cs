namespace AI.Sample.Personas.Commands;

public class PersonaCreateCommand : Command<PersonaCreateCommand> {
    private readonly IPersonaHandler _personaHandler;
    private const int _maxQuestions = 10;

    public PersonaCreateCommand(IHasChildren parent, IPersonaHandler personaHandler)
        : base(parent, "Generate", ["gen"]) {
        _personaHandler = personaHandler;
        Description = "Generate a new agent persona using AI assistance.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var name = await Input.BuildTextPrompt<string>("How would you like to call the Agent:")
                                .AsRequired()
                                .ShowAsync(ct);
        var role = await Input.BuildTextPrompt<string>($"What is the [white]{name}[/] primary role:")
                                .AnswerOnANewLine()
                                .AsRequired()
                                .ShowAsync(ct);
        var persona = new PersonaEntity {
            Name = name,
            Role = role,
        };
        var goal = await Input.BuildMultilinePrompt($"What is the Main Goal for the [white]{name}[/]: ")
                                .ShowAsync(ct);
        persona.Goals.AddRange(goal.Replace("\r", "").Split("\n"));
        var addAnotherGoal = await Input.ConfirmAsync("Would you like to add another goal?", ct);
        while (addAnotherGoal) {
            goal = await Input.BuildMultilinePrompt("Additional goal: ")
                        .ShowAsync(ct);
            persona.Goals.AddRange(goal.Replace("\r", "").Split("\n"));
            addAnotherGoal = await Input.ConfirmAsync("Would you like to add another goal?", ct);
        }

        for (var questionCount = 0; questionCount < _maxQuestions; questionCount++) {
            Output.WriteLine("[yellow]Let me see if I have more questions...[/]");
            Output.WriteLine("[grey](You can skip the questions by typing 'proceed' at any time.)[/]");

            var queries = await _personaHandler.GeneratePersonaCreationQuestion(persona);
            if (queries.Length == 0) {
                Output.WriteLine("[green]I've gathered sufficient information to generate the agent's persona.[/]");
                break;
            }
            var proceed = false;
            foreach (var query in queries) {
                query.Answer = await Input.BuildMultilinePrompt($"Question {questionCount + 1}: {query.Question}")
                                    .ShowAsync(ct);
                if (query.Answer.Equals("proceed", StringComparison.OrdinalIgnoreCase)) {
                    proceed = true;
                    break;
                }
                persona.Questions.Add(query);
            }

            if (!proceed) continue;
            Output.WriteLine("[green]Ok. Let's proceed with the Agent's Persona generation.[/]");
            break;
        }

        await _personaHandler.UpdateCreatedPersona(persona);

        Output.WriteLine();
        Output.WriteLine("[yellow]Here is generated Agent Persona:[/]");
        Output.WriteLine($"[teal]Name:[/] {persona.Name}");
        Output.WriteLine($"[teal]Role:[/] {persona.Role}");
        Output.WriteLine("[teal]Goals:[/]");
        Output.WriteLine(string.Join("\n", persona.Goals.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Expertise:[/]");
        Output.WriteLine(persona.Expertise);
        Output.WriteLine("[teal]Traits:[/]");
        Output.WriteLine(string.Join("\n", persona.Traits.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Requirements:[/]");
        Output.WriteLine(string.Join("\n", persona.Important.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Restrictions:[/]");
        Output.WriteLine(string.Join("\n", persona.Negative.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Other:[/]");
        Output.WriteLine(string.Join("\n", persona.Other.Select(i => $" - {i}")));
        Output.WriteLine();

        Output.WriteLine($"[green]Persona '{persona.Name}' generated successfully.[/]");
        Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' generated successfully.", persona.Key, persona.Name);

        var savePersona = await Input.ConfirmAsync("Are you ok with the Agent Persona above?", ct);
        if (savePersona) {
            _personaHandler.Add(persona);
            Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' added successfully.", persona.Key, persona.Name);
        }

        Output.WriteLine();
        return Result.Success();
    }, "Error generating the new persona.", ct);
}
