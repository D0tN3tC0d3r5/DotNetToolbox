namespace DotNetToolbox.AI.Agents;

public abstract class StandardAgent<TAgent, TMapper, TRequest, TResponse>(
        World world,
        Persona persona,
        IAgentOptions options,
        IHttpClientProvider httpClientProvider,
        ILogger<TAgent> logger)
    : IStandardAgent
    where TAgent : StandardAgent<TAgent, TMapper, TRequest, TResponse>
    where TMapper : class, IMapper, new()
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    protected ILogger<TAgent> Logger { get; } = logger;
    protected TMapper Mapper { get; } = new();

    public World World { get; } = world;
    public IAgentOptions Options { get; set; } = IsValidOrDefault(options, new AgentOptions());
    public Persona Persona { get; } = persona;

    public virtual async Task<HttpResult> SendRequest(IResponseAwaiter source, IChat chat, CancellationToken ct) {
        try {
            var isCompleted = false;
            var count = 1;
            while (!isCompleted) {
                Logger.LogDebug("Sending request {count}...", count++);
                var result = await Submit(chat, ct);
                if (!result.IsOk) return result;
                await source.StartWait(ct);
                isCompleted = source switch {
                    IResponseConsumer ac => ac.VerifyResponse(chat.Id, chat.Messages[^1]),
                    IAsyncResponseConsumer ac => await ac.VerifyResponse(chat.Id, chat.Messages[^1], ct),
                    _ => throw new NotSupportedException(nameof(source)),
                };
            }

            switch (source) {
                case IResponseConsumer ac: ac.ResponseApproved(chat.Id, chat.Messages[^1]); break;
                case IAsyncResponseConsumer ac: await ac.ResponseApproved(chat.Id, chat.Messages[^1], ct); break;
            }
            Logger.LogDebug("Request completed.");
            return HttpResult.Ok();
        }
        catch (Exception ex) {
            Logger.LogError(ex, "An error occurred while sending the request!");
            return HttpResult.InternalError(ex);
        }
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
