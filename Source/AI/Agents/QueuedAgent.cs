namespace DotNetToolbox.AI.Agents;

public abstract class QueuedAgent<TRunner, TOptions, TApiRequest, TApiResponse>(
        World world,
        TOptions options,
        IPersona persona,
        IHttpClientProvider httpClientProvider,
        ILogger<TRunner> logger)
    : BackgroundAgent<TRunner, TOptions, TApiRequest, TApiResponse>(world, options, persona, httpClientProvider, logger)
    where TRunner : QueuedAgent<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {
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

//public abstract class QueuedRunner2<TRunner, TOptions, TChatRequest, TChatResponse>(
//        IAgent agent,
//        World world,
//        IHttpClientProvider httpClientProvider,
//        ILogger<TRunner> logger)
//    : BackgroundAgent<TRunner, TOptions, TChatRequest, TChatResponse>(agent, world, httpClientProvider, logger),
//      IBackgroundAgent
//    where TRunner : QueuedRunner2<TRunner, TOptions, TChatRequest, TChatResponse>
//    where TOptions : class, IAgentOptions, new()
//    where TChatRequest : class
//    where TChatResponse : class {
//    private readonly Queue<RequestPackage> _receivedRequests = [];
//    private readonly Queue<ResponsePackage> _receivedResponses = [];

//    // this should be a fire and forget method.
//    // Use the cancellation token to stop the agent.
//    protected override async Task Execute(CancellationToken ct) {
//        if (ct.IsCancellationRequested) return;
//        if (_receivedRequests.TryDequeue(out var request)) await ProcessRequest(request, ct);
//        if (ct.IsCancellationRequested) return;
//        if (_receivedResponses.TryDequeue(out var response)) await ProcessResponse(response, ct);
//    }

//    public virtual Task HandleRequest(IConsumer source, IChat chat, CancellationToken ct) {
//        if (ct.IsCancellationRequested) return Task.CompletedTask;
//        var package = new RequestPackage(source, chat);
//        _receivedRequests.Enqueue(package);
//        return Task.CompletedTask;
//    }

//    public virtual Task ProcessResponse(string chatId, Message response, CancellationToken ct) {
//        if (ct.IsCancellationRequested) return Task.CompletedTask;
//        var package = new ResponsePackage(chatId, response);
//        _receivedResponses.Enqueue(package);
//        return Task.CompletedTask;
//    }

//    private Task ProcessRequest(RequestPackage package, CancellationToken ct) {
//        if (ct.IsCancellationRequested) return Task.CompletedTask;
//        return SubmitRequest(package.Source, package.Chat, ct);
//    }

//    private Task ProcessResponse(ResponsePackage package, CancellationToken ct) {
//        if (ct.IsCancellationRequested) return Task.CompletedTask;
//        return ProcessResponse(package.ChatId, package.Message, ct);
//    }
//}
