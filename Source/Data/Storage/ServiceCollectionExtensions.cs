namespace DotNetToolbox.Data;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddRepositoryStrategyProvider<TStrategy>(this IServiceCollection services, Action<IRepositoryStrategyContainer>? configure = null)
        where TStrategy : class, IRepositoryStrategy, new() {
        var provider = new RepositoryStrategyProvider();
        configure?.Invoke(provider);
        services.TryAddSingleton<IRepositoryStrategyProvider>(provider);
        return services;
    }
}
