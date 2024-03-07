namespace DotNetToolbox.AI.Anthropic.Chats;

public interface IChatHandler : IChatHandler<Chat, ChatOptions, Message> {
    Task<Message> SendMessage(Chat chat, string message, CancellationToken ct = default);
}
