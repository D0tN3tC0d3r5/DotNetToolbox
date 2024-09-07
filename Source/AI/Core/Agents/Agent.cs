namespace DotNetToolbox.AI.Agents;

public abstract class Agent<TAgent> : IAgent
    where TAgent : Agent<TAgent> {
    private readonly IHttpClientProvider _httpClientProvider;

    public Agent(string provider, IServiceProvider services, ILogger<TAgent> logger) {
        var httpClientProviderAccessor = services.GetRequiredService<IHttpClientProviderAccessor>();
        _httpClientProvider = httpClientProviderAccessor.Get(provider);
        Logger = logger;
    }

    protected ILogger<TAgent> Logger { get; }

    public AgentSettings Settings { get; } = new AgentSettings();

    public virtual async Task<HttpResult<string>> SendRequest(IMessages messages, JobContext jobContext, CancellationToken ct = default) {
        try {
            var count = 1;
            var hasFinished = false;
            var result = string.Empty;
            while (!hasFinished) {
                Logger.LogDebug("Sending request {RequestNumber} for {ChatId}...", count++, messages.Id);
                var request = CreateRequest(messages, jobContext);
                var mediaType = MediaTypeWithQualityHeaderValue.Parse(HttpClientOptions.DefaultContentType);
                var content = JsonContent.Create(request, mediaType, options: IAgentSettings.SerializerOptions);
                var httpClient = _httpClientProvider.GetHttpClient();
                var chatEndpoint = _httpClientProvider.Options.Endpoints["Chat"];
                var httpResult = await httpClient.PostAsync(chatEndpoint, content, ct).ConfigureAwait(false);
                switch (httpResult.StatusCode) {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        Logger.LogDebug("Authentication failed.");
                        return HttpResult.Unauthorized<string>();
                    case HttpStatusCode.NotFound:
                        Logger.LogDebug("Agent endpoint not found.");
                        return HttpResult.NotFound<string>();
                    case HttpStatusCode.BadRequest:
                        Logger.LogDebug("Invalid request.");
                        var input = JsonSerializer.Serialize(request, IAgentSettings.SerializerOptions);
                        var errorResponse = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                        return HttpResult.BadRequest(string.Empty, new ValidationError($"""
                            Request: {string.Join("\n", messages.Select(x => x.Text))}
                            Error: {errorResponse};
                            """));
                    default:
                        Logger.LogDebug("Response received.");
                        var response = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                        var answer = GetAnswer(response);
                        result += answer;
                        hasFinished = HasFinished(response);
                        if (!hasFinished) {
                            Logger.LogDebug("Response is incomplete.");
                            messages.AppendUserMessage(answer);
                        }
                        break;
                }
            }
            Logger.LogDebug("Request completed.");
            return HttpResult.Ok(result);
        }
        catch (Exception ex) {
            Logger.LogWarning(ex, "Request failed!");
            return HttpResult.InternalError<string>(ex);
        }
    }

    protected abstract string CreateRequest(IMessages messages, JobContext jobContext);
    protected abstract bool HasFinished(string response);
    protected abstract string GetAnswer(string response);
}

// public abstract class HttpConnection<TAgent, TRequest>(string provider, IHttpClientProviderAccessor httpClientProviderAccessor, ILogger<TAgent> logger)
//     : IHttpConnection
//     where TAgent : HttpConnection<TAgent, TRequest>
//     where TRequest : class, IChatRequest {
//     private readonly IHttpClientProvider _httpClientProvider = httpClientProviderAccessor.Get(provider);

//     protected HttpConnection(IHttpClientProviderAccessor factory, ILogger<TAgent> logger)
//         : this(null!, factory, logger) {
//     }

//     protected ILogger<TAgent> Logger { get; } = logger;

//     public HttpConnectionSettings Settings { get; } = new HttpConnectionSettings();
//     public Persona Persona { get; set; } = default!;
//     public List<Tool> Tools { get; } = [];

