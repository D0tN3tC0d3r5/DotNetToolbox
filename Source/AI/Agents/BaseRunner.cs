namespace DotNetToolbox.AI.Agents;

public abstract class BaseRunner<TRunner, TOptions, TApiRequest, TApiResponse>(
        IAgent agent,
        World world,
        IHttpClientProvider httpClientProvider,
        ILogger<TRunner> logger)
    : IAgentRunner
    where TRunner : BaseRunner<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {

    public IAgent Agent { get; } = agent;
    protected World World { get; } = world;

    // this should be a fire and forget method.
    // Use the cancellation token to stop the agent.
    public async void Run(CancellationToken ct) {
        logger.LogInformation("Starting runner...");
        try {
            while (!ct.IsCancellationRequested) {
                await Execute(ct);
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

    public abstract Task ReceiveRequest(IRequestSource source, IChat chat, CancellationToken token);
    protected abstract Task ProcessRequest(IRequestSource source, IChat chat, CancellationToken token);
    public abstract Task ReceiveResponse(string chatId, Message response, CancellationToken token);
    protected abstract Task ProcessResponse(string chatId, Message response, CancellationToken token);

    protected virtual Task Execute(CancellationToken token) => Task.CompletedTask;
    protected virtual string CreateSystemMessage() => "You are a helpful agent.";
    protected abstract TApiRequest CreateRequest(IRequestSource source, IChat chat, CancellationToken token);
    protected abstract Message CreateResponseMessage(IChat chat, TApiResponse response);
    protected virtual Task<bool> IsRequestedCompleted(IRequestSource source, IChat chat, CancellationToken ct)
        => Task.FromResult(true);

    protected async Task SubmitRequest(IRequestSource source, IChat chat, CancellationToken ct) {
        if (ct.IsCancellationRequested) return;
        var isCompleted = false;
        while (!isCompleted) {
            var result = await Submit(source, chat, ct);
            if (!result.IsOk) return;
            isCompleted = await IsRequestedCompleted(source, chat, ct);
        }
        await source.ReceiveResponse(chat.Id, chat.Messages[^1], ct);
    }

    private async Task<HttpResult> Submit(IRequestSource source, IChat chat, CancellationToken ct = default) {
        var request = CreateRequest(source, chat, ct);
        var content = JsonContent.Create(request, options: IAgentOptions.SerializerOptions);
        var httpClient = httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Agent.Options.ApiEndpoint, content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<TApiResponse>(json, IAgentOptions.SerializerOptions)!;
            var responseMessage = CreateResponseMessage(chat, apiResponse);
            chat.Messages.Add(responseMessage);
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
}
