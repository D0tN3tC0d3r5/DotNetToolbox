namespace DotNetToolbox.AI.Consumers;

public interface IAsyncResponseConsumer
    : IResponseAwaiter {
    Task ResponseApproved(Guid chatId, int? agentNumber, Message response, CancellationToken ct);
    Task<bool> VerifyResponse(Guid chatId, int? agentNumber, Message response, CancellationToken ct);
}
