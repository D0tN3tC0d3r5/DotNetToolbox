namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplication : IHasChildren {
    string Version { get; }
    string AssemblyName { get; }
    string FullName { get; }
    IServiceProvider Services { get; }

    void ExitWith(int exitCode);
}

public interface IApplication<out TApplication, out TBuilder>
    : IApplication, IAsyncDisposable
    where TApplication : class, IApplication<TApplication, TBuilder>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder> {

    int Run();
    Task<int> RunAsync();
}
