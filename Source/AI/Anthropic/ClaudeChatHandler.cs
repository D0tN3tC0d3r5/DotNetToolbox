using DotNetToolbox.AI.Anthropic.DataModels;

namespace DotNetToolbox.AI.Anthropic;

internal class ClaudeChatHandler(IChatRepository<ClaudeChat> repository, IHttpClientProvider httpClientProvider, ILogger<ClaudeChatHandler> logger)
    : ChatHandler<ClaudeChatHandler, ClaudeChat, ClaudeChatOptions>(repository, httpClientProvider, logger),
      IClaudeChatHandler {
    public override Task<ClaudeChat[]> List(CancellationToken ct = default) => throw new NotImplementedException();
    public override Task Terminate(ClaudeChat chat, CancellationToken ct = default) => throw new NotImplementedException();

    public override async Task<ClaudeChat> Start(string userName, Action<ClaudeChatOptions> configure, CancellationToken ct = default) {
        try {
            Logger.LogDebug("Creating new chat...");
            var options = new ClaudeChatOptions();
            IsNotNull(configure)(options);
            var chat = new ClaudeChat(userName, options);
            await Repository.Add(chat, ct).ConfigureAwait(false);
            Logger.LogDebug("Chat '{id}' created.", chat.Id);
            return chat;
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to create a new chat.");
            throw;
        }
    }

    public async Task<ClaudeMessage> SendMessage(ClaudeChat chat, string message, CancellationToken ct = default) {
        try {
            var userMessage = new ClaudeMessage(MessageType.User) { Content = message };
            chat.Messages.Add(userMessage);
            var request = new AnthropicCompletionRequest {
                Prompt = message,
                Model = chat.Options.Model,
                MaxTokensToSample = chat.Options.MaxTokensToSample,
                StopSequences = chat.Options.StopSequences
            };
            var response = await SendMessage<AnthropicCompletionRequest, ClaudeMessage>(chat, request, ct).ConfigureAwait(false);
            Logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            Logger.LogError(ex, "Failed to send a message to '{id}'.", chat.Id);
            throw;
        }
    }

    public override async Task<TResponse> SendMessage<TRequest, TResponse>(ClaudeChat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class
        where TResponse : class {
        var content = JsonContent.Create(request);
        var response = await HttpClient.PostAsync("v1/complete", content, ct).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AnthropicCompletionResponse>(cancellationToken: ct).ConfigureAwait(false);
            var message = new ClaudeMessage(MessageType.Assistant) {
                Content = result!.Completion.Trim(),
                FinishReason = result.StopReason
            };
            chat.Messages.Add(message);
            return (message as TResponse)!;
        }
        catch (Exception ex) {
            var error = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            return (new ClaudeMessage(MessageType.Error) {
                Content = $"""
                                                                        StatusCode: {response.StatusCode};
                                                                        Content: {error};
                                                                        Error: {ex};
                                                                        """,
            } as TResponse)!;
        }
    }
}