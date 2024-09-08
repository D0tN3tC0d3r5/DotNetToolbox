namespace DotNetToolbox.AI.Agents;

public abstract class Agent<TAgent, TRequest, TResponse>
    : IAgent
    where TAgent : Agent<TAgent, TRequest, TResponse> {
    private readonly IHttpClientProvider _httpClientProvider;

    protected Agent(string provider, IServiceProvider services, ILogger<TAgent> logger) {
        var httpClientProviderAccessor = services.GetRequiredService<IHttpClientProviderAccessor>();
        _httpClientProvider = httpClientProviderAccessor.Get(provider);
        Logger = logger;
    }

    protected ILogger<TAgent> Logger { get; }

    public AgentSettings Settings { get; } = new();

    public virtual async Task<HttpResult<Message>> SendRequest(IChat chat, JobContext context, CancellationToken ct = default) {
        try {
            var hasFinished = false;
            var resultMessage = new Message(MessageRole.Assistant);
            while (!hasFinished) {
                Logger.LogDebug("Sending request {RequestNumber} for {ChatId}...", ++chat.CallCount, chat.Id);
                var httpRequest = CreateRequest(chat, context);
                var mediaType = MediaTypeWithQualityHeaderValue.Parse(HttpClientOptions.DefaultContentType);
                var httpRequestContent = JsonContent.Create(httpRequest, mediaType, options: IAgentSettings.SerializerOptions);
                using var httpClient = _httpClientProvider.GetHttpClient();
                var chatEndpoint = _httpClientProvider.Options.Endpoints["Chat"];
                using var httpResponse = await httpClient.PostAsync(chatEndpoint, httpRequestContent, ct).ConfigureAwait(false);
                switch (httpResponse.StatusCode) {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        Logger.LogDebug("Authentication failed.");
                        return HttpResult.Unauthorized<Message>();
                    case HttpStatusCode.NotFound:
                        Logger.LogDebug("Agent endpoint not found.");
                        return HttpResult.NotFound<Message>();
                    case HttpStatusCode.BadRequest:
                        Logger.LogDebug("Invalid request.");
                        var errorResponse = await httpResponse.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                        return HttpResult.BadRequest(resultMessage, new ValidationError($"""
                            Request: {string.Join("\n", chat.Select(x => x.Text))}
                            Error: {errorResponse};
                            """));
                    default:
                        Logger.LogDebug("Response received.");
                        var httpResponseContent = await httpResponse.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                        var response = JsonSerializer.Deserialize<TResponse>(httpResponseContent, IAgentSettings.SerializerOptions)!;
                        hasFinished = ProcessResponse(chat, response, resultMessage);
                        if (!hasFinished) Logger.LogDebug("Response is incomplete.");
                        break;
                }
            }
            Logger.LogDebug("Request completed.");
            return HttpResult.Ok(resultMessage);
        }
        catch (Exception ex) {
            Logger.LogWarning(ex, "Request failed!");
            return HttpResult.InternalError<Message>(ex);
        }
    }

    protected abstract TRequest CreateRequest(IChat chat, JobContext context);
    protected abstract bool ProcessResponse(IChat chat, TResponse response, Message resultMessage);
    //protected abstract string GetAnswer(TResponse response);
    //protected abstract void UpdateUsage(IChat chat, TResponse response);
    //protected abstract bool HasFinished(TResponse response);
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
