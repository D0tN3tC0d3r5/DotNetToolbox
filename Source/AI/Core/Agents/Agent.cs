namespace DotNetToolbox.AI.Agents;

public abstract class Agent<TAgent, TRequest, TResponse>(string provider,
                                                         IHttpClientProviderFactory factory,
                                                         ILogger<TAgent> logger)
    : IAgent
    where TAgent : Agent<TAgent, TRequest, TResponse>
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    private readonly IHttpClientProvider _httpClientProvider = factory.Create(provider);

    protected ILogger<TAgent> Logger { get; } = logger;

    public AgentModel Model { get; set; } = default!;
    public World World { get; set; } = default!;
    public Persona Persona { get; set; } = default!;
    public List<Tool> Tools { get; } = [];

    public virtual async Task<HttpResult> SendRequest(IJob job, IChat chat, CancellationToken ct) {
        try {
            var count = 1;
            var hasFinished = false;
            while (!hasFinished) {
                Logger.LogDebug("Sending request {RequestNumber} for {ChatId}...", count++, chat.Id);
                var request = CreateRequest(job, chat);
                var mediaType = MediaTypeWithQualityHeaderValue.Parse(HttpClientOptions.DefaultContentType);
                var content = JsonContent.Create(request, mediaType, options: IAgentModel.SerializerOptions);
                var httpClient = _httpClientProvider.GetHttpClient();
                var chatEndpoint = _httpClientProvider.Options.Endpoints["Chat"];
                var httpResult = await httpClient.PostAsync(chatEndpoint, content, ct).ConfigureAwait(false);
                switch (httpResult.StatusCode) {
                    case HttpStatusCode.BadRequest:
                        Logger.LogDebug("Invalid request.");
                        var input = JsonSerializer.Serialize(request, IAgentModel.SerializerOptions);
                        var response = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                        var errorMessage = $"""
                                            RequestPackage: {input}
                                            ResponseContent: {response};
                                            """;
                        return HttpResult.BadRequest(errorMessage);
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        Logger.LogDebug("Authentication failed.");
                        return HttpResult.Unauthorized();
                    case HttpStatusCode.NotFound:
                        Logger.LogDebug("Wrong agent endpoint.");
                        return HttpResult.NotFound();
                    default:
                        Logger.LogDebug("Response received.");
                        var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                        var apiResponse = JsonSerializer.Deserialize<TResponse>(json, IAgentModel.SerializerOptions)!;
                        hasFinished = UpdateChat(chat, apiResponse);
                        if (!hasFinished) Logger.LogDebug("Response is incomplete.");
                        break;
                }
            }
            Logger.LogDebug("Request completed.");
            return HttpResult.Ok();
        }
        catch (Exception ex) {
            Logger.LogWarning(ex, "Request failed!");
            return HttpResult.InternalError(ex);
        }
    }

    protected abstract TRequest CreateRequest(IJob job, IChat chat);
    protected abstract bool UpdateChat(IChat chat, TResponse response);
}
