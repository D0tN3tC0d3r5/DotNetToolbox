namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChatHandlerFactory(IHttpClientProvider httpClientProvider, ILogger<AnthropicChatHandlerFactory> logger)
    : ChatHandlerFactory<AnthropicChatHandlerFactory, AnthropicChatHandler, AnthropicChatOptions, AnthropicChatRequest, AnthropicChatResponse>(httpClientProvider, logger);
