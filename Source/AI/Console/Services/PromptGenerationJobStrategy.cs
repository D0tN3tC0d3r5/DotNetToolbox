using DotNetToolbox.AI.Chats;
using DotNetToolbox.AI.Jobs;

namespace AI.Sample.Services;

public class PromptGenerationJobStrategy : IJobStrategy<PersonaEntity, string> {
    public string Instructions => "Generate a comprehensive prompt for an AI persona based on the provided information.";

    public void AddPrompt(IChat chat, PersonaEntity input) {
        // This method is not used in our implementation as we're adding messages directly in the GeneratePrompt method
    }

    public string GetResult(IChat chat) {
        return chat.Messages.Last(m => m.Role == MessageRole.Assistant).Text;
    }
}
