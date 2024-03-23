namespace DotNetToolbox.AI.Agents;

public abstract class QueuedAgent<TAgent, TMapper, TRequest, TResponse>(
        World world,
        Persona persona,
        IAgentOptions options,
        IHttpClientProvider httpClientProvider,
        ILogger<TAgent> logger)
    : BackgroundAgent<TAgent, TMapper, TRequest, TResponse>(world, persona, options, httpClientProvider, logger),
      IQueuedAgent
    where TAgent : QueuedAgent<TAgent, TMapper, TRequest, TResponse>
    where TMapper : class, IMapper, new()
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    private readonly Queue<RequestPackage> _receivedRequests = [];

    protected override Task Execute(CancellationToken ct)
        => _receivedRequests.TryDequeue(out var request)
               ? ProcessRequest(request, ct)
               : Task.CompletedTask;

    public override Task<HttpResult> SendRequest(IResponseAwaiter source, IChat chat, CancellationToken ct) {
        var package = new RequestPackage(source, chat);
        _receivedRequests.Enqueue(package);
        return HttpResult.OkTask();
    }

    private Task<HttpResult> ProcessRequest(RequestPackage package, CancellationToken ct)
        => base.SendRequest(package.Source, package.Chat, ct);
}
