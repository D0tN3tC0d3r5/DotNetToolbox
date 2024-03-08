namespace DotNetToolbox.AI.OpenAI.Agents;

internal class Agent(IChatHandler chatHandler)
    : IAgent {
    private readonly Queue<(IAgent Agent, IChat Chat, Message Message, CancellationToken Token)> _requests = [];

    public async Task Start(CancellationToken ct) {
        while (!ct.IsCancellationRequested) {
            if (!_requests.TryDequeue(out var request)) {
                await Task.Delay(100, ct);
                continue;
            }

            (var agent, var chat, var message, var token) = request;
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(token, ct).Token;
            await ProcessRequest(agent, chat, message, linkedToken);
        }
    }

    public CancellationTokenSource EnqueueRequest(IAgent source, IChat chat, Message message) {
        var tokenSource = new CancellationTokenSource();
        _requests.Enqueue((source, chat, message, tokenSource.Token));
        return tokenSource;
    }

    // Do something with the response from the processing agent.
    public Task ProcessResponse(string chatId, Message response, CancellationToken ct)
        => Task.CompletedTask;

    private async Task ProcessRequest(IAgent source, IChat chat, Message content, CancellationToken ct) {
        if (ct.IsCancellationRequested) return;
        var response = await chatHandler.SendMessage(chat, content, ct);
        await source.ProcessResponse(chat.Id, response, ct);
    }
}
