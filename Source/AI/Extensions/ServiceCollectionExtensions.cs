using DotNetToolbox.AI.Anthropic;

namespace DotNetToolbox.AI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, OpenAIHttpClientProvider>(configuration);
        services.TryAddSingleton<IChatRepository<OpenAIChat>, InMemoryChatRepository<OpenAIChat>>();
        services.TryAddSingleton<IModelsHandler, ModelsHandler>();
        services.TryAddSingleton<IChatHandler<OpenAIChat, OpenAIChatOptions>, OpenAIChatHandler>();
        return services;
    }

    public static IServiceCollection AddClaude(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, AnthropicHttpClientProvider>(configuration);
        services.TryAddSingleton<IChatRepository<ClaudeChat>, InMemoryChatRepository<ClaudeChat>>();
        services.TryAddSingleton<IClaudeChatHandler, ClaudeChatHandler>(); return services;
    }
}
