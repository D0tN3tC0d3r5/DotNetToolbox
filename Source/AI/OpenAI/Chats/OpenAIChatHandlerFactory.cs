namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatHandlerFactory(World world, IHttpClientProvider httpClientProvider, ILogger<OpenAIChatHandlerFactory> logger)
    : ChatHandlerFactory<OpenAIChatHandlerFactory, OpenAIChatHandler, OpenAIChatOptions, OpenAIChatRequest, OpenAIChatResponse>(world, httpClientProvider, logger) {
}
