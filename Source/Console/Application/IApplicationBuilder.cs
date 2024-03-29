﻿namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplicationBuilder<out TApplication, out TBuilder, TOptions>
    where TApplication : class, IApplication<TApplication, TBuilder, TOptions>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : class, IApplicationOptions, new() {
    TBuilder SetEnvironment(string environment);

    //TBuilder SetConfigurationSectionName(string sectionName);

    TBuilder AddEnvironmentVariables(string? prefix);
    TBuilder AddAppSettings(IFileProvider? fileProvider = null);
    TBuilder AddUserSecrets<TReference>() where TReference : class;

    TBuilder ConfigureLogging(Action<ILoggingBuilder> configure);

    TApplication Build();
}
