namespace DotNetToolbox.AI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAIProvider<THttpProvider, TStandardAgent, TBackgroundAgent, TQueuedAgent, TMapper>(this IServiceCollection services, string key, IConfiguration configuration)
        where THttpProvider : class, IHttpClientProvider
        where TMapper : class, IMapper {
        services.AddKeyedHttpClientProvider<IHttpClientProvider, THttpProvider>(key, configuration);
        services.TryAddKeyedSingleton<IMapper, TMapper>(key);
        services.TryAddKeyedSingleton<IStandardAgentFactory, StandardAgentFactory<TStandardAgent>>(key);
        services.TryAddKeyedSingleton<IBackgroundAgentFactory, BackgroundAgentFactory<TBackgroundAgent>>(key);
        services.TryAddKeyedSingleton<IQueuedAgentFactory, QueuedAgentFactory<TQueuedAgent>>(key);
        return services;
    }
}
