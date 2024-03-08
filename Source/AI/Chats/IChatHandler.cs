namespace DotNetToolbox.AI.Chats;

public interface IChatHandler {
    IChat Start(Action<IChatOptions>? configure = null);
    Task<Message> SendMessage(IChat chat, Message input, CancellationToken ct = default);
}
