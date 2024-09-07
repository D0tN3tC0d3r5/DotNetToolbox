// namespace AI.Sample.Services;

// public class PromptGenerationJobStrategy(IServiceProvider services)
//     : IJobStrategy<PersonaEntity, string> {
//     private readonly IOutput _output = services.GetRequiredService<IOutput>();
//     private readonly IInput _input = services.GetRequiredService<IInput>();

//     public string Instructions => "Generate a comprehensive prompt for an AI persona based on the provided information.";

//     public void AddPrompt(IMessages chat, PersonaEntity input, IJobContext context) {
//         _output.WriteLine("I am here!");
//         _input.Confirm("continue?");
//     }

//     public string GetResult(IMessages chat, IJobContext context)
//         => chat.Messages.Last(m => m.Role == MessageRole.Assistant).Text;
// }
