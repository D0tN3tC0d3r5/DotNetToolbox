namespace DotNetToolbox.AI.Consumers;

public abstract class ResponseConsumer<TConsumer>(int timeoutInMilliseconds = 5000, int delayInMilliseconds = 100, ILogger<TConsumer>? logger = null)
    : Awaiter<TConsumer>(timeoutInMilliseconds, delayInMilliseconds, logger),
      IResponseConsumer
    where TConsumer : ResponseConsumer<TConsumer> {

    public void ResponseApproved(Guid chat, int? agent, Message message) {
        Logger.LogDebug("Approved response received...");
        StopWaiting();
        OnResponseReceived(chat, agent, message);
    }

    public virtual bool VerifyResponse(Guid chat, int? agent, Message message) {
        Logger.LogDebug("Verify received response...");
        return true;
    }

    protected abstract void OnResponseReceived(Guid chat, int? agent, Message message);
}
