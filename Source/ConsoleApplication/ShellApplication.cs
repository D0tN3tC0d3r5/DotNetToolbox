namespace DotNetToolbox.ConsoleApplication;

public abstract record ShellApplication<TApplication, TBuilder, TOptions>
    : IApplication<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    private bool _isDisposed;

    internal ShellApplication(string[] args, IServiceProvider serviceProvider) {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = new ApplicationOptions();
        configuration.Bind(ApplicationOptions.SectionName, options);
        Environment = options.Environment;
        Name = options.Name;
        Version = options.Version;
        Description = options.Description;
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        Options = options;
        Arguments = args;
        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        Logger = loggerFactory.CreateLogger<TApplication>();
    }

    internal ShellApplication(IServiceProvider services)
        : this([], services) {
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
    public ApplicationOptions Options { get; }

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

    public async Task<int> RunAsync() {
        var taskRun = new CancellationTokenSource();
        while (!taskRun.IsCancellationRequested) {
            Console.Write("> ");
            var userInput = Console.ReadLine() ?? string.Empty;
            if (string.Equals(userInput, "exit", CurrentCultureIgnoreCase))
                break;

            var result = await ProcessInput(userInput);
            if (result.HasException) {
                Console.WriteLine(result.Exception.ToString());
                return 1;
            }
        }

        return 0;
    }

    public abstract Task<Result> ProcessInput(string input);

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;
        await Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}

public record ShellApplication
    : ShellApplication<ShellApplication, ShellApplicationBuilder, ApplicationOptions> {
    internal ShellApplication(string[] args, IServiceProvider serviceProvider) : base(args, serviceProvider) {
    }

    internal ShellApplication(IServiceProvider services) : base(services) {
    }

    public override Task<Result> ProcessInput(string input) => throw new NotImplementedException();
}
