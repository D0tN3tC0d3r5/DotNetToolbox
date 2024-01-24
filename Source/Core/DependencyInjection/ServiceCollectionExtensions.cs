namespace DotNetToolbox.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddSystemUtilities(
        this IServiceCollection services
      , IAssemblyAccessor? assemblyAccessor = null
      , IDateTimeProvider? dateTimeProvider = null
      , IGuidProvider? guidProvider = null
      , IFileSystem? fileSystem = null
      , IInput? input = null
      , IOutput? output = null) {
        services.AddAssemblyDescriptor(assemblyAccessor);
        services.AddDateTimeProvider(dateTimeProvider);
        services.AddGuidProvider(guidProvider);
        services.AddFileSystem(fileSystem);
        services.AddInput(input);
        services.AddOutput(output);
        return services;
    }

    public static IServiceCollection AddAssemblyDescriptor(this IServiceCollection services, IAssemblyAccessor? accessor = null) {
        services.TryAddSingleton(accessor ?? new AssemblyAccessor());
        return services;
    }

    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services, IDateTimeProvider? provider = null) {
        services.TryAddSingleton(provider ?? new DateTimeProvider());
        return services;
    }

    public static IServiceCollection AddGuidProvider(this IServiceCollection services, IGuidProvider? provider = null) {
        services.TryAddSingleton(provider ?? new GuidProvider());
        return services;
    }

    public static IServiceCollection AddFileSystem(this IServiceCollection services, IFileSystem? provider = null) {
        services.TryAddSingleton(provider ?? new FileSystem());
        return services;
    }

    public static IServiceCollection AddInput(this IServiceCollection services, IInput? provider = null) {
        services.TryAddSingleton(provider ?? new Input());
        return services;
    }

    public static IServiceCollection AddOutput(this IServiceCollection services, IOutput? provider = null) {
        services.TryAddSingleton(provider ?? new Output());
        return services;
    }
}
