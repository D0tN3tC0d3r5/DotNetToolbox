namespace DotNetToolbox.AI.OpenAI;

public class OpenAIChat(string userName, OpenAIChatOptions? options = null)
    : Chat<OpenAIChatOptions>(userName, options) {
    public List<OpenAIMessage> Messages { get; } = [];
}
