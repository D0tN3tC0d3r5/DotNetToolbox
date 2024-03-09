namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChatFactory(IHttpClientProvider httpClientProvider, ILogger<AnthropicChatFactory> logger)
    : ChatFactory<AnthropicChatFactory, AnthropicChat, AnthropicChatOptions, AnthropicChatRequest, AnthropicChatResponse>(httpClientProvider, logger);
