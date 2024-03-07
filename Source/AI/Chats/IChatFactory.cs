namespace DotNetToolbox.AI.Chats;

public interface IChatFactory {
    Task<TChat> Create<TChat, TBuilder, TOptions, TMessage>(string userName, Action<TBuilder> configure, CancellationToken ct = default)
        where TChat : class, IChat<TOptions, TMessage>
        where TOptions : class, IChatOptions, new()
        where TMessage : class, IMessage;
}
