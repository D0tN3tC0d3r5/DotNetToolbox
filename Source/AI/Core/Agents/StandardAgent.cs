namespace DotNetToolbox.AI.Agents;

public abstract class StandardAgent<TAgent, TOptions, TMapper, TRequest, TResponse>(
        World world,
        TOptions options,
        Persona persona,
        IHttpClientProvider httpClientProvider,
        ILogger<TAgent> logger)
    : IStandardAgent<TOptions>
    where TAgent : StandardAgent<TAgent, TOptions, TMapper, TRequest, TResponse>
    where TOptions : class, IAgentOptions, new()
    where TMapper : class, IMapper, new()
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    protected ILogger<TAgent> Logger { get; } = logger;
    protected TMapper Mapper { get; } = new();

    public World World { get; } = world;
    public TOptions Options { get; set; } = IsValidOrDefault(options, new());
    public Persona Persona { get; } = persona;

    protected virtual Task<bool> IsRequestedCompleted(IConsumer source, IChat chat, CancellationToken ct)
        => Task.FromResult(true);

    public virtual async Task<HttpResult> HandleRequest(IConsumer source, IChat chat, CancellationToken ct) {
        var isCompleted = false;
        while (!isCompleted) {
            Logger.LogDebug("Sending request...");
            var result = await Submit(chat, ct);
            if (!result.IsOk) return result;
            Logger.LogDebug("Response received.");
            isCompleted = await IsRequestedCompleted(source, chat, ct);
        }
        await source.ProcessResponse(chat.Id, chat.Messages[^1], ct);
        Logger.LogDebug("Request completed.");
        return HttpResult.Ok();
    }

    private async Task<HttpResult> Submit(IChat chat, CancellationToken ct = default) {
        var request = Mapper.CreateRequest(this, chat);
        var content = JsonContent.Create(request, options: IAgentOptions.SerializerOptions);
        var httpClient = httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Options.ApiEndpoint, content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var apiResponse = JsonSerializer.Deserialize<TResponse>(json, IAgentOptions.SerializerOptions)!;
            var responseMessage = Mapper.CreateResponseMessage(chat, apiResponse);
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
