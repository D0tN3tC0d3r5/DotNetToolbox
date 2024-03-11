namespace DotNetToolbox.AI.Agents;

public abstract class AgentRunner<TRunner, TOptions, TApiRequest, TApiResponse>(
        IAgent agent,
        World world,
        IHttpClientProvider httpClientProvider,
        ILogger<TRunner> logger)
    : IAgentRunner
    where TRunner : AgentRunner<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {
    private readonly Queue<RequestPackage> _receivedRequests = [];
    private readonly Queue<ResponsePackage> _receivedResponses = [];

    public IAgent Agent { get; } = agent;
    protected World World { get; } = world;

    // this should be a fire and forget method.
    // Use the cancellation token to stop the agent.
    public async void Run(CancellationToken ct) {
        logger.LogInformation("Starting runner...");
        try {
            while (!ct.IsCancellationRequested) {
                if (_receivedRequests.TryDequeue(out var request)) await ProcessRequest(request, ct);
                if (ct.IsCancellationRequested) return;
                if (_receivedResponses.TryDequeue(out var response)) ProcessResponse(response);
                if (ct.IsCancellationRequested) return;
                await Task.Delay(100, ct);
            }
        }
        catch (OperationCanceledException ex) {
            logger.LogWarning(ex, "Runner cancellation requested!");
        }
        catch (Exception ex) {
            logger.LogError(ex, "An error occurred while executing the runner!");
            throw;
        }
        logger.LogInformation("Runner stopped.");
    }

    protected abstract TApiRequest CreateRequest(RequestPackage package);
    protected virtual string CreateSystemMessage() => "You are a helpful agent.";

    public CancellationTokenSource PostRequest(IOriginator from, IChat chat) {
        var tokenSource = new CancellationTokenSource();
        var request = new RequestPackage(from, chat, tokenSource.Token);
        _receivedRequests.Enqueue(request);
        return tokenSource;
    }

    private async Task ProcessRequest(RequestPackage package, CancellationToken ct) {
        var ts = CancellationTokenSource.CreateLinkedTokenSource(package.Token, ct);
        if (ts.IsCancellationRequested) return;
        var result = await Submit(package, ts.Token);
        if (!result.IsOk) return;
        var isFinished = false;
        while (!isFinished)
            isFinished = await ProcessSubmissionResult(package, ct);
        var response = new ResponsePackage(package.Chat.Id, package.Chat.Messages[^1]);
        package.Source.ReceiveResponse(response);
    }

    private async Task<HttpResult> Submit(RequestPackage package, CancellationToken ct = default) {
        var request = CreateRequest(package);
        var content = JsonContent.Create(request, options: IAgentOptions.SerializerOptions);
        var httpClient = httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Agent.Options.ApiEndpoint, content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<TApiResponse>(json, IAgentOptions.SerializerOptions)!;
            var responseMessage = CreateResponseMessage(package.Chat, apiResponse);
            package.Chat.Messages.Add(responseMessage);
            return HttpResult.Ok();
        }
        catch (Exception ex) {
            switch (httpResult.StatusCode) {
                case HttpStatusCode.BadRequest:
                    var response = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                    var errorMessage = $"""
                                        RequestPackage: {JsonSerializer.Serialize(request, IAgentOptions.SerializerOptions)}
                                        ResponseContent: {response};
                                        """;
                    return HttpResult.BadRequest(errorMessage);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return HttpResult.Unauthorized();
                case HttpStatusCode.NotFound:
                    return HttpResult.NotFound();
                default:
                    return HttpResult.InternalError(ex);
            }
        }
    }

    protected abstract Message CreateResponseMessage(IChat chat, TApiResponse response);
    public void ReceiveResponse(ResponsePackage response) => _receivedResponses.Enqueue(response);
    protected virtual void ProcessResponse(ResponsePackage response) { }

    // Do something with the apiResponse from the processing agent.
    protected virtual Task<bool> ProcessSubmissionResult(RequestPackage request, CancellationToken ct)
        => Task.FromResult(true);
}
