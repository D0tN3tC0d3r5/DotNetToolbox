namespace DotNetToolbox.Data;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddRepositoryStrategyProvider(this IServiceCollection services, Action<IRepositoryStrategyContainer>? configure = null) {
        var provider = new RepositoryStrategyProvider();
        configure?.Invoke(provider);
        services.TryAddSingleton<IRepositoryStrategyProvider>(provider);
        return services;
    }
}
