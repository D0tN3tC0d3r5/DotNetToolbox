namespace DotNetToolbox.AI.Chats;

public abstract class ChatFactory<THandler, TChat, TOptions, TRequest, TResponse>(IHttpClientProvider httpClientProvider, ILogger<THandler> logger)
    : IChatFactory
    where THandler : ChatFactory<THandler, TChat, TOptions, TRequest, TResponse>
    where TChat : class, IChat
    where TOptions : class, IChatOptions, new()
    where TRequest : class
    where TResponse : class {

    IChat IChatFactory.Create(Action<IChatOptions>? configure)
        => Create(o => configure?.Invoke(o));

    public TChat Create(Action<TOptions>? configure = null) {
        try {
            logger.LogDebug("Creating new chat...");
            var options = new TOptions();
            configure?.Invoke(options);
            var chat = CreateInstance.Of<TChat>(httpClientProvider, options);
            logger.LogDebug("Chat '{id}' started.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }
}
