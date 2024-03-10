namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatHandlerFactory(IHttpClientProvider httpClientProvider, ILogger<OpenAIChatHandlerFactory> logger)
    : ChatHandlerFactory<OpenAIChatHandlerFactory, OpenAIChatHandler, OpenAIChatOptions, OpenAIChatRequest, OpenAIChatResponse>(httpClientProvider, logger) {
}
