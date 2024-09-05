namespace DotNetToolbox.AI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAIProvider(this IServiceCollection services) {
        services.AddHttpClientProviderFactory();
        services.TryAddSingleton<IHttpConnectionAccessor, HttpConnectionAccessor>();
        return services;
    }

    public static IServiceCollection AddAIAgent<TAgent>(this IServiceCollection services, string? provider = null)
        where TAgent : class, IHttpConnection {
        if (provider is null)
            services.TryAddTransient<IHttpConnection, TAgent>();
        else
            services.AddKeyedTransient<IHttpConnection, TAgent>(provider);
        return services;
    }
}
