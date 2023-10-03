namespace System.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddCoreUtilities(this IServiceCollection services)
        => services
           .AddSingleton<DateTimeProvider>()
           .AddSingleton<FileSystemHandler>();
}
