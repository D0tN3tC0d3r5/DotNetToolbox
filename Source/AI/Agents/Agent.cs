namespace DotNetToolbox.AI.Agents;

public class Agent<TOptions>(string name, Profile profile, IChatHandlerFactory chatHandlerFactory, TOptions options)
    : IAgent
    where TOptions : class, IChatOptions, new() {
    private readonly Queue<Package> _receivedRequests = [];
    private readonly Queue<Package> _receivedResponses = [];

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = name;
    public Profile Profile { get; set; } = profile;
    public List<Skill> Skills { get; set; } = [];

    public async Task Start(CancellationToken ct) {
        while (!ct.IsCancellationRequested) {
            if (_receivedRequests.TryDequeue(out var request))
                await ProcessReceivedRequest(request, ct);
            if (_receivedResponses.TryDequeue(out var response))
                await ProcessReceivedResponse(response, ct);
            await Task.Delay(100, ct);
        }
    }

    public CancellationTokenSource AddRequest(IAgent source, IChat chat) {
        var tokenSource = new CancellationTokenSource();
        var request = new Package(source, chat, tokenSource.Token);
        _receivedRequests.Enqueue(request);
        return tokenSource;
    }

    public void AddResponse(Package request) {
        if (request.Agent.Id != Id) return;
        _receivedResponses.Enqueue(request);
    }

    // Do something with the response from the processing agent.
    private async Task ProcessReceivedRequest(Package request, CancellationToken ct) {
        var ts = CancellationTokenSource.CreateLinkedTokenSource(request.Token, ct);
        if (ts.IsCancellationRequested) return;
        var chat = chatHandlerFactory.Create(options, request.Chat);
        var result = await chat.Submit(this, ts.Token);
        if (!result.IsOk) return;
        var isFinished = false;
        while (!isFinished)
            isFinished = await ProcessSubmissionResult(request, ct);
        request.Agent.AddResponse(request);
    }

    private Task ProcessReceivedResponse(Package response, CancellationToken ct)
        => Task.CompletedTask;

    private Task<bool> ProcessSubmissionResult(Package request, CancellationToken ct)
        => Task.FromResult(true);
}
