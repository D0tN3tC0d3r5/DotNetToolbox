namespace DotNetToolbox.AI.Chats;

internal abstract class ChatHandler<THandler, TChat, TOptions>(IChatRepository<TChat> repository, IHttpClientProvider httpClientProvider, ILogger<THandler> logger)
    : IChatHandler<TChat, TOptions>
    where TChat : IChat<TOptions>
    where TOptions : ChatOptions, new() {

    protected IChatRepository<TChat> Repository { get; } = repository;
    protected HttpClient HttpClient { get; } = httpClientProvider.GetHttpClient();
    protected ILogger<THandler> Logger { get; } = logger;

    public abstract Task<TChat[]> List(CancellationToken ct = default);
    public Task<TChat> Start(string userName, CancellationToken ct = default) => Start(userName, _ => { }, ct);
    public abstract Task<TChat> Start(string userName, Action<TOptions> configure, CancellationToken ct = default);
    public abstract Task<TResponse> SendMessage<TRequest, TResponse>(TChat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class
        where TResponse : class;
    public abstract Task Terminate(TChat chat, CancellationToken ct = default);
}
