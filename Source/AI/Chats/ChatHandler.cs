namespace DotNetToolbox.AI.Chats;

public abstract class ChatHandler<THandler, TChat, TOptions, TRequest, TResponse>(IHttpClientProvider httpClientProvider, ILogger<THandler> logger)
    : IChatHandler
    where THandler : ChatHandler<THandler, TChat, TOptions, TRequest, TResponse>
    where TChat : class, IChat
    where TOptions : class, IChatOptions, new()
    where TRequest : class
    where TResponse : class {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    IChat IChatHandler.Start(Action<IChatOptions>? configure)
        => (TChat)Start(o => configure?.Invoke(o));

    public IChat Start(Action<TOptions>? configure = null) {
        try {
            logger.LogDebug("Creating new chat...");
            var options = new TOptions();
            configure?.Invoke(options);
            var chat = CreateInstance.Of<TChat>(options);
            logger.LogDebug("AnthropicChat '{id}' created.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    Task<Message> IChatHandler.SendMessage(IChat chat, Message input, CancellationToken ct)
        => SendMessage((TChat)chat, input, ct);

    public async Task<Message> SendMessage(TChat chat, Message input, CancellationToken ct = default) {
        chat.Messages.Add(input);
        var request = CreateRequest(chat);
        var content = JsonContent.Create(request);
        var httpResult = await _httpClient.PostAsync("v1/complete", content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var response = JsonSerializer.Deserialize<TResponse>(json, IChatOptions.SerializerOptions)!;
            UpdateUsage(chat, response);
            chat.Messages.Add(CreateOutput(response));
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return chat.Messages[^1];
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a inputMessage to '{id}'.", chat.Id);
            var error = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var errorMessage = $"""
                           StatusCode: {httpResult.StatusCode};
                           AnthropicMessageContent: {error};
                           Error: {ex};
                           """;
            return new("error", [new("text", errorMessage)]);
        }
    }

    protected abstract TRequest CreateRequest(TChat chat);
    protected abstract void UpdateUsage(TChat chat, TResponse response);
    protected abstract Message CreateOutput(TResponse response);
}
