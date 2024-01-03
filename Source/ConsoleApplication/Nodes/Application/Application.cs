namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class Application<TApplication, TBuilder, TOptions>
    : IApplication<TApplication, TBuilder, TOptions>
    where TApplication : Application<TApplication, TBuilder, TOptions>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    private bool _isDisposed;

    protected bool Terminate { get; private set; }
    protected int ExitCode { get; private set; }

    internal Application(string[] args, string? environment, IServiceProvider serviceProvider) {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = serviceProvider.GetService<IOptions<TOptions>>();
        Options = options?.Value ?? new TOptions();
        Environment = environment ?? Options.Environment;
        var assembly = Assembly.GetEntryAssembly()!;
        var assemblyName = assembly.GetName();
        AssemblyName = assemblyName.Name!;
        Version = assemblyName.Version!.ToString();
        Title = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? AssemblyName;
        Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        Arguments = args;

        Output = serviceProvider.GetRequiredService<Output>();
        Input = serviceProvider.GetRequiredService<Input>();
        DateTime = serviceProvider.GetRequiredService<DateTimeProvider>();
        Guid = serviceProvider.GetRequiredService<GuidProvider>();
        FileSystem = serviceProvider.GetRequiredService<FileSystem>();

        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        Logger = loggerFactory.CreateLogger<TApplication>();

        AddCommand<HelpCommand>();
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public string AssemblyName { get; }
    public string Title { get; }
    public string Version { get; }
    public string? Description { get; }
    public string[] Arguments { get; }
    public string Environment { get; }
    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }
    public TOptions Options { get; }

    public IDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>();

    public IExecutableNode? Parent => null;
    public ICollection<INamedNode> Children { get; init; } = new HashSet<INamedNode>();


    public ILogger Logger { get; init; }

    public Output Output { get; init; }
    public Input Input { get; init; }
    public DateTimeProvider DateTime { get; init; }
    public GuidProvider Guid { get; init; }
    public FileSystem FileSystem { get; init; }

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);
    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public TApplication AddCommand<TCommand>()
        where TCommand : ICommand {
        Children.Add(CreateInstance.Of<TCommand>(this));
        return (TApplication)this;
    }

    public Task ExitAsync(int exitCode = 0) {
        Terminate = true;
        ExitCode = exitCode;
        return Task.CompletedTask;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public virtual Task<int> RunAsync(CancellationToken ct = default) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        return Task.FromResult(0);
    }

    Task<Result> IExecutableNode.ExecuteAsync(string[] input, CancellationToken ct)
        => Result.SuccessTask();

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;
        await Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}
