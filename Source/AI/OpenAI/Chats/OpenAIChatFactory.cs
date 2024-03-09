namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatFactory(IHttpClientProvider httpClientProvider, ILogger<OpenAIChatFactory> logger)
    : ChatFactory<OpenAIChatFactory, OpenAIChat, OpenAIChatOptions, OpenAIChatRequest, OpenAIChatResponse>(httpClientProvider, logger) {
}
