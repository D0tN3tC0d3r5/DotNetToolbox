namespace DotNetToolbox.OpenAI.Agents;

internal class AgentHandler(IAgentRepository repository, IHttpClientProvider httpClientProvider, ILogger<AgentHandler> logger)
    : IAgentHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower), },
    };

    public Task<Agent> Create(CancellationToken ct = default) => Create(_ => { }, ct);
    public async Task<Agent> Create(Action<AgentBuilder> configure, CancellationToken ct = default) {
        try {
            logger.LogDebug("Creating new chat...");
            var builder = new AgentBuilder();
            IsNotNull(configure)(builder);
            var chat = new Agent(builder.Build());
            chat.Messages.Add(new() {
                Type = MessageType.System,
                Content = builder.SystemMessage,
            });
            await repository.Add(chat, ct).ConfigureAwait(false);
            logger.LogDebug("Chat '{id}' created.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    public async Task GetResponse(Agent chat, string message, CancellationToken ct = default) {
        try {
            chat.Messages.Add(new() {
                Type = MessageType.User,
                Content = message,
            });
            await GetReplyAsync(chat, ct).ConfigureAwait(false);
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a message to '{id}'.", chat.Id);
            throw;
        }
    }

    private async Task GetReplyAsync(Agent chat, CancellationToken ct = default) {
        var request = CreateCompletionRequest(chat);
        var content = JsonContent.Create(request, null, _jsonSerializerOptions);
        var response = await _httpClient.PostAsync("chat/completions", content, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var reply = JsonSerializer.Deserialize<MessageResponse>(json, _jsonSerializerOptions)!;
        chat.TotalNumberOfTokens = reply.Usage!.TotalTokens;
        chat.Messages.Add(new() { Content = reply.Choices[0].Message.Content });
    }

    private static CompletionRequest CreateCompletionRequest(Agent chat)
            => new() {
                Model = chat.Options.Model,
                Temperature = chat.Options.Temperature,
                MaximumTokensPerMessage = (int?)chat.Options.MaximumTokensPerMessage,
                FrequencyPenalty = chat.Options.FrequencyPenalty,
                PresencePenalty = chat.Options.PresencePenalty,
                NumberOfChoices = chat.Options.NumberOfChoices,
                StopSignals = chat.Options.StopSignals?.ToArray(),
                TopProbability = chat.Options.TopProbability,
                UseStreaming = chat.Options.UseStreaming,
                Tools = chat.Options.Tools?.ToArray(),
                Messages = chat.Messages.Select(i => new Message {
                    Type = i.Type,
                    Name = i.Name,
                    Content = i.Content,
                }).ToArray(),
            };
}
