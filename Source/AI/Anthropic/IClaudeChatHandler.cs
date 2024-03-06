namespace DotNetToolbox.AI.Anthropic;

public interface IClaudeChatHandler : IChatHandler<ClaudeChat, ClaudeChatOptions> {
    Task<ClaudeMessage> SendMessage(ClaudeChat chat, string message, CancellationToken ct = default);
}