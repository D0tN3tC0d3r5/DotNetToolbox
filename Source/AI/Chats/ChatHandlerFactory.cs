namespace DotNetToolbox.AI.Chats;

public abstract class ChatHandlerFactory<THandler, TChatHandler, TOptions, TRequest, TResponse>(IHttpClientProvider httpClientProvider, ILogger<THandler> logger)
    : IChatHandlerFactory
    where THandler : ChatHandlerFactory<THandler, TChatHandler, TOptions, TRequest, TResponse>
    where TChatHandler : class, IChatHandler
    where TOptions : class, IChatOptions, new()
    where TRequest : class
    where TResponse : class {

    IChatHandler IChatHandlerFactory.Create(IChatOptions options)
        => Create((TOptions)options);

    public TChatHandler Create(TOptions options) {
        try {
            logger.LogDebug("Creating new chat...");
            var chat = CreateInstance.Of<TChatHandler>(httpClientProvider, options);
            logger.LogDebug("Chat '{id}' started.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }
}
