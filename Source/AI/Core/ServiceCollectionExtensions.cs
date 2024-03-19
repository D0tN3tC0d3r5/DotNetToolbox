using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAI<THttpProvider, TAgentFactory>(this IServiceCollection services, IConfiguration configuration)
        where THttpProvider : class, IHttpClientProvider
        where TAgentFactory : class, IAgentFactory {
        services.TryAddSingleton<World>();
        services.AddHttpClientProvider<IHttpClientProvider, THttpProvider>(configuration);
        services.TryAddSingleton<IAgentFactory, TAgentFactory>();
        return services;
    }
}
