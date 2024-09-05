using DotNetToolbox.AI.Chats;
using DotNetToolbox.AI.Jobs;

namespace AI.Sample.Services;

// Custom job strategies
public class PersonaGenerationJobStrategy : IJobStrategy<PersonaEntity, string> {
    public string Instructions => "Generate the next question for persona creation based on the provided information.";

    public void AddPrompt(IChat chat, PersonaEntity input) {
        // This method is not used in our implementation as we're adding messages directly in the GetNextQuestion method
    }

    public string GetResult(IChat chat)
        => chat.Messages.Last(m => m.Role == MessageRole.Assistant).Text;
}
