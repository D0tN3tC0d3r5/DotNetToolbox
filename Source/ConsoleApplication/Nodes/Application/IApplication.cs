using DotNetToolbox.ConsoleApplication.Nodes;

namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public interface IApplication : IExecutableNode {
    string AssemblyName { get; }
    string Title { get; }
    string Version { get; }
    string Environment { get; }
    string[] Arguments { get; }
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
    Task<int> RunAsync(CancellationToken ct = default);
}
