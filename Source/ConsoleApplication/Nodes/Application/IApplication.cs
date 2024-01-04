namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public interface IApplication : IRoot, IExecutable {
    string AssemblyName { get; }
    string Version { get; }
    string Environment { get; }
    IServiceProvider ServiceProvider { get; }
    IConfiguration Configuration { get; }
    IDictionary<string, object?> Data { get; }

    Output Output { get; }
    Input Input { get; }
    DateTimeProvider DateTime { get; }
    GuidProvider Guid { get; }
    FileSystem FileSystem { get; }

    ILogger Logger { get; }
    Task ExitAsync(int exitCode = 0);
}

public interface IApplication<out TApplication, out TBuilder, TOptions>
    : IApplication, IAsyncDisposable
    where TApplication : class, IApplication<TApplication, TBuilder, TOptions>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : class, IApplicationOptions, new() {

    int Run();
    Task<int> RunAsync();
}
