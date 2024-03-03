namespace DotNetToolbox.OpenAI.Agents;

internal class AgentHandler(IChatRepository repository, IHttpClientProvider httpClientProvider, ILogger<AgentHandler> logger)
    : IChatHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower), },
    };

    public Task<Chat> Create(CancellationToken ct = default) => Create(_ => { }, ct);
    public async Task<Chat> Create(Action<AgentBuilder> configure, CancellationToken ct = default) {
        try {
            logger.LogDebug("Creating new chat...");
            var builder = new AgentBuilder();
            IsNotNull(configure)(builder);
            var chat = new Chat(builder.Build());
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

    public async Task GetResponse(Chat chat, string message, CancellationToken ct = default) {
        try {
            chat.Messages.Add(new(MessageType.User) { Content = message });
            await GetReplyAsync(chat, ct).ConfigureAwait(false);
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a message to '{id}'.", chat.Id);
            throw;
        }
    }

    private async Task GetReplyAsync(Chat chat, CancellationToken ct = default) {
        var request = CreateCompletionRequest(chat);
        var content = JsonContent.Create(request, null, _jsonSerializerOptions);
        var response = await _httpClient.PostAsync("chat/completions", content, ct).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var reply = JsonSerializer.Deserialize<MessageResponse>(json, _jsonSerializerOptions)!;
            chat.TotalNumberOfTokens = reply.Usage!.TotalTokens;
            chat.Messages.Add(new(MessageType.Assistant) {
                Content = reply.Choices[0].Message.Content,
                ToolCallId = reply.Choices[0].Message.ToolCallId,
                ToolCalls = reply.Choices[0].Message.ToolCalls,
                Name = reply.Choices[0].Message.Name,
            });
        }
        catch (Exception ex) {
            var error = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            throw new HttpRequestException(HttpRequestError.InvalidResponse, error, ex, response.StatusCode);
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
                Messages = chat.Messages.Select(i => new Message(i.Type) {
                    Name = i.Name,
                    Content = i.Content,
                }).ToArray(),
            };
}
