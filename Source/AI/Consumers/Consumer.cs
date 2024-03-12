namespace DotNetToolbox.AI.Consumers;

public abstract class Consumer<TConsumer>(
        IChat chat,
        ILogger<TConsumer> logger)
    : IConsumer
    where TConsumer : Consumer<TConsumer> {

    public IChat Chat { get; } = chat;
    protected ILogger<TConsumer> Logger = logger;

    public virtual async Task ProcessResponse(string chatId, Message response, CancellationToken ct) {
        if (ct.IsCancellationRequested) return;
        if (Chat?.Id != chatId) return;
        Logger.LogDebug("Response received...");
        await ProcessResponse(response, ct);
        Logger.LogDebug("Request completed.");
    }

    protected abstract Task ProcessResponse(Message response, CancellationToken ct);
}
