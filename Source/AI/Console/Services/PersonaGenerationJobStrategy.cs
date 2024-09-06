namespace AI.Sample.Services;

// Custom job strategies
public class PersonaGenerationJobStrategy(IServiceProvider services)
    : IJobStrategy<PersonaEntity, string> {
    private readonly IOutput _output = services.GetRequiredService<IOutput>();
    private readonly IInput _input = services.GetRequiredService<IInput>();

    public string Instructions => "Generate the next question for persona creation based on the provided information.";

    public void AddPrompt(IChat chat, PersonaEntity input, IJobContext context) {
        _output.WriteLine("I am here!");
        _input.Confirm("continue?");
    }

    public string GetResult(IChat chat, IJobContext context)
        => chat.Messages.Last(m => m.Role == MessageRole.Assistant).Text;
}
