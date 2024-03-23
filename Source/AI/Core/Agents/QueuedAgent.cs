namespace DotNetToolbox.AI.Agents;

public abstract class QueuedAgent<TAgent, TOptions, TMapper, TRequest, TResponse>(
        World world,
        TOptions options,
        Persona persona,
        IHttpClientProvider httpClientProvider,
        ILogger<TAgent> logger)
    : BackgroundAgent<TAgent, TOptions, TMapper, TRequest, TResponse>(world, options, persona, httpClientProvider, logger),
      IQueuedAgent
    where TAgent : QueuedAgent<TAgent, TOptions, TMapper, TRequest, TResponse>
    where TOptions : class, IAgentOptions, new()
    where TMapper : class, IMapper, new()
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    private readonly Queue<RequestPackage> _receivedRequests = [];

    protected override Task Execute(CancellationToken ct)
        => _receivedRequests.TryDequeue(out var request)
               ? ProcessRequest(request, ct)
               : Task.CompletedTask;

    public override Task<HttpResult> HandleRequest(IConsumer source, IChat chat, CancellationToken ct) {
        var package = new RequestPackage(source, chat);
        _receivedRequests.Enqueue(package);
        return HttpResult.OkTask();
    }

    [SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Irrelevant here.")]
    private Task ProcessRequest(RequestPackage package, CancellationToken ct)
        => base.HandleRequest(package.Source, package.Chat, ct);
}
