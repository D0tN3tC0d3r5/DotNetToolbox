namespace DotNetToolbox.AI.OpenAI.Agents;

internal class Agent(Chats.IChatHandler chatHandler)
    : IAgent {
    private readonly Queue<(IAgent Agent, Chats.Chat Chat, object Content, CancellationToken Token)> _requests = [];

    public async Task Start(CancellationToken ct) {
        while (!ct.IsCancellationRequested) {
            if (!_requests.TryDequeue(out var request)) {
                await Task.Delay(100, ct);
                continue;
            }

            (var agent, var chat, var content, var token) = request;
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(token, ct).Token;
            await ProcessRequest(agent, chat, content, linkedToken);
        }
    }

    public CancellationTokenSource EnqueueRequest(IAgent source, Chats.Chat chat, object content) {
        var tokenSource = new CancellationTokenSource();
        _requests.Enqueue((source, chat, content, tokenSource.Token));
        return tokenSource;
    }

    // Do something with the response from the processing agent.
    public Task ProcessResponse(string chatId, Chats.Message response, CancellationToken ct)
        => Task.CompletedTask;

    private async Task ProcessRequest(IAgent source, Chats.Chat chat, object content, CancellationToken ct) {
        if (ct.IsCancellationRequested) return;
        var response = await chatHandler.SendMessage(chat, (string)content, ct);
        await source.ProcessResponse(chat.Id, response, ct);
    }
}
