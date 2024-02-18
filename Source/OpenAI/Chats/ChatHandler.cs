namespace DotNetToolbox.OpenAI.Chats;

internal class ChatHandler(IChatRepository repository, IHttpClientProvider httpClientProvider, ILogger<ChatHandler> logger)
    : IChatHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    public async Task<Chat> Create(string? model = null, Action<ChatOptionsBuilder>? configure = null) {
        try {
            logger.LogDebug("Creating new chat...");
            var builder = new ChatOptionsBuilder(model);
            configure?.Invoke(builder);
            var chat = new Chat(builder.Build());
            chat.Messages.Add(new() {
                Type = MessageType.System,
                Content = JsonSerializer.SerializeToElement(builder.SystemMessage),
            });
            await repository.Add(chat);
            logger.LogDebug("Chat '{id}' created.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    public async Task<string?> SendMessage(string id, string message) {
        logger.LogDebug("Sending message to chat '{id}'...", id);
        var chat = await repository.GetById(id);
        if (chat is null) {
            logger.LogDebug("Chat '{id}' not found.", id);
            return string.Empty;
        }

        try {
            chat.Messages.Add(new() {
                Content = JsonSerializer.SerializeToElement(message),
                Type = MessageType.User,
            });
            var reply = await GetReplyAsync(chat).ConfigureAwait(false);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (reply?.Length == 0) logger.LogDebug("Invalid reply received for chat '{id}'.", id);
            else logger.LogDebug("Reply for chat '{id}' received.", id);
            return reply;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a message to '{id}'.", id);
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
        var choice = reply.Choices[0];
        var message = choice is MessageChoice messageChoice
                          ? messageChoice.Message
                          : ((DeltaChoice)choice).Delta;
        chat.Messages.Add(new Prompt {
            Type = message.Type,
            Content = message.Content,
        });
        return message.Content.Deserialize<string>() ?? string.Empty;
    }

    private static CompletionRequest CreateCompletionRequest(Chat chat)
        => new(chat.Options.Model) {
            Temperature = chat.Options.Temperature,
            MaximumTokensPerMessage = (int)chat.Options.MaximumTokensPerMessage,
            FrequencyPenalty = chat.Options.FrequencyPenalty,
            PresencePenalty = chat.Options.PresencePenalty,
            NumberOfChoices = chat.Options.NumberOfChoices,
            StopSignals = [.. chat.Options.StopSignals],
            TopProbability = chat.Options.TopProbability,
            UseStreaming = chat.Options.UseStreaming,
            Tools = [.. chat.Options.Tools],
            Messages = chat.Messages.Select(i => new Prompt {
                Content = i.Content,
                Type = i.Type,
                Name = i.Name,
            }).ToArray(),
        };
}
