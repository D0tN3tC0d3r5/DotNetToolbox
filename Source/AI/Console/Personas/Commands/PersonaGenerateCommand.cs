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
                PrimaryRole = Input.Ask<string>("Enter the primary role for the persona:"),
                IntendedUse = Input.Ask<string>("Enter the intended use for the persona:")
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
                persona.AdditionalInformation.Add(new Query { Question = nextQuestion, Answer = answer });
            }

            //Output.WriteLine("[yellow]Generating persona...[/]");
            //persona.Prompt = await _aiService.GeneratePrompt(persona);
            //var reviewPrompt = string.Empty;
            //var feedBackCount = 0;
            //while (!reviewPrompt.Equals("No", StringComparison.OrdinalIgnoreCase)) {
            //    Output.WriteLine(persona.Prompt);
            //    reviewPrompt = Input.TextPrompt("Would you like to review the generated prompt?\n[white]Respond with 'No' to continue or enter your feedback to regenerate the prompt.[/]")
            //                        .WithDefault("No").Show();
            //    Output.WriteLine("[yellow]Updating persona...[/]");
            //    persona.AdditionalInformation.Add(new Query { Question = $"User feedback {++feedBackCount}", Answer = reviewPrompt });
            //    persona.Prompt = await _aiService.GeneratePrompt(persona);
            //}

            _personaHandler.Add(persona);
            Output.WriteLine($"[green]Persona '{persona.Name}' generated successfully.[/]");
            Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' added successfully.", persona.Key, persona.Name);
            return Result.Success();
        }
        catch (Exception ex) {
            Output.WriteError($"Error generating persona: {ex.Message}");
            Logger.LogError(ex, "Error generating the new persona.");
            return Result.Error(ex.Message);
        }
    }
}
