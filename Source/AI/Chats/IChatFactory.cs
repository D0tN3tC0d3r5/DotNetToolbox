namespace DotNetToolbox.AI.Chats;

public interface IChatFactory {
    Task<TChat> Create<TChat, TBuilder, TOptions, TMessage>(string userName, Action<TBuilder> configure, CancellationToken ct = default)
        where TChat : IChat<TOptions>
        where TOptions : ChatOptions, new()
        where TMessage : Message;
}
