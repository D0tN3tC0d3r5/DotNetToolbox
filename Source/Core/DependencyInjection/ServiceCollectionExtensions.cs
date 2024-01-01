namespace DotNetToolbox.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSystemUtilities(this IServiceCollection services) {
        services.AddSingleton<GuidProvider>();
        services.AddSingleton<DateTimeProvider>();
        services.AddSingleton<FileSystemHandler>();
        services.AddSingleton<StandardOutput>();
        services.AddSingleton<StandardInput>();
        return services;
    }
}
