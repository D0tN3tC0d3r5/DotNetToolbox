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

    protected bool IsRunning { get; private set; }
    private int _exitCode;

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

    public TApplication AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(ServiceProvider, this));
        return (TApplication)this;
    }

    public TApplication AddAction<TAction>()
        where TAction : Arguments.Action<TAction> {
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
        return ReadArguments(args, ct);
    }

    private async Task<Result> ReadArguments(string[] input, CancellationToken ct) {
        for (var index = 0; index < input.Length; index++) {
            var argument = Children.FirstOrDefault(arg => arg.Ids.Contains(input[index]));
            switch (argument) {
                case IHasValue hasValue:
                    if (argument is IOption) index++;
                    if (index >= input.Length) return Error($"Missing value for option '{input[index]}'");
                    var argumentResult = await hasValue.SetValue(input[index], ct);
                    if (!argumentResult.IsSuccess) return argumentResult;
                    break;
                default:
                    return await ProcessParameters(input, ct);
            }
        }
        return Success();
    }

    private async Task<Result> ProcessParameters(string[] input, CancellationToken ct) {
        var parameters = Children.OfType<IParameter>().OrderBy(p => p.Order).ToArray();
        var index = 0;
        foreach (var parameter in parameters) {
            if (index >= input.Length && parameter.DefaultValue is null)
                return Error($"Missing value for parameter {index + 1}:'{parameter.Name}'");
            if (index >= input.Length) break;
            var result = await parameter.SetValue(input[index], ct);
            if (!result.IsSuccess) return result;
            index++;
        }

        return Success();
    }

    public async ValueTask DisposeAsync() {
        if (_isDisposed) return;
        await Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
    }
}
