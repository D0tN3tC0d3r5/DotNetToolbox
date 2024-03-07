namespace DotNetToolbox.AI.OpenAI.Chats;

internal class ChatHandler(IHttpClientProvider httpClientProvider, ILogger<ChatHandler> logger)
    : IChatHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };

    public Task<Chat> Start(string userName, CancellationToken ct = default)
        => Start(userName, _ => { }, ct);

    public Task<Chat> Start(string userName, Action<ChatOptions> configure, CancellationToken ct = default) {
        try {
            logger.LogDebug("Creating new chat...");
            var options = new ChatOptions();
            IsNotNull(configure)(options);
            var chat = new Chat(userName, options);
            var message = new Message("system", options.SystemMessage);
            chat.Messages.Add(message);
            logger.LogDebug("Chat '{id}' created.", chat.Id);
            return Task.FromResult(chat);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    public async Task<Message> SendMessage(Chat chat, string message, CancellationToken ct = default) {
        try {
            var userMessage = new Message("user", message) { Name = chat.UserName };
            chat.Messages.Add(userMessage);
            var request = CreateCompletionRequest(chat);
            var response = await SendMessage(chat, request, ct).ConfigureAwait(false);
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a completion to '{id}'.", chat.Id);
            throw;
        }
    }

    public async Task<Message> SendToolResult(Chat chat, ToolResult[] results, CancellationToken ct = default) {
        try {
            foreach (var toolResult in results) {
                var message = new Message("tool", toolResult.Value) { ToolCallId = toolResult.Id };
                chat.Messages.Add(message);
            }
            var request = CreateCompletionRequest(chat);
            var response = await SendMessage(chat, request, ct).ConfigureAwait(false);
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a completion to '{id}'.", chat.Id);
            throw;
        }
    }

    public async Task<Message> SendMessage<TRequest>(Chat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class {
        var content = JsonContent.Create(request, null, _jsonSerializerOptions);
        var response = await _httpClient.PostAsync("v1/chat/completions", content, ct).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var reply = JsonSerializer.Deserialize<ChatCompletionResponse>(json, _jsonSerializerOptions)!;
            chat.TotalNumberOfTokens = reply.Usage!.TotalTokens;
            var message = MapMessage(reply.Choices[0].Message ?? reply.Choices[0].Delta);
            chat.Messages.Add(message);
            return message;
        }
        catch (Exception ex) {
            var error = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var message = $"""
                           StatusCode: {response.StatusCode};
                           Content: {error};
                           Error: {ex};
                           """;

            return new("error", message);
        }
    }

    private static ChatCompletionRequest CreateCompletionRequest(Chat chat)
            => new() {
                Model = chat.Options.Model,
                Temperature = chat.Options.Temperature,
                MaximumTokensPerMessage = (int?)chat.Options.MaximumTokensPerMessage,
                FrequencyPenalty = chat.Options.FrequencyPenalty,
                PresencePenalty = chat.Options.PresencePenalty,
                NumberOfChoices = chat.Options.NumberOfChoices,
                StopSequences = chat.Options.StopSequences.Count == 0 ? null : [.. chat.Options.StopSequences],
                MinimumTokenProbability = chat.Options.MinimumTokenProbability,
                UseStreaming = chat.Options.UseStreaming,
                Tools = chat.Options.Tools.Count == 0 ? null : [.. chat.Options.Tools],
                Messages = [.. chat.Messages],
            };

    private static Message MapMessage(Message? source) {
        if (source is null) throw new NotSupportedException("Invalid message type");
        var message = source.Content is string text
                          ? new("assistant", text)
                          : source.ToolCalls is not null
                              ? new Message("assistant", source.ToolCalls)
                              : throw new NotSupportedException("Invalid message format.");
        message.Name = source.Name;
        message.ToolCallId = source.ToolCallId;
        message.FinishReason = source.FinishReason;
        return message;
    }
}
