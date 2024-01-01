namespace DotNetToolbox.ConsoleApplication;

public interface IApplication : IApplicationOptions {
    string[] Arguments { get; }
    IServiceProvider ServiceProvider { get; }
    IConfiguration Configuration { get; }
    IDictionary<string, object?> Data { get; }

    Output Output { get; }
    Input Input { get; }
    DateTimeProvider DateTime { get; }
    GuidProvider Guid { get; }
    FileSystemHandler FileSystem { get; }
}

public interface IApplication<out TApplication, out TBuilder, TOptions>
    : IApplication, IAsyncDisposable
    where TApplication : class, IApplication<TApplication, TBuilder, TOptions>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : class, IApplicationOptions, new() {
    public static abstract TBuilder CreateBuilder(Action<TBuilder>? configure = null);
    public static abstract TBuilder CreateBuilder(string[] args, Action<TBuilder>? configure = null);

    ILogger<TApplication> Logger { get; }
    int Run();
    Task<int> RunAsync();
}
