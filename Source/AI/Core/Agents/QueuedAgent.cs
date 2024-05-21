namespace DotNetToolbox.AI.Agents;

public abstract class QueuedAgent<TAgent, TRequest, TResponse>(string provider,
                                                                        IHttpClientProviderFactory factory,
                                                                        ILogger<TAgent> logger)
    : BackgroundAgent<TAgent, TRequest, TResponse>(provider, factory, logger),
      IQueuedAgent
    where TAgent : QueuedAgent<TAgent, TRequest, TResponse>
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    private readonly Queue<RequestPackage> _receivedRequests = [];

    protected override Task Execute(CancellationToken ct)
        => _receivedRequests.TryDequeue(out var request)
               ? ProcessRequest(request, ct)
               : Task.CompletedTask;

    public override Task<HttpResult> SendRequest(IResponseAwaiter source, IChat chat, int? number, CancellationToken ct) {
        var package = new RequestPackage(source, chat, number);
        _receivedRequests.Enqueue(package);
        return HttpResult.OkTask();
    }

    private Task<HttpResult> ProcessRequest(RequestPackage package, CancellationToken ct)
        => base.SendRequest(package.Source, package.Chat, package.AgentNumber, ct);
}
