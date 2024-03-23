namespace DotNetToolbox.AI.Consumers;

public interface IAsyncResponseConsumer
    : IResponseAwaiter {
    Task ResponseApproved(string chatId, Message response, CancellationToken ct);
    Task<bool> VerifyResponse(string chatId, Message response, CancellationToken ct);
}
