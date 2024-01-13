namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public interface IApplication : IHasChildren {
    string Version { get; }
    public string FullName => string.Join(" v", Name, Version);
    IServiceProvider ServiceProvider { get; }
    IConfiguration Configuration { get; }
    IDictionary<string, object?> Data { get; }

    IOutput Output { get; }
    IInput Input { get; }
    IDateTimeProvider DateTime { get; }
    IGuidProvider Guid { get; }
    IFileSystem FileSystem { get; }

    void Exit(int exitCode = 0);
}

public interface IApplication<out TApplication, out TBuilder, TOptions>
    : IApplication, IAsyncDisposable
    where TApplication : class, IApplication<TApplication, TBuilder, TOptions>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : class, IApplicationOptions, new() {

    int Run();
    Task<int> RunAsync();
}
