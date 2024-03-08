namespace DotNetToolbox.AI.Agent;

internal interface IAgent {
    Task Start(CancellationToken ct);
    CancellationTokenSource EnqueueRequest(IAgent source, IChat chat, Message message);
    Task ProcessResponse(string chatId, Message response, CancellationToken ct);
}
