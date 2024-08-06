namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplication : IHasChildren {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;
    int ExitCode { get; }

    string Version { get; }
    string AssemblyName { get; }
    string FullName { get; }
    IServiceProvider Services { get; }
    IConfigurationRoot Configuration { get; }

    void Exit(int code = DefaultExitCode);
}

public interface IApplication<TApplication, out TBuilder>
    : IApplication,
      IBuilderCreator<TApplication, TBuilder>,
      IApplicationCreator<TApplication, TBuilder>,
      IAsyncDisposable
    where TApplication : class, IApplication<TApplication, TBuilder>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder> {
    int Run();
    Task<int> RunAsync();
}
