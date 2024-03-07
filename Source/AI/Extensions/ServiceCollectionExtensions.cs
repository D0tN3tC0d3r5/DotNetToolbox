namespace DotNetToolbox.AI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, OpenAIHttpClientProvider>(configuration);
        services.TryAddSingleton<OpenAI.Chats.IChatHandler, OpenAI.Chats.ChatHandler>();
        services.TryAddSingleton<IModelsHandler, ModelsHandler>();
        return services;
    }

    public static IServiceCollection AddAnthropic(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, AnthropicHttpClientProvider>(configuration);
        services.TryAddSingleton<Anthropic.Chats.IChatHandler, Anthropic.Chats.ChatHandler>(); return services;
    }
}
