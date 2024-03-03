using DotNetToolbox.OpenAI.Chats;

namespace DotNetToolbox.OpenAI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, OpenAIHttpClientProvider>(configuration);
        services.TryAddSingleton<IChatRepository, InMemoryChatRepository>();
        services.TryAddSingleton<IModelsHandler, ModelsHandler>();
        services.TryAddSingleton<IChatHandler, ChatHandler>();
        return services;
    }
}
