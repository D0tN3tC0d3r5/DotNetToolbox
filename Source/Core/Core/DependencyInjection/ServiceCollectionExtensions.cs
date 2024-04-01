﻿namespace DotNetToolbox.DependencyInjection;

public static class ServiceCollectionExtensions {
    public static IServiceCollection SetEnvironment(this IServiceCollection services,
            string? name = null,
            IAssemblyDescriptor? assemblyDescriptor = null,
            IDateTimeProvider? dateTimeProvider = null,
            IGuidProvider? guidProvider = null,
            IFileSystemAccessor? fileSystem = null,
            IInput? input = null,
            IOutput? output = null) {
        services.SetEnvironmentFrom(assemblyDescriptor,
                                    dateTimeProvider,
                                    guidProvider,
                                    fileSystem,
                                    input,
                                    output);
        services.TryAddSingleton<ISystemEnvironment>(prv => new SystemEnvironment(prv, name));
        return services;
    }

    public static IServiceCollection SetEnvironment(this IServiceCollection services,
            ISystemEnvironment env) {
        services.SetEnvironmentFrom(env.Assembly,
                                    env.DateTime,
                                    env.Guid,
                                    env.FileSystemAccessor,
                                    env.ConsoleInput,
                                    env.ConsoleOutput);
        services.TryAddSingleton(IsNotNull(env));
        return services;
    }

    public static IServiceCollection AddAssemblyDescriptor(this IServiceCollection services, IAssemblyDescriptor? descriptor = null) {
        services.TryAddSingleton(descriptor ?? AssemblyDescriptor.Default);
        return services;
    }

    public static IServiceCollection SetDateTimeProvider(this IServiceCollection services, IDateTimeProvider? provider = null) {
        services.TryAddSingleton(provider ?? DateTimeProvider.Default);
        return services;
    }

    public static IServiceCollection SetGuidProvider(this IServiceCollection services, IGuidProvider? provider = null) {
        services.TryAddSingleton(provider ?? GuidProvider.Default);
        return services;
    }

    public static IServiceCollection SetFileSystemAccessor(this IServiceCollection services, IFileSystemAccessor? provider = null) {
        services.TryAddSingleton(provider ?? FileSystemAccessor.Default);
        return services;
    }

    public static IServiceCollection SetConsoleInput(this IServiceCollection services, IInput? provider = null) {
        services.TryAddSingleton(provider ?? ConsoleInput.Default);
        return services;
    }

    public static IServiceCollection SetConsoleOutput(this IServiceCollection services, IOutput? provider = null) {
        services.TryAddSingleton(provider ?? ConsoleOutput.Default);
        return services;
    }

    private static void SetEnvironmentFrom(this IServiceCollection services,
                                           IAssemblyDescriptor? assemblyDescriptor = null,
                                           IDateTimeProvider? dateTimeProvider = null,
                                           IGuidProvider? guidProvider = null,
                                           IFileSystemAccessor? fileSystem = null,
                                           IInput? input = null,
                                           IOutput? output = null) {
        services.AddAssemblyDescriptor(assemblyDescriptor);
        services.SetDateTimeProvider(dateTimeProvider);
        services.SetGuidProvider(guidProvider);
        services.SetFileSystemAccessor(fileSystem);
        services.SetConsoleInput(input);
        services.SetConsoleOutput(output);
    }
}
