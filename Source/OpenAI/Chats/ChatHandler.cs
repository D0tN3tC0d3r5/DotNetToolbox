namespace DotNetToolbox.OpenAI.Chats;

internal class ChatHandler(IChatRepository repository, IHttpClientProvider httpClientProvider, ILogger<ChatHandler> logger)
    : IChatHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };

    public Task<Chat> Create(string userName, CancellationToken ct = default) => Create(userName, _ => { }, ct);
    public async Task<Chat> Create(string userName, Action<ChatBuilder> configure, CancellationToken ct = default) {
        try {
            logger.LogDebug("Creating new chat...");
            var builder = new ChatBuilder();
            IsNotNull(configure)(builder);
            var chat = new Chat(userName, builder.Build());
            chat.Messages.Add(new(MessageType.System) { Content = builder.SystemMessage });
            await repository.Add(chat, ct).ConfigureAwait(false);
            logger.LogDebug("Chat '{id}' created.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    public async Task<Message> SendUserMessage(Chat chat, string message, CancellationToken ct = default) {
        try {
            var userMessage = new Message(MessageType.User) {
                Name = chat.UserName,
                Content = message,
            };
            chat.Messages.Add(userMessage);
            var response = await GetReplyAsync(chat, ct).ConfigureAwait(false);
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
                var message = new Message(MessageType.Tool) {
                    ToolCallId = toolResult.Id,
                    Content = toolResult.Value,
                };
                chat.Messages.Add(message);
            }
            var response = await GetReplyAsync(chat, ct).ConfigureAwait(false);
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a completion to '{id}'.", chat.Id);
            throw;
        }
    }

    private async Task<Message> GetReplyAsync(Chat chat, CancellationToken ct = default) {
        var request = CreateCompletionRequest(chat);
        var content = JsonContent.Create(request, null, _jsonSerializerOptions);
        var response = await _httpClient.PostAsync("chat/completions", content, ct).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var reply = JsonSerializer.Deserialize<MessageResponse>(json, _jsonSerializerOptions)!;
            chat.TotalNumberOfTokens = reply.Usage!.TotalTokens;
            var completion = reply.Choices[0].Message;
            var message = new Message(MessageType.Assistant) {
                Content = completion.Content,
                ToolCallId = completion.ToolCallId,
                ToolCalls = completion.ToolCalls,
                Name = completion.Name,
                FinishReason = completion.FinishReason,
            };
            chat.Messages.Add(message);
            return message;
        }
        catch (Exception ex) {
            var error = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            return new(MessageType.Error) {
                Content = $"""
                           StatusCode: {response.StatusCode};
                           Content: {error};
                           Error: {ex};
                           """,
            };
        }
    }

    private static CompletionRequest CreateCompletionRequest(Chat chat)
            => new() {
                Model = chat.Options.Model,
                Temperature = chat.Options.Temperature,
                MaximumTokensPerMessage = (int?)chat.Options.MaximumTokensPerMessage,
                FrequencyPenalty = chat.Options.FrequencyPenalty,
                PresencePenalty = chat.Options.PresencePenalty,
                NumberOfChoices = chat.Options.NumberOfChoices,
                StopSignals = chat.Options.StopSignals.Count == 0 ? null : [.. chat.Options.StopSignals],
                TopProbability = chat.Options.TopProbability,
                UseStreaming = chat.Options.UseStreaming,
                Tools = chat.Options.Tools.Count == 0 ? null : [.. chat.Options.Tools],
                Messages = chat.Messages.Select(i => new Completion {
                    Role = (Role)i.Type,
                    Name = i.Name,
                    Content = i.Content,
                    FinishReason = i.FinishReason,
                    ToolCallId = i.ToolCallId,
                    ToolCalls = i.ToolCalls,
                }).ToArray(),
            };
}
