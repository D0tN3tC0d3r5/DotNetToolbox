namespace DotNetToolbox.AI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAIProvider(this IServiceCollection services) {
        services.AddHttpClientProviderFactory();
        services.TryAddSingleton<IAgentFactory, AgentFactory>();
        return services;
    }

    public static IServiceCollection AddAIAgent<TAgent>(this IServiceCollection services, string? provider = null)
        where TAgent : class, IAgent {
        if (provider is null)
            services.TryAddTransient<IAgent, TAgent>();
        else
            services.AddKeyedTransient<IAgent, TAgent>(provider);
        return services;
    }
}
