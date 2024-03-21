namespace DotNetToolbox.AI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAIProvider<THttpProvider, TAgentFactory>(this IServiceCollection services, string key, IConfiguration configuration)
        where THttpProvider : class, IHttpClientProvider
        where TAgentFactory : class, IAgentFactory {
        services.AddKeyedHttpClientProvider<IHttpClientProvider, THttpProvider>(key, configuration);
        services.TryAddKeyedSingleton<IAgentFactory, TAgentFactory>(key);
        return services;
    }
}
