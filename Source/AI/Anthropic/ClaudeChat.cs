namespace DotNetToolbox.AI.Anthropic;

public class ClaudeChat(string userName, ClaudeChatOptions? options = null)
    : Chat<ClaudeChatOptions>(userName, options) {
    public List<ClaudeMessage> Messages { get; } = [];
}
