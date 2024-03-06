namespace DotNetToolbox.AI.Chats;

public interface IChatHandler<TChat, out TOptions>
    where TChat : IChat<TOptions>
    where TOptions : ChatOptions, new() {
    Task<TChat[]> List(CancellationToken ct = default);
    Task<TChat> Start(string userName, CancellationToken ct = default);
    Task<TChat> Start(string userName, Action<TOptions> configure, CancellationToken ct = default);
    Task<TResponse> SendMessage<TRequest, TResponse>(TChat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class
        where TResponse : class;
    Task Terminate(TChat chat, CancellationToken ct = default);
}
