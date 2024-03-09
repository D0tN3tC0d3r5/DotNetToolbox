namespace DotNetToolbox.AI.Chats;

public interface IChatFactory {
    IChat Create(Action<IChatOptions>? configure = null);
    //Task<Message> SendMessage(IChat chat, Message input, CancellationToken ct = default);
}
