namespace DotNetToolbox.AI.Consumers;

public interface IAsyncResponseConsumer
    : IResponseAwaiter {
    Task ResponseApproved(Guid chat, int? agent, Message response, CancellationToken ct);
    Task<bool> VerifyResponse(Guid chat, int? agent, Message response, CancellationToken ct);
}
