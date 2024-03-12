namespace DotNetToolbox.AI.Actors;

public abstract class QueuedActor<TRunner, TOptions, TApiRequest, TApiResponse>(
        IAgent agent,
        World world,
        IHttpClientProvider httpClientProvider,
        ILogger<TRunner> logger)
    : BackgroundActor<TRunner, TOptions, TApiRequest, TApiResponse>(agent, world, httpClientProvider, logger)
    where TRunner : QueuedActor<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {
    private readonly Queue<RequestPackage> _receivedRequests = [];

    protected override Task Execute(CancellationToken ct) {
        if (ct.IsCancellationRequested) return Task.CompletedTask;
        if (_receivedRequests.TryDequeue(out var request)) return ProcessRequest(request, ct);
        return Task.CompletedTask;
    }

    public override Task RespondTo(IRequestSource source, IChat chat, CancellationToken ct) {
        if (ct.IsCancellationRequested) return Task.CompletedTask;
        var package = new RequestPackage(source, chat);
        _receivedRequests.Enqueue(package);
        return Task.CompletedTask;
    }

    private Task ProcessRequest(RequestPackage package, CancellationToken ct) {
        if (ct.IsCancellationRequested) return Task.CompletedTask;
        return base.RespondTo(package.Source, package.Chat, ct);
    }
}

//public abstract class QueuedRunner2<TRunner, TOptions, TApiRequest, TApiResponse>(
//        IAgent agent,
//        World world,
//        IHttpClientProvider httpClientProvider,
//        ILogger<TRunner> logger)
//    : BackgroundActor<TRunner, TOptions, TApiRequest, TApiResponse>(agent, world, httpClientProvider, logger),
//      IBackgroundRunner
//    where TRunner : QueuedRunner2<TRunner, TOptions, TApiRequest, TApiResponse>
//    where TOptions : class, IAgentOptions, new()
//    where TApiRequest : class
//    where TApiResponse : class {
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

//    public virtual Task RespondTo(IRequestSource source, IChat chat, CancellationToken ct) {
//        if (ct.IsCancellationRequested) return Task.CompletedTask;
//        var package = new RequestPackage(source, chat);
//        _receivedRequests.Enqueue(package);
//        return Task.CompletedTask;
//    }

//    public virtual Task RespondWith(string chatId, Message response, CancellationToken ct) {
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
