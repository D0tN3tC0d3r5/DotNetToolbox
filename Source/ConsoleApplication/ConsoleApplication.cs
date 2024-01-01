namespace DotNetToolbox.ConsoleApplication;

public abstract record ConsoleApplication<TApplication, TBuilder, TOptions>
    : IApplication<TApplication, TBuilder, TOptions>
    where TApplication : ConsoleApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ConsoleApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, INamedOptions<TOptions>, new() {
    private bool _isDisposed;

    internal ConsoleApplication(string[] args, string environment, IServiceProvider serviceProvider) {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = serviceProvider.GetRequiredService<IOptions<TOptions>>();
        Options = options.Value;
        Environment = environment != string.Empty ? environment : Options.Environment ?? string.Empty;
        Name = Options.Name;
        Version = Options.Version;
        Description = Options.Description;
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        Arguments = args;
        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        Logger = loggerFactory.CreateLogger<TApplication>();
    }

    internal ConsoleApplication(IServiceProvider services)
        : this([], string.Empty, services) {
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public string Environment { get; }
    public string Name { get; }
    public string Version { get; }
    public string? Description { get; }
    public string[] Arguments { get; }
    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }
    public TOptions Options { get; }

    public IDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>();

    public ILogger<TApplication> Logger { get; init; }

    public static TBuilder CreateBuilder(Action<TBuilder>? configure = null)
        => CreateBuilder([], configure);
    public static TBuilder CreateBuilder(string[] args, Action<TBuilder>? configure = null) {
        var builder = Create.Instance<TBuilder>([]);
        configure?.Invoke(builder);
        return builder;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();
    public abstract Task<int> RunAsync();

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;
        await Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}

public record ConsoleApplication : ConsoleApplication<ConsoleApplication, ConsoleApplicationBuilder, ApplicationOptions> {
    internal ConsoleApplication(string[] args, string environment, IServiceProvider serviceProvider) : base(args, environment, serviceProvider) {
    }

    internal ConsoleApplication(IServiceProvider services) : base(services) {
    }

    public override Task<int> RunAsync() => throw new NotImplementedException();
}
