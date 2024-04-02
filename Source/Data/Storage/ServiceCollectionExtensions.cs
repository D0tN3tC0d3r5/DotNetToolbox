namespace DotNetToolbox.Data;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddDataContext<TDataContext>(this IServiceCollection services, string? key = null)
        where TDataContext : DataContext {
        if (key is null) services.TryAddScoped<DataContext, TDataContext>();
        services.AddKeyedScoped<DataContext, TDataContext>(IsNotNullOrWhiteSpace(key));
        return services;
    }
}
