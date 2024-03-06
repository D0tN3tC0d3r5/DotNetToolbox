namespace DotNetToolbox.AI.OpenAI;

internal class OpenAIChatHandler(IChatRepository<OpenAIChat> repository, IHttpClientProvider httpClientProvider, ILogger<OpenAIChatHandler> logger)
    : ChatHandler<OpenAIChatHandler, OpenAIChat, OpenAIChatOptions>(repository, httpClientProvider, logger),
      IOpenAIChatHandler {
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };

    public override Task<OpenAIChat[]> List(CancellationToken ct = default) => throw new NotImplementedException();
    public override Task Terminate(OpenAIChat chat, CancellationToken ct = default) => throw new NotImplementedException();

    public override async Task<OpenAIChat> Start(string userName, Action<OpenAIChatOptions> configure, CancellationToken ct = default) {
        try {
            Logger.LogDebug("Creating new chat...");
            var options = new OpenAIChatOptions();
            IsNotNull(configure)(options);
            var chat = new OpenAIChat(userName, options);
            chat.Messages.Add(new(MessageType.System) { Content = OpenAIChatOptions.DefaultSystemMessage });
            await Repository.Add(chat, ct).ConfigureAwait(false);
            Logger.LogDebug("Chat '{id}' created.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    public async Task<OpenAIMessage> SendMessage(OpenAIChat chat, string message, CancellationToken ct = default) {
        try {
            var userMessage = new OpenAIMessage(MessageType.User) {
                Name = chat.UserName,
                Content = message,
            };
            chat.Messages.Add(userMessage);
            var request = CreateCompletionRequest(chat);
            var response = await SendMessage<CompletionRequest, OpenAIMessage>(chat, request, ct).ConfigureAwait(false);
            Logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to send a completion to '{id}'.", chat.Id);
            throw;
        }
    }

    public async Task<OpenAIMessage> SendToolResult(OpenAIChat chat, ToolResult[] results, CancellationToken ct = default) {
        try {
            foreach (var toolResult in results) {
                var message = new OpenAIMessage(MessageType.Tool) {
                    ToolCallId = toolResult.Id,
                    Content = toolResult.Value,
                };
                chat.Messages.Add(message);
            }
            var request = CreateCompletionRequest(chat);
            var response = await SendMessage<CompletionRequest, OpenAIMessage>(chat, request, ct).ConfigureAwait(false);
            Logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to send a completion to '{id}'.", chat.Id);
            throw;
        }
    }

    public override async Task<TResponse> SendMessage<TRequest, TResponse>(OpenAIChat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class
        where TResponse : class {
        var content = JsonContent.Create(request, null, _jsonSerializerOptions);
        var response = await HttpClient.PostAsync("chat/completions", content, ct).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var reply = JsonSerializer.Deserialize<MessageResponse>(json, _jsonSerializerOptions)!;
            chat.TotalNumberOfTokens = reply.Usage!.TotalTokens;
            var completion = reply.Choices[0].Message;
            var message = new OpenAIMessage(MessageType.Assistant) {
                Content = completion.Content,
                ToolCallId = completion.ToolCallId,
                ToolCalls = completion.ToolCalls,
                Name = completion.Name,
                FinishReason = completion.FinishReason,
            };
            chat.Messages.Add(message);
            return (message as TResponse)!;
        }
        catch (Exception ex) {
            var error = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            return (new OpenAIMessage(MessageType.Error) {
                Content = $"""
                           StatusCode: {response.StatusCode};
                           Content: {error};
                           Error: {ex};
                           """,
            } as TResponse)!;
        }
    }

    private static CompletionRequest CreateCompletionRequest(OpenAIChat chat)
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
