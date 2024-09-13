namespace AI.Sample.Personas.Commands;

public class PersonaCreateCommand(IHasChildren parent, IPersonaHandler personaHandler)
    : Command<PersonaCreateCommand>(parent, "Generate", n => {
        n.Aliases = ["gen"];
        n.Description = "Create a new persona.";
        n.Help = "Generate a new agent persona using AI assistance.";
    }) {
    private const int _maxQuestions = 10;

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing Personas->Generate command...");
        var cts = CancellationTokenSource.CreateLinkedTokenSource(lt, ct);
        var name = await Input.BuildTextPrompt<string>("How would you like to call the Agent:")
                              .AsRequired()
                              .ShowAsync(cts.Token);
        var role = await Input.BuildTextPrompt<string>($"What is the [white]{name}[/] primary role:")
                              .AnswerOnANewLine()
                              .AsRequired()
                              .ShowAsync(cts.Token);
        var persona = new PersonaEntity {
            Name = name,
            Role = role,
        };
        var goal = await Input.BuildMultilinePrompt($"What is the Main Goal for the [white]{name}[/]: ")
                              .ShowAsync(cts.Token);
        persona.Goals.AddRange(goal.Replace("\r", "").Split("\n"));
        var addAnotherGoal = await Input.ConfirmAsync("Would you like to add another goal?", cts.Token);
        while (addAnotherGoal) {
            goal = await Input.BuildMultilinePrompt("Additional goal: ")
                              .ShowAsync(cts.Token);
            persona.Goals.AddRange(goal.Replace("\r", "").Split("\n"));
            addAnotherGoal = await Input.ConfirmAsync("Would you like to add another goal?", cts.Token);
        }

        for (var questionCount = 0; questionCount < _maxQuestions; questionCount++) {
            Output.WriteLine("[yellow]Let me see if I have more questions...[/]");
            Output.WriteLine("[grey](You can skip the questions by typing 'proceed' at any time.)[/]");

            var queries = await personaHandler.GeneratePersonaCreationQuestion(persona);
            if (queries.Length == 0) {
                Output.WriteLine("[green]I've gathered sufficient information to generate the agent's persona.[/]");
                break;
            }
            var proceed = false;
            foreach (var query in queries) {
                query.Answer = await Input.BuildMultilinePrompt($"Question {questionCount + 1}: {query.Question}")
                                          .ShowAsync(cts.Token);
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

        await personaHandler.UpdateCreatedPersona(persona);

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

        var savePersona = await Input.ConfirmAsync("Are you ok with the Agent Persona above?", cts.Token);
        if (savePersona) {
            personaHandler.Add(persona);
            Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' added successfully.", persona.Key, persona.Name);
        }

        Output.WriteLine();
        return Result.Success();
    }, "Error generating the new persona.", ct);
}
