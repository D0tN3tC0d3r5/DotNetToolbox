namespace DotNetToolbox.AI.Actors;

public abstract class Actor<TApiClient, TOptions, TApiRequest, TApiResponse>(
        IAgent agent,
        World world,
        IHttpClientProvider httpClientProvider,
        ILogger<TApiClient> logger)
    : IRequestHandler
    where TApiClient : Actor<TApiClient, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {

    public IAgent Agent { get; } = agent;
    protected World World { get; } = world;
    protected ILogger<TApiClient> Logger = logger;

    protected virtual string CreateSystemMessage(IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(World.ToString());
        builder.AppendLine(Agent.Profile.ToString());
        builder.AppendLine(Agent.Skills.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    protected virtual Task<bool> IsRequestedCompleted(IRequestSource source, IChat chat, CancellationToken ct)
        => Task.FromResult(true);

    protected abstract TApiRequest CreateRequest(IRequestSource source, IChat chat);
    protected abstract Message CreateResponse(IChat chat, TApiResponse response);
    public virtual async Task RespondTo(IRequestSource source, IChat chat, CancellationToken ct) {
        if (ct.IsCancellationRequested) return;
        var isCompleted = false;
        while (!isCompleted) {
            Logger.LogDebug("Sending request...");
            var result = await Submit(source, chat, ct);
            if (!result.IsOk) return;
            Logger.LogDebug("Response received.");
            isCompleted = await IsRequestedCompleted(source, chat, ct);
        }
        await source.RespondWith(chat.Id, chat.Messages[^1], ct);
        Logger.LogDebug("Request completed.");
    }

    private async Task<HttpResult> Submit(IRequestSource source, IChat chat, CancellationToken ct = default) {
        var request = CreateRequest(source, chat);
        var content = JsonContent.Create(request, options: IAgentOptions.SerializerOptions);
        var httpClient = httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Agent.Options.ApiEndpoint, content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<TApiResponse>(json, IAgentOptions.SerializerOptions)!;
            var responseMessage = CreateResponse(chat, apiResponse);
            chat.Messages.Add(responseMessage);
            return HttpResult.Ok();
        }
        catch (Exception ex) {
            Logger.LogWarning(ex, "Request failed!");
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
