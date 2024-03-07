namespace DotNetToolbox.AI.OpenAI.Chats;

public interface IChatHandler : IChatHandler<Chat, ChatOptions, Message> {
    Task<Message> SendMessage(Chat chat, string message, CancellationToken ct = default);
    Task<Message> SendToolResult(Chat chat, ToolResult[] results, CancellationToken ct = default);
}
