namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class Application<TApplication, TOptions>
    : Application<TApplication, ApplicationBuilder<TApplication, TOptions>, TOptions>
    where TApplication : Application<TApplication, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    protected Application(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider)
    {
    }
}

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
        Version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? assemblyName.Version!.ToString();
        Name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? AssemblyName;
        Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? string.Empty;
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
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public string AssemblyName { get; }
    public string Name { get; }
    public string[] Aliases { get; } = [];
    public string[] Ids => [ Name ];

    public string Version { get; }
    public required string Description { get; init; }

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, GetType().Name, ": ", Name, " v", Version, " => ", Description);
        return builder.ToString();
    }

    public void AppendHelp(StringBuilder builder) {
        builder.AppendLine($"{Name} v{Version}");
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

    public Output Output { get; init; }
    public Input Input { get; init; }
    public DateTimeProvider DateTime { get; init; }
    public GuidProvider Guid { get; init; }
    public FileSystem FileSystem { get; init; }

    public static TApplication Create(System.Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);
    public static TApplication Create(string[] args, System.Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public TApplication AddCommand<TCommand>()
        where TCommand : ICommand {
        Children.Add(CreateInstance.Of<TCommand>(this));
        return (TApplication)this;
    }

    public TApplication AddArgument<TArgument>()
        where TArgument : IArgument {
        Children.Add(CreateInstance.Of<TArgument>(this));
        return (TApplication)this;
    }

    public Task ExitAsync(int exitCode = 0) {
        Terminate = true;
        ExitCode = exitCode;
        return Task.CompletedTask;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        var taskRun = new CancellationTokenSource();
        var result = await ExecuteAsync(Arguments, taskRun.Token);
        if (result.IsSuccess) return ExitCode;
        if (result.HasErrors) Output.WriteLine(result.Errors);
        if (result.HasException) Output.WriteLine(result.Exception.ToString());
        return 1;
    }

    public virtual Task<Result> ExecuteAsync(string[] input, CancellationToken ct) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        return SuccessTask();
    }

    protected async Task<Result> ProcessInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = Children.OfType<IExecutable>().FirstOrDefault(c => c.Ids.Contains(input.First(), StringComparer.InvariantCultureIgnoreCase));
        if (command is null) return Error($"Command '{input}' not found. For a list of available commands use 'help'.");
        var arguments = input.Length > 1 ? input.Skip(1).ToArray() : [];
        return await command.ExecuteAsync(arguments, ct);
    }

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;
        await Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}
