namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplication
    : IHasChildren,
      IAsyncDisposable {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;
    int ExitCode { get; }

    string Version { get; }
    string DisplayVersion { get; }
    string AssemblyName { get; }
    string FullName { get; }
    IServiceProvider Services { get; }
    IConfigurationRoot Configuration { get; }

    void Exit(int code = DefaultExitCode);
}

public interface IApplication<TApplication, out TBuilder, out TSettings>
    : IApplication,
      IBuilderCreator<TApplication, TBuilder, TSettings>,
      IApplicationCreator<TApplication, TBuilder, TSettings>
    where TApplication : class, IApplication<TApplication, TBuilder, TSettings>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationSettings, new() {
    TSettings Settings { get; }
    int Run();
    Task<int> RunAsync();
}
