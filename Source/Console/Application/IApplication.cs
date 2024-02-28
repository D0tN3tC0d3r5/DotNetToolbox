namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplication : IHasChildren {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;
    int ExitCode { get; }

    string Version { get; }
    string AssemblyName { get; }
    string FullName { get; }
    IServiceProvider Services { get; }

    void Exit(int exitCode = DefaultExitCode);
}

public interface IApplication<out TApplication, out TBuilder>
    : IApplication, IAsyncDisposable
    where TApplication : class, IApplication<TApplication, TBuilder>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder> {

    int Run();
    Task<int> RunAsync();
}
