namespace DotNetToolbox.AI.OpenAI;

public interface IOpenAIChatHandler : IChatHandler<OpenAIChat, OpenAIChatOptions> {
    Task<OpenAIMessage> SendMessage(OpenAIChat chat, string message, CancellationToken ct = default);
    Task<OpenAIMessage> SendToolResult(OpenAIChat chat, ToolResult[] results, CancellationToken ct = default);
}
