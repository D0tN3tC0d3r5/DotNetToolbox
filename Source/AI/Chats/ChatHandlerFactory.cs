namespace DotNetToolbox.AI.Chats;

public abstract class ChatHandlerFactory<THandler, TChatHandler, TOptions, TRequest, TResponse>
    : IChatHandlerFactory
    where THandler : ChatHandlerFactory<THandler, TChatHandler, TOptions, TRequest, TResponse>
    where TChatHandler : class, IChatHandler
    where TOptions : class, IChatOptions, new()
    where TRequest : class
    where TResponse : class {
    private readonly IHttpClientProvider _httpClientProvider;
    private readonly ILogger<THandler> _logger;

    protected ChatHandlerFactory(World world, IHttpClientProvider httpClientProvider, ILogger<THandler> logger) {
        World = world;
        _httpClientProvider = httpClientProvider;
        _logger = logger;
    }

    protected World World { get; }

    IChatHandler IChatHandlerFactory.Create(IChatOptions options, IChat chat)
        => Create((TOptions)options, chat);

    public TChatHandler Create(TOptions options, IChat chat) {
        try {
            _logger.LogDebug("Creating new chat handler...");
            var handler = CreateInstance.Of<TChatHandler>(World, _httpClientProvider, options, chat);
            _logger.LogDebug("Chat handler for '{id}' started.", chat.Id);
            return handler;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }
}
