namespace DotNetToolbox.AI.Agents;

public abstract class Agent<TAgent, TOptions, TApiRequest, TApiResponse>(
        World world,
        TOptions options,
        IPersona persona,
        IHttpClientProvider httpClientProvider,
        ILogger<TAgent> logger)
    : IAgent
    where TAgent : Agent<TAgent, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {

    public IPersona Persona { get; } = persona;
    IAgentOptions IAgent.Options => Options;
    public TOptions Options { get; set; } = IsValidOrDefault(options, new());
    protected World World { get; } = world;
    protected ILogger<TAgent> Logger = logger;

    protected virtual string CreateSystemMessage(IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(World.ToString());
        builder.AppendLine(Persona.Profile.ToString());
        builder.AppendLine(Persona.Skills.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    protected virtual Task<bool> IsRequestedCompleted(IConsumer source, IChat chat, CancellationToken ct)
        => Task.FromResult(true);

    protected abstract TApiRequest CreateRequest(IConsumer source, IChat chat);
    protected abstract Message CreateResponse(IChat chat, TApiResponse response);
    public virtual async Task<HttpResult> HandleRequest(IConsumer source, IChat chat, CancellationToken ct) {
        var isCompleted = false;
        while (!isCompleted) {
            Logger.LogDebug("Sending request...");
            var result = await Submit(source, chat, ct);
            if (!result.IsOk) return result;
            Logger.LogDebug("Response received.");
            isCompleted = await IsRequestedCompleted(source, chat, ct);
        }
        await source.ProcessResponse(chat.Id, chat.Messages[^1], ct);
        Logger.LogDebug("Request completed.");
        return HttpResult.Ok();
    }

    private async Task<HttpResult> Submit(IConsumer source, IChat chat, CancellationToken ct = default) {
        var request = CreateRequest(source, chat);
        var content = JsonContent.Create(request, options: IAgentOptions.SerializerOptions);
        var httpClient = httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Options.ApiEndpoint, content, ct).ConfigureAwait(false);
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