//     public virtual async Task<HttpResult> SendRequest(IJob job, IMessages messages, CancellationToken ct) {
//         try {
//             var count = 1;
//             var hasFinished = false;
//             while (!hasFinished) {
//                 Logger.LogDebug("Sending request {RequestNumber} for {ChatId}...", count++, messages.Id);
//                 var request = CreateRequest(job, messages);
//                 var mediaType = MediaTypeWithQualityHeaderValue.Parse(HttpClientOptions.DefaultContentType);
//                 var content = JsonContent.Create(request, mediaType, options: IHttpConnectionSettings.SerializerOptions);
//                 var httpClient = _httpClientProvider.GetHttpClient();
//                 var chatEndpoint = _httpClientProvider.Options.Endpoints["Chat"];
//                 var httpResult = await httpClient.PostAsync(chatEndpoint, content, ct).ConfigureAwait(false);
//                 switch (httpResult.StatusCode) {
//                     case HttpStatusCode.BadRequest:
//                         Logger.LogDebug("Invalid request.");
//                         var input = JsonSerializer.Serialize(request, IHttpConnectionSettings.SerializerOptions);
//                         var response = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
//                         var errorMessage = $"""
//                                             RequestPackage: {input}
//                                             ResponseContent: {response};
//                                             """;
//                         return HttpResult.BadRequest(errorMessage);
//                     case HttpStatusCode.Unauthorized:
//                     case HttpStatusCode.Forbidden:
//                         Logger.LogDebug("Authentication failed.");
//                         return HttpResult.Unauthorized();
//                     case HttpStatusCode.NotFound:
//                         Logger.LogDebug("Wrong agent endpoint.");
//                         return HttpResult.NotFound();
//                     default:
//                         Logger.LogDebug("Response received.");
//                         var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
//                         var aiResponse = typeof(TResponse) == typeof(string)
//                             ? (TResponse)(object)json
//                             : JsonSerializer.Deserialize<TResponse>(json, IHttpConnectionSettings.SerializerOptions)!;
//                         hasFinished = UpdateChat(messages, aiResponse);
//                         if (!hasFinished) Logger.LogDebug("Response is incomplete.");
//                         break;
//                 }
//             }
//             Logger.LogDebug("Request completed.");
//             return HttpResult.Ok();
//         }
//         catch (Exception ex) {
//             Logger.LogWarning(ex, "Request failed!");
//             return HttpResult.InternalError(ex);
//         }
//     }

//     protected abstract TRequest CreateRequest(IJob job, IMessages messages);
//     protected abstract bool UpdateChat(IMessages chat, TResponse response);
// }

// public abstract class HttpConnection<TAgent, TRequest, TResponse>(string provider, IHttpClientProviderAccessor httpClientProviderAccessor, ILogger<TAgent> logger)
//     : IHttpConnection
//     where TAgent : HttpConnection<TAgent, TRequest, TResponse>
//     where TRequest : class, IChatRequest
//     where TResponse : class, IChatResponse {
//     private readonly IHttpClientProvider _httpClientProvider = httpClientProviderAccessor.Get(provider);

//     protected HttpConnection(IHttpClientProviderAccessor factory, ILogger<TAgent> logger)
//         : this(null!, factory, logger) {
//     }

//     protected ILogger<TAgent> Logger { get; } = logger;

//     public HttpConnectionSettings Settings { get; } = new HttpConnectionSettings();
//     public Persona Persona { get; set; } = default!;
//     public List<Tool> Tools { get; } = [];

//     public virtual async Task<HttpResult> SendRequest(IJob job, IMessages messages, CancellationToken ct) {
//         try {
//             var count = 1;
//             var hasFinished = false;
//             while (!hasFinished) {
//                 Logger.LogDebug("Sending request {RequestNumber} for {ChatId}...", count++, messages.Id);
//                 var request = CreateRequest(job, messages);
//                 var mediaType = MediaTypeWithQualityHeaderValue.Parse(HttpClientOptions.DefaultContentType);
//                 var content = JsonContent.Create(request, mediaType, options: IHttpConnectionSettings.SerializerOptions);
//                 var httpClient = _httpClientProvider.GetHttpClient();
//                 var chatEndpoint = _httpClientProvider.Options.Endpoints["Chat"];
//                 var httpResult = await httpClient.PostAsync(chatEndpoint, content, ct).ConfigureAwait(false);
//                 switch (httpResult.StatusCode) {
//                     case HttpStatusCode.BadRequest:
//                         Logger.LogDebug("Invalid request.");
//                         var input = JsonSerializer.Serialize(request, IHttpConnectionSettings.SerializerOptions);
//                         var response = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
//                         var errorMessage = $"""
//                                             RequestPackage: {input}
//                                             ResponseContent: {response};
//                                             """;
//                         return HttpResult.BadRequest(errorMessage);
//                     case HttpStatusCode.Unauthorized:
//                     case HttpStatusCode.Forbidden:
//                         Logger.LogDebug("Authentication failed.");
//                         return HttpResult.Unauthorized();
//                     case HttpStatusCode.NotFound:
//                         Logger.LogDebug("Wrong agent endpoint.");
//                         return HttpResult.NotFound();
//                     default:
//                         Logger.LogDebug("Response received.");
//                         var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
//                         var aiResponse = typeof(TResponse) == typeof(string)
//                             ? (TResponse)(object)json
//                             : JsonSerializer.Deserialize<TResponse>(json, IHttpConnectionSettings.SerializerOptions)!;
//                         hasFinished = UpdateChat(messages, aiResponse);
//                         if (!hasFinished) Logger.LogDebug("Response is incomplete.");
//                         break;
//                 }
//             }
//             Logger.LogDebug("Request completed.");
//             return HttpResult.Ok();
//         }
//         catch (Exception ex) {
//             Logger.LogWarning(ex, "Request failed!");
//             return HttpResult.InternalError(ex);
//         }
//     }

//     protected abstract TRequest CreateRequest(IJob job, IMessages messages);
//     protected abstract bool UpdateChat(IMessages chat, TResponse response);
// }
