namespace AI.Sample.Personas.Commands;

public class PersonaGenerateCommand : Command<PersonaGenerateCommand> {
    private readonly IPersonaHandler _personaHandler;
    private readonly IAIService _aiService;
    private const int MaxQuestions = 10;

    public PersonaGenerateCommand(IHasChildren parent, IPersonaHandler personaHandler, IAIService aiService)
        : base(parent, "Generate", ["gen"]) {
        _personaHandler = personaHandler;
        _aiService = aiService;
        Description = "Generate a new persona using AI assistance.";
    }

    protected override async Task<Result> Execute(CancellationToken ct = default) {
        try {
            var persona = new PersonaEntity {
                Name = Input.Ask<string>("Enter the name for the persona:"),
                Role = Input.Ask<string>("Enter the primary role for the persona:"),
                Goal = Input.Ask<string>("Enter the intended use for the persona:")
            };

            Output.WriteLine("[yellow]Starting AI-assisted questioning phase...[/]");
            Output.WriteLine("[grey](Type 'generate' at any time to proceed with persona generation)[/]");

            for (var questionCount = 0; questionCount < MaxQuestions; questionCount++) {
                var nextQuestion = await _aiService.GetNextQuestion(persona);
                if (string.IsNullOrEmpty(nextQuestion)) {
                    Output.WriteLine("[green]AI has gathered sufficient information to generate the persona.[/]");
                    break;
                }
                var answer = Input.Ask($"Question {questionCount + 1}: {nextQuestion}");
                if (answer.Equals("generate", StringComparison.OrdinalIgnoreCase)) {
                    Output.WriteLine("[green]The user requested to proceed with the persona generation.[/]");
                    break;
                }
                persona.Traits.Add(new Query { Question = nextQuestion, Answer = answer });
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
