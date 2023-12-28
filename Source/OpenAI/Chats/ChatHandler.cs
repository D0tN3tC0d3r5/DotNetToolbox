namespace DotNetToolbox.OpenAI.Chats;

internal class ChatHandler(IChatRepository repository, IConfiguration configuration, IHttpClientProvider httpClientProvider, ILogger<ChatHandler>? logger = null)
    : IChatHandler {
    private readonly ILogger<ChatHandler> _logger = logger ?? NullLogger<ChatHandler>.Instance;
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient(opt => SetOptions(opt, configuration));

    internal static void SetOptions(HttpClientOptionsBuilder opt, IConfiguration configuration) {
        opt.SetBaseAddress(new("https://api.openai.com/v1/"));
        opt.UseSimpleTokenAuthentication(auth => SetAuthentication(auth: auth, configuration: configuration));
    }

    internal static void SetAuthentication(StaticTokenAuthenticationOptions auth, IConfiguration configuration) {
        auth.Scheme = AuthenticationScheme.Bearer;
        auth.Token = IsNotNullOrWhiteSpace(configuration.GetValue<string>("OpenAI:ApiKey"));
    }

    public async Task<string> Create(ChatOptions options) {
        try {
            var chat = new Chat {
                Options = options,
            };
            await repository.Add(chat);
            return chat.Id;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to start a new chat.");
            throw;
        }
    }

    public async Task<string?> SendMessage(string id, string message) {
        var chat = await repository.GetById(id);
        if (chat is null) return null;

        try {
            chat.Messages.Add(new Message {
                Content = JsonSerializer.Deserialize<JsonElement>(message),
                Type = MessageType.User,
            });
            return await GetReplyAsync(chat).ConfigureAwait(false);
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
        var choice = reply!.Choices[0].Message!;
        chat.Messages.Add(new Prompt {
            Type = choice.Type,
            Content = choice.Content!,
        });
        return choice.Content.GetValueOrDefault().Deserialize<string>() ?? string.Empty;
    }

    private static CompletionRequest CreateCompletionRequest(Chat chat)
        => new() {
            Model = chat.Options.Model,
            Temperature = 1.0m,
            MaximumNumberOfTokensPerMessage = 8000,
            Messages = chat.Messages.Select(i => new Prompt {
                Content = i.Content,
                Type = i.Type,
                Name = i.Name,
            }).ToArray(),
        };
}
