﻿
namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplicationBuilder {
    IServiceCollection Services { get; }
    IConfigurationRoot Configuration { get; }

    void SetDateTimeProvider(IDateTimeProvider dateTimeProvider);
    void SetAssemblyInformation(IAssemblyDescriptor assemblyDescriptor);
    void SetInputHandler(IInput input);
    void SetOutputHandler(IOutput output);
    void SetFileSystem(IFileSystemAccessor fileSystem);
    void SetGuidProvider(IGuidProvider guidProvider);
    void ConfigureLogging(Action<ILoggingBuilder> configure);
}

public interface IApplicationBuilder<out TApplication, out TBuilder>
    : IApplicationBuilder
    where TApplication : class, IApplication<TApplication, TBuilder>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder> {
    TApplication Build();
}
