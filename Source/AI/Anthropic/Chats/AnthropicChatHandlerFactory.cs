namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChatHandlerFactory(World world, IHttpClientProvider httpClientProvider, ILogger<AnthropicChatHandlerFactory> logger)
    : ChatHandlerFactory<AnthropicChatHandlerFactory, AnthropicChatHandler, AnthropicChatOptions, AnthropicChatRequest, AnthropicChatResponse>(world, httpClientProvider, logger);
