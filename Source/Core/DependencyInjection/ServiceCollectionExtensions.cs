namespace DotNetToolbox.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSystemUtilities(this IServiceCollection services)
        => services
        .AddSingleton<DateTimeProvider>()
        .AddSingleton<FileSystemHandler>();
}
