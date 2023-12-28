namespace DotNetToolbox.OpenAI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IOpenAIHttpClientProvider, OpenAIHttpClientProvider, OpenAIHttpClientOptionsBuilder, OpenAIHttpClientOptions>(configuration);
        services.TryAddSingleton<IModelsHandler, ModelsHandler>();
        services.TryAddSingleton<IChatHandler, ChatHandler>();
        return services;
    }
}
