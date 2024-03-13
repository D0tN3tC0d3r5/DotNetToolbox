namespace DotNetToolbox.AI.OpenAI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, OpenAIHttpClientProvider>(configuration);
        services.TryAddSingleton<IAgentFactory, OpenAIAgentFactory>();
        return services;
    }
}
