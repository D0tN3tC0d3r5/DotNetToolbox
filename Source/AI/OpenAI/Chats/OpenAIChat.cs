namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChat
    : Chat<OpenAIChatOptions> {
    public OpenAIChat(OpenAIChatOptions? options = null)
        : base(options) {
        Messages.Add(new("system", [new("text", Options.SystemMessage)]));
    }
}
