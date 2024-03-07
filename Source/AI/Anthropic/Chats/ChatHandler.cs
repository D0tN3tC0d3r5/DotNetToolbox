using ChatCompletionResponse = DotNetToolbox.AI.Anthropic.DataModels.ChatCompletionResponse;

namespace DotNetToolbox.AI.Anthropic.Chats;

public class ChatHandler(IHttpClientProvider httpClientProvider, ILogger<ChatHandler> logger)
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
            var userMessage = new Message("user", message);
            chat.Messages.Add(userMessage);
            var request = CreateCompletionRequest(chat);
            var response = await SendMessage(chat, request, ct).ConfigureAwait(false);
            logger.LogDebug("Reply for chat '{id}' received.", chat.Id);
            return response;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a message to '{id}'.", chat.Id);
            throw;
        }
    }

    public async Task<Message> SendMessage<TRequest>(Chat chat, TRequest request, CancellationToken ct = default)
        where TRequest : class {
        var content = JsonContent.Create(request);
        var response = await _httpClient.PostAsync("v1/complete", content, ct).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var reply = JsonSerializer.Deserialize<ChatCompletionResponse>(json, _jsonSerializerOptions)!;
            chat.TotalNumberOfTokens += reply.Usage.InputTokens + reply.Usage.OutputTokens;
            var message = new Message("assistant", reply.Completion[0].Text!);
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
    private static DataModels.ChatCompletionRequest CreateCompletionRequest(Chat chat)
        => new() {
            Model = chat.Options.Model,
            Temperature = chat.Options.Temperature,
            MaximumTokensPerMessage = (int)chat.Options.MaximumTokensPerMessage,
            StopSequences = chat.Options.StopSequences.Count == 0 ? null : [.. chat.Options.StopSequences],
            MinimumTokenProbability = chat.Options.MinimumTokenProbability,
            UseStreaming = chat.Options.UseStreaming,
            Messages = [.. chat.Messages],
        };
}
