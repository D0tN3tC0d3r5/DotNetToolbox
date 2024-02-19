using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace DotNetToolbox.OpenAI.Chats;

internal class ChatHandler(IChatRepository repository, IHttpClientProvider httpClientProvider, ILogger<ChatHandler> logger)
    : IChatHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower), },
    };

    public async Task<Chat> Create(Action<ChatOptionsBuilder>? configure = null) {
        try {
            logger.LogDebug("Creating new chat...");
            var builder = new ChatOptionsBuilder();
            configure?.Invoke(builder);
            var chat = new Chat(builder.Build());
            chat.Messages.Add(new() {
                Type = MessageType.System,
                Content = builder.SystemMessage,
            });
            await repository.Add(chat).ConfigureAwait(false);
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
        var chat = await repository.GetById(id).ConfigureAwait(false);
        if (chat is null) {
            logger.LogDebug("Chat '{id}' not found.", id);
            return string.Empty;
        }

        try {
            chat.Messages.Add(new() {
                Type = MessageType.User,
                Content = message,
            });
            var reply = await GetReplyAsync(chat, null).ConfigureAwait(false);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (reply.Length == 0) logger.LogDebug("Invalid reply received for chat '{id}'.", id);
            else logger.LogDebug("Reply for chat '{id}' received.", id);
            return reply;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a message to '{id}'.", id);
            throw;
        }
    }

    public async Task SendMessage(string id, string message, Action<string> processChunk) {
        logger.LogDebug("Sending message to chat '{id}'...", id);
        var chat = await repository.GetById(id);
        if (chat is null) {
            logger.LogDebug("Chat '{id}' not found.", id);
            return;
        }

        try {
            chat.Messages.Add(new() {
                Type = MessageType.User,
                Content = message,
            });
            var reply = await GetReplyAsync(chat, IsNotNull(processChunk)).ConfigureAwait(false);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (reply.Length == 0) logger.LogDebug("Invalid reply received for chat '{id}'.", id);
            else logger.LogDebug("Reply for chat '{id}' received.", id);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to send a message to '{id}'.", id);
            throw;
        }
    }

    private async Task<string> GetReplyAsync(Chat chat, Action<string>? processChunk) {
        var request = CreateCompletionRequest(chat);
        var content = JsonContent.Create(request, null, _jsonSerializerOptions);
        var response = await _httpClient.PostAsync("chat/completions", content).ConfigureAwait(false);
        try {
            response.EnsureSuccessStatusCode();
            return await ReadMessageAsStream(chat, response, processChunk).ConfigureAwait(false);
        }
        catch (Exception ex) {
            Console.WriteLine(await content.ReadAsStringAsync());
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.WriteLine(ex.ToString());
            return string.Empty;
        }
    }

    private static async Task<string> ReadMessageAsStream(Chat chat, HttpResponseMessage response, Action<string>? processChunk) {
        var contentBuilder = new StringBuilder();
        await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync().ConfigureAwait(false) is { } line) {
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ")) continue;
            if (line[6..] == "[DONE]") break;
            var reply = JsonSerializer.Deserialize<StreamResponse>(line[6..], _jsonSerializerOptions);
            var chunk = reply!.Choices[0].Delta;
            processChunk?.Invoke(chunk.Content);
            if (chunk.Type == MessageType.Assistant)
                chat.Messages.Add(chunk);
            else
                chat.Messages[^1] = chat.Messages[^1] with { Content = chat.Messages[^1].Content + chunk.Content };
            contentBuilder.Append(chunk.Content);
        }
        return contentBuilder.ToString();
    }

    private static CompletionRequest CreateCompletionRequest(Chat chat)
            => new(chat.Options.Model) {
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
