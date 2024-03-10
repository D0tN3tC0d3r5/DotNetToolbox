namespace DotNetToolbox.AI.Chats;

public abstract class ChatHandlerFactory<THandler, TChatHandler, TOptions, TRequest, TResponse>(
        World world,
        IHttpClientProvider httpClientProvider,
        ILogger<THandler> logger)
    : IChatHandlerFactory
    where THandler : ChatHandlerFactory<THandler, TChatHandler, TOptions, TRequest, TResponse>
    where TChatHandler : class, IChatHandler
    where TOptions : class, IChatOptions, new()
    where TRequest : class
    where TResponse : class {
    protected World World { get; } = world;

    IChatHandler IChatHandlerFactory.Create(IChatOptions options, IChat? previousContext)
        => Create((TOptions)options, previousContext);

    public TChatHandler Create(TOptions options, IChat? previousContext = null) {
        try {
            logger.LogDebug("Creating new previousContext handler...");
            var handler = CreateInstance.Of<TChatHandler>(World, httpClientProvider, options, previousContext);
            logger.LogDebug("Chat handler for '{id}' started.", handler.Chat.Id);
            return handler;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new previousContext.");
            throw;
        }
    }
}
