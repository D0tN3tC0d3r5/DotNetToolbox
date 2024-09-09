namespace AI.Sample.Personas.Commands;

public class PersonaCreateCommand : Command<PersonaCreateCommand> {
    private readonly IPersonaHandler _personaHandler;
    private const int _maxQuestions = 10;

    public PersonaCreateCommand(IHasChildren parent, IPersonaHandler personaHandler)
        : base(parent, "Generate", ["gen"]) {
        _personaHandler = personaHandler;
        Description = "Generate a new persona using AI assistance.";
    }

    protected override async Task<Result> ExecuteAsync(CancellationToken ct = default) {
        try {
            var persona = new PersonaEntity {
                Name = Input.BuildTextPrompt<string>("Enter the name for the persona:").AnswerOnANewLine().For(nameof(PersonaEntity.Name)).Show(),
                Role = Input.BuildTextPrompt<string>("Enter the primary role for the persona:").AsRequired().AnswerOnANewLine().For(nameof(PersonaEntity.Role)).Show(),
            };
            var goalCount = 1;
            Output.WriteLine("[yellow]Add the goals for the persona:[/]");
            var goal = Input.BuildTextPrompt<string>($"Goal {goalCount}: ").AsRequired().AnswerOnANewLine().For($"Goal {goalCount}").Show();
            persona.Goals.Add(goal);
            var addAnotherGoal = Input.Confirm("Would you like to add another goal?", false);
            while (addAnotherGoal) {
                goalCount++;
                goal = Input.BuildTextPrompt<string>($"Goal {goalCount}: ").AsRequired().AnswerOnANewLine().For($"Goal {goalCount}").Show();
                persona.Goals.Add(goal);
                addAnotherGoal = Input.Confirm("Would you like to add another goal?", false);
            }

            for (var questionCount = 0; questionCount < _maxQuestions; questionCount++) {
                Output.WriteLine("[yellow]Let me see if I have more questions...[/]");
                Output.WriteLine("[grey](You can skip the questions by typing 'proceed' at any time.)[/]");

                var queries = await _personaHandler.GenerateQuestions(persona);
                if (queries.Length == 0) {
                    Output.WriteLine("[green]I've gathered sufficient information to generate the agent's persona.[/]");
                    break;
                }

                var proceed = false;
                foreach (var query in queries) {
                    query.Answer = Input.BuildTextPrompt<string>($"Question {questionCount + 1}: {query.Question}")
                                           .AsRequired()
                                           .AnswerOnANewLine()
                                           .For($"Question {questionCount + 1}")
                                           .Show();
                    if (query.Answer.Equals("proceed", StringComparison.OrdinalIgnoreCase)) {
                        Output.WriteLine("[green]The user requested to proceed with the persona generation.[/]");
                        proceed = true;
                        break;
                    }
                    persona.Questions.Add(query);
                }

                if (!proceed) continue;
                Output.WriteLine("[green]Ok, Let's proceed with the generation of the agent's persona.[/]");
                break;
            }

            await _personaHandler.GeneratePersonaProperties(persona);

            _personaHandler.Add(persona);
            Output.WriteLine($"[green]Persona '{persona.Name}' generated successfully.[/]");
            Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' added successfully.", persona.Key, persona.Name);
            Output.WriteLine();

            return Result.Success();
        }
        catch (Exception ex) {
            Output.WriteError($"Error generating persona: {ex.Message}");
            Logger.LogError(ex, "Error generating the new persona.");
            Output.WriteLine();

            return Result.Error(ex.Message);
        }
    }
}
