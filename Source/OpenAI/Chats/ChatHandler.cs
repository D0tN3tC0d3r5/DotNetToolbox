namespace DotNetToolbox.OpenAI.Chats;

internal class ChatHandler(IChatRepository repository, IOpenAIHttpClientProvider httpClientProvider, ILogger<ChatHandler>? logger = null)
    : IChatHandler {
    private readonly ILogger<ChatHandler> _logger = logger ?? NullLogger<ChatHandler>.Instance;
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    public async Task<string> Create(ChatOptions options) {
        try {
            _logger.LogDebug("Creating new chat...");
            var chat = new Chat {
                Options = options,
            };
            await repository.Add(chat);
            _logger.LogDebug("Chat '{id}' created.", chat.Id);
            return chat.Id;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to start a new chat.");
            throw;
        }
    }

    public async Task<string?> SendMessage(string id, string message) {
        _logger.LogDebug("Sending message to chat '{id}'...", id);
        var chat = await repository.GetById(id);
        if (chat is null) return null;

        try {
            chat.Messages.Add(new Message {
                Content = JsonSerializer.SerializeToElement(message),
                Type = MessageType.User,
            });
            var reply = await GetReplyAsync(chat).ConfigureAwait(false);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (reply == string.Empty) _logger.LogDebug("Empty reply received for chat '{id}'.", id);
            else _logger.LogDebug("Reply for chat '{id}' received.", id);
            return reply;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to send query.");
            throw;
        }
    }

    private async Task<string> GetReplyAsync(Chat chat) {
        var request = CreateCompletionRequest(chat);
        var content = JsonContent.Create(request);
        var response = await _httpClient.PostAsync("chat/completions", content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var reply = await response.Content.ReadFromJsonAsync<CompletionResponse>().ConfigureAwait(false);
        if (reply!.Choices.Length == 0) return string.Empty;
        var choice = reply!.Choices[0];
        var message = choice is MessageChoice messageChoice
                          ? messageChoice.Message
                          : ((DeltaChoice)choice).Delta;
        chat.Messages.Add(new Prompt {
            Type = message.Type,
            Content = message.Content!,
        });
        return message.Content.GetValueOrDefault().Deserialize<string>() ?? string.Empty;
    }

    private static CompletionRequest CreateCompletionRequest(Chat chat)
        => new(chat.Options.Model) {
            Temperature = 1.0m,
            MaximumNumberOfTokensPerMessage = 8000,
            Messages = chat.Messages.Select(i => new Prompt {
                Content = i.Content,
                Type = i.Type,
                Name = i.Name,
            }).ToArray(),
        };
}
