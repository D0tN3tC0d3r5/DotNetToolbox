namespace DotNetToolbox.AI.Chats;

public interface IChatHandler<TChat, out TOptions, TMessage>
    where TChat : class, IChat<TOptions, TMessage>
    where TOptions : class, IChatOptions, new()
    where TMessage : class, IMessage {
    Task<TChat> Start(CancellationToken ct = default);
    Task<TChat> Start(Action<TOptions> configure, CancellationToken ct = default);
    Task<TMessage> SendMessage<TRequest>(TChat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class;
}
