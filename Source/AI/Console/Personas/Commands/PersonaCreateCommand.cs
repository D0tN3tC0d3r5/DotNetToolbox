namespace AI.Sample.Personas.Commands;

public class PersonaCreateCommand : Command<PersonaCreateCommand> {
    private readonly IPersonaHandler _personaHandler;
    private readonly IAIService _aiService;
    private const int _maxQuestions = 10;

    public PersonaCreateCommand(IHasChildren parent, IPersonaHandler personaHandler, IAIService aiService)
        : base(parent, "Generate", ["gen"]) {
        _personaHandler = personaHandler;
        _aiService = aiService;
        Description = "Generate a new persona using AI assistance.";
    }

    protected override async Task<Result> ExecuteAsync(CancellationToken ct = default) {
        try {
            var persona = new PersonaEntity {
                Name = Input.BuildTextPrompt<string>("Enter the name for the persona:").StartTheAnswerOnANewLine().For(nameof(PersonaEntity.Name)).Show(),
                Role = Input.BuildTextPrompt<string>("Enter the primary role for the persona:").AsRequired().StartTheAnswerOnANewLine().For(nameof(PersonaEntity.Role)).Show(),
            };
            var goalCount = 1;
            Output.WriteLine("[yellow]Add the goals for the persona:[/]");
            var goal = Input.BuildTextPrompt<string>($"Goal {goalCount}: ").AsRequired().StartTheAnswerOnANewLine().For($"Goal {goalCount}").Show();
            persona.Goals.Add(goal);
            var addAnotherGoal = Input.Confirm("Would you like to add another goal?", false);
            while (addAnotherGoal) {
                goalCount++;
                goal = Input.BuildTextPrompt<string>($"Goal {goalCount}: ").AsRequired().StartTheAnswerOnANewLine().For($"Goal {goalCount}").Show();
                persona.Goals.Add(goal);
                addAnotherGoal = Input.Confirm("Would you like to add another goal?", false);
            }

            Output.WriteLine("[yellow]Starting AI-assisted questioning phase...[/]");
            Output.WriteLine("[grey](Type 'proceed' at any time to proceed with persona generation)[/]");

            for (var questionCount = 0; questionCount < _maxQuestions; questionCount++) {
                var queries = await _personaHandler.GenerateQuestion(persona);
                if (queries.Length == 0) {
                    Output.WriteLine("[green]AI has gathered sufficient information to generate the persona.[/]");
                    break;
                }

                var proceed = false;
                foreach (var query in queries) {
                    query.Answer = Input.BuildTextPrompt<string>($"Question {questionCount + 1}: {query.Question}")
                                           .AsRequired()
                                           .StartTheAnswerOnANewLine()
                                           .For($"Question {questionCount + 1}")
                                           .Show();
                    if (query.Answer.Equals("proceed", StringComparison.OrdinalIgnoreCase)) {
                        Output.WriteLine("[green]The user requested to proceed with the persona generation.[/]");
                        proceed = true;
                        break;
                    }
                    persona.Questions.Add(query);
                }

                if (proceed) break;
            }

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
