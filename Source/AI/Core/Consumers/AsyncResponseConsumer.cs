namespace DotNetToolbox.AI.Consumers;

public abstract class AsyncResponseConsumer<TConsumer>(int timeoutInMilliseconds = 5000, int delayInMilliseconds = 100, ILogger<TConsumer>? logger = null)
    : Awaiter<TConsumer>(timeoutInMilliseconds, delayInMilliseconds, logger),
      IAsyncResponseConsumer
    where TConsumer : AsyncResponseConsumer<TConsumer> {

    public Task ResponseApproved(Guid chat, int? agent, Message message, CancellationToken ct) {
        if (ct.IsCancellationRequested) return Task.CompletedTask;
        Logger.LogDebug("Approved response received...");
        return OnResponseReceived(chat, agent, message, ct);
    }

    public virtual Task<bool> VerifyResponse(Guid chat, int? agent, Message message, CancellationToken ct) {
        Logger.LogDebug("Verifying received response...");
        return Task.FromResult(true);
    }

    protected abstract Task OnResponseReceived(Guid chat, int? agentNumber, Message message, CancellationToken ct);
}
