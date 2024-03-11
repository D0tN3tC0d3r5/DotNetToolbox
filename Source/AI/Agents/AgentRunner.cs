namespace DotNetToolbox.AI.Agents;

public abstract class AgentRunner<TOptions, TApiRequest, TApiResponse>
    : IAgentRunner
    where TOptions : class, IAgentOptions {
    private readonly Queue<RequestPackage> _receivedRequests = [];
    private readonly Queue<ResponsePackage> _receivedResponses = [];
    private readonly IHttpClientProvider _httpClientProvider;

    public delegate void ResponseReceivedHandler(string chatId, Message message);

    protected AgentRunner(Agent<TOptions> agent, World world, IHttpClientProvider httpClientProvider) {
        Agent = agent;
        World = world;
        _httpClientProvider = httpClientProvider;
    }

    public Agent<TOptions> Agent { get; }
    protected World World { get; }
    public event ResponseReceivedHandler? OnResponseReceived;


    public async Task Start(CancellationToken ct) {
        while (!ct.IsCancellationRequested) {
            if (_receivedRequests.TryDequeue(out var request))
                await ProcessRequest(request, ct);
            if (_receivedResponses.TryDequeue(out var response))
                ProcessResponse(response);
            await Task.Delay(100, ct);
        }
    }


    protected abstract TApiRequest CreateRequest(RequestPackage package);
    public CancellationTokenSource HandleRequest(IOriginator from, IChat chat) {
        var tokenSource = new CancellationTokenSource();
        var request = new RequestPackage(from, chat, tokenSource.Token);
        _receivedRequests.Enqueue(request);
        return tokenSource;
    }

    protected abstract Message CreateResponseMessage(IChat chat, TApiResponse response);
    public void EnqueueResponse(ResponsePackage response) => _receivedResponses.Enqueue(response);

    // Do something with the apiResponse from the processing agent.
    private async Task ProcessRequest(RequestPackage package, CancellationToken ct) {
        var ts = CancellationTokenSource.CreateLinkedTokenSource(package.Token, ct);
        if (ts.IsCancellationRequested) return;
        var result = await Submit(package, ts.Token);
        if (!result.IsOk) return;
        var isFinished = false;
        while (!isFinished)
            isFinished = await ProcessResult(package, ct);
        var response = new ResponsePackage(package.Chat.Id, package.Chat.Messages[^1]);
        package.Source.EnqueueResponse(response);
    }

    private void ProcessResponse(ResponsePackage response)
        => OnResponseReceived?.Invoke(response.ChatId, response.Message);

    private Task<bool> ProcessResult(RequestPackage request, CancellationToken ct)
        => Task.FromResult(true);

    private async Task<HttpResult> Submit(RequestPackage package, CancellationToken ct = default) {
        var request = CreateRequest(package);
        var content = JsonContent.Create(request, options: IAgentOptions.SerializerOptions);
        var httpClient = _httpClientProvider.GetHttpClient();
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
}
