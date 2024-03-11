namespace DotNetToolbox.AI.Agents;

public abstract class QueuedRunner<TRunner, TOptions, TApiRequest, TApiResponse>(
        IAgent agent,
        World world,
        IHttpClientProvider httpClientProvider,
        ILogger<TRunner> logger)
    : BaseRunner<TRunner, TOptions, TApiRequest, TApiResponse>(agent, world, httpClientProvider, logger),
      IAgentRunner
    where TRunner : QueuedRunner<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {
    private readonly Queue<RequestPackage> _receivedRequests = [];
    private readonly Queue<ResponsePackage> _receivedResponses = [];

    // this should be a fire and forget method.
    // Use the cancellation token to stop the agent.
    protected override async Task Execute(CancellationToken ct) {
        if (ct.IsCancellationRequested) return;
        if (_receivedRequests.TryDequeue(out var request)) await ProcessRequest(request, ct);
        if (ct.IsCancellationRequested) return;
        if (_receivedResponses.TryDequeue(out var response)) await ProcessResponse(response, ct);
    }

    public override Task ReceiveRequest(IRequestSource source, IChat chat, CancellationToken ct) {
        var package = new RequestPackage(source, chat, ct);
        _receivedRequests.Enqueue(package);
        return Task.CompletedTask;
    }

    public override Task ReceiveResponse(string chatId, Message response, CancellationToken ct) {
        var package = new ResponsePackage(chatId, response, ct);
        _receivedResponses.Enqueue(package);
        return Task.CompletedTask;
    }

    private Task ProcessRequest(RequestPackage package, CancellationToken ct) {
        if (ct.IsCancellationRequested) return Task.CompletedTask;
        var ts = CancellationTokenSource.CreateLinkedTokenSource(ct, package.Token);
        return SubmitRequest(package.Source, package.Chat, ts.Token);
    }

    private Task ProcessResponse(ResponsePackage package, CancellationToken ct) {
        if (ct.IsCancellationRequested) return Task.CompletedTask;
        var ts = CancellationTokenSource.CreateLinkedTokenSource(ct, package.Token);
        return ProcessResponse(package.ChatId, package.Message, ts.Token);
    }
}
