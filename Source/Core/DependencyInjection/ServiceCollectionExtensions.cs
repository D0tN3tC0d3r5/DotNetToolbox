namespace DotNetToolbox.DependencyInjection;

public static class ServiceCollectionExtensions {
    private static void SetEnvironmentFrom(
        this IServiceCollection services,
        string? name = null,
        IAssemblyDescriptor? assemblyDescriptor = null,
        IDateTimeProvider? dateTimeProvider = null,
        IGuidProvider? guidProvider = null,
        IFileSystem? fileSystem = null,
        IInput? input = null,
        IOutput? output = null) {
        services.AddAssemblyDescriptor(name, assemblyDescriptor);
        services.AddDateTimeProvider(name, dateTimeProvider);
        services.AddGuidProvider(name, guidProvider);
        services.AddFileSystem(name, fileSystem);
        services.AddInput(name, input);
        services.AddOutput(name, output);
    }

    public static IServiceCollection AddEnvironment(
        this IServiceCollection services,
        string? name = null,
        IAssemblyDescriptor? assemblyDescriptor = null,
        IDateTimeProvider? dateTimeProvider = null,
        IGuidProvider? guidProvider = null,
        IFileSystem? fileSystem = null,
        IInput? input = null,
        IOutput? output = null) {
        services.SetEnvironmentFrom(name,
                                    assemblyDescriptor,
                                    dateTimeProvider,
                                    guidProvider,
                                    fileSystem,
                                    input,
                                    output);
        services.TryAddSingleton<IEnvironment>(prv => new Environment(prv, name));
        return services;
    }

    public static IServiceCollection AddEnvironment(this IServiceCollection services, IEnvironment environment) {
        services.SetEnvironmentFrom(IsNotNull(environment.Name),
                                             environment.Assembly,
                                             environment.DateTime,
                                             environment.Guid,
                                             environment.FileSystem,
                                             environment.Input,
                                             environment.Output);
        services.TryAddSingleton(IsNotNull(environment));
        return services;
    }

    public static IServiceCollection AddAssemblyDescriptor(this IServiceCollection services, string? environment = null, IAssemblyDescriptor? descriptor = null) {
        services.TryAddKeyedSingleton(environment ?? string.Empty, descriptor ?? AssemblyDescriptor.Default);
        return services;
    }

    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services, string? environment = null, IDateTimeProvider? provider = null) {
        services.TryAddKeyedSingleton(environment ?? string.Empty, provider ?? DateTimeProvider.Default);
        return services;
    }

    public static IServiceCollection AddGuidProvider(this IServiceCollection services, string? environment = null, IGuidProvider? provider = null) {
        services.TryAddKeyedSingleton(environment ?? string.Empty, provider ?? GuidProvider.Default);
        return services;
    }

    public static IServiceCollection AddFileSystem(this IServiceCollection services, string? environment = null, IFileSystem? provider = null) {
        services.TryAddKeyedSingleton(environment ?? string.Empty, provider ?? FileSystem.Default);
        return services;
    }

    public static IServiceCollection AddInput(this IServiceCollection services, string? environment = null, IInput? provider = null) {
        services.TryAddKeyedSingleton(environment ?? string.Empty, provider ?? Input.Default);
        return services;
    }

    public static IServiceCollection AddOutput(this IServiceCollection services, string? environment = null, IOutput? provider = null) {
        services.TryAddKeyedSingleton(environment ?? string.Empty, provider ?? Output.Default);
        return services;
    }
}
