namespace DotNetToolbox.AI.OpenAI.Agents;

internal interface IAgent {
    Task Start(CancellationToken ct);
    CancellationTokenSource EnqueueRequest(IAgent source, Chats.Chat chat, object content);
    Task ProcessResponse(string chatId, Chats.Message response, CancellationToken ct);
}
