namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class Application<TApplication, TBuilder, TOptions>
    : IApplication<TApplication, TBuilder, TOptions>
    where TApplication : Application<TApplication, TBuilder, TOptions>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>
                   , IHasDefault<TOptions>
                   , new() {
    private bool _isDisposed;

    protected bool IsRunning { get; private set; }
    private int _exitCode;

    internal Application(string[] args, string? environment, IServiceProvider serviceProvider) {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var options = serviceProvider.GetService<IOptions<TOptions>>();
        Options = options?.Value ?? TOptions.Default;
        Environment = environment ?? Options.Environment;
        var assembly = Assembly.GetEntryAssembly()!;
        var assemblyName = assembly.GetName();
        AssemblyName = assemblyName.Name!;
        Version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? assemblyName.Version!.ToString();
        Name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? AssemblyName;
        Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? string.Empty;
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        Arguments = args;

        Output = serviceProvider.GetRequiredService<IOutput>();
        Input = serviceProvider.GetRequiredService<IInput>();
        DateTime = serviceProvider.GetRequiredService<IDateTimeProvider>();
        Guid = serviceProvider.GetRequiredService<IGuidProvider>();
        FileSystem = serviceProvider.GetRequiredService<IFileSystem>();

        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        Logger = loggerFactory.CreateLogger<TApplication>();
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public string AssemblyName { get; }
    public string Name { get; }
    public string[] Ids => [ Name ];

    public string Version { get; }
    public required string Description { get; init; }

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, GetType().Name, ": ", Name, " v", Version, " => ", Description);
        return builder.ToString();
    }

    public void AppendVersion(StringBuilder builder)
        => builder.AppendLine($"{Name} v{Version}");

    public void AppendHelp(StringBuilder builder) {
        AppendVersion(builder);
        builder.AppendLine(Description);
    }

    public string[] Arguments { get; }
    public string Environment { get; }
    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }
    public TOptions Options { get; }
    public ICollection<INode> Children { get; init; } = new HashSet<INode>();
    public IDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>();

    public ILogger Logger { get; init; }

    public IOutput Output { get; init; }
    public IInput Input { get; init; }
    public IDateTimeProvider DateTime { get; init; }
    public IGuidProvider Guid { get; init; }
    public IFileSystem FileSystem { get; init; }

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);

    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public TApplication AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(ServiceProvider, this));
        return (TApplication)this;
    }

    public TApplication AddAction<TAction>()
        where TAction : ExecutableAction<TAction> {
        Children.Add(CreateInstance.Of<TAction>(ServiceProvider, this));
        return (TApplication)this;
    }

    public TApplication AddOption(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Option>(this, name, aliases));
        return (TApplication)this;
    }

    public TApplication AddOption<TOption>()
        where TOption : Option<TOption> {
        Children.Add(CreateInstance.Of<TOption>(ServiceProvider, this));
        return (TApplication)this;
    }

    public TApplication AddParameter(string name, object? defaultValue = default) {
        Children.Add(CreateInstance.Of<Parameter>(this, name, defaultValue));
        return (TApplication)this;
    }

    public TApplication AddParameter<TParameter>()
        where TParameter : Parameter<TParameter> {
        Children.Add(CreateInstance.Of<TParameter>(ServiceProvider, this));
        return (TApplication)this;
    }

    public TApplication AddFlag(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Flag>(this, name, aliases));
        return (TApplication)this;
    }

    public TApplication AddFlag<TFlag>()
        where TFlag : Flag<TFlag> {
        Children.Add(CreateInstance.Of<TFlag>(ServiceProvider, this));
        return (TApplication)this;
    }

    public void Exit(int exitCode = 0) {
        _exitCode = exitCode;
        IsRunning = false;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        IsRunning = true;
        var taskRun = new CancellationTokenSource();
        var result = await ExecuteAsync(Arguments, taskRun.Token);
        if (result.HasErrors) Output.WriteLine(result.Errors);
        if (result.HasException) Output.WriteLine(result.Exception.ToString());
        return _exitCode;
    }

    public virtual Task<Result> ExecuteAsync(string[] args, CancellationToken ct) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        return InputReader.ParseTokens([.. Children], args, ct);
    }

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;
        await Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}
