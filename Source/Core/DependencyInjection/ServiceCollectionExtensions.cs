namespace DotNetToolbox.DependencyInjection;

public static class ServiceCollectionExtensions {
    private static void SetEnvironmentFrom(
        this IServiceCollection services,
        IAssemblyDescriptor? assemblyDescriptor = null,
        IDateTimeProvider? dateTimeProvider = null,
        IGuidProvider? guidProvider = null,
        IFileSystem? fileSystem = null,
        IInput? input = null,
        IOutput? output = null) {
        services.AddAssemblyDescriptor(assemblyDescriptor);
        services.AddDateTimeProvider(dateTimeProvider);
        services.AddGuidProvider(guidProvider);
        services.AddFileSystem(fileSystem);
        services.AddInput(input);
        services.AddOutput(output);
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
        services.SetEnvironmentFrom(assemblyDescriptor,
                                    dateTimeProvider,
                                    guidProvider,
                                    fileSystem,
                                    input,
                                    output);
        services.TryAddSingleton<IEnvironment>(prv => new Environment(prv, name));
        return services;
    }

    public static IServiceCollection AddEnvironment(this IServiceCollection services, IEnvironment environment) {
        services.SetEnvironmentFrom(environment.Assembly,
                                    environment.DateTime,
                                    environment.Guid,
                                    environment.FileSystem,
                                    environment.Input,
                                    environment.Output);
        services.TryAddSingleton(IsNotNull(environment));
        return services;
    }

    public static IServiceCollection AddAssemblyDescriptor(this IServiceCollection services, IAssemblyDescriptor? descriptor = null) {
        services.TryAddSingleton(descriptor ?? AssemblyDescriptor.Default);
        return services;
    }

    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services, IDateTimeProvider? provider = null) {
        services.TryAddSingleton(provider ?? DateTimeProvider.Default);
        return services;
    }

    public static IServiceCollection AddGuidProvider(this IServiceCollection services, IGuidProvider? provider = null) {
        services.TryAddSingleton(provider ?? GuidProvider.Default);
        return services;
    }

    public static IServiceCollection AddFileSystem(this IServiceCollection services, IFileSystem? provider = null) {
        services.TryAddSingleton(provider ?? FileSystem.Default);
        return services;
    }

    public static IServiceCollection AddInput(this IServiceCollection services, IInput? provider = null) {
        services.TryAddSingleton(provider ?? Input.Default);
        return services;
    }

    public static IServiceCollection AddOutput(this IServiceCollection services, IOutput? provider = null) {
        services.TryAddSingleton(provider ?? Output.Default);
        return services;
    }
}
