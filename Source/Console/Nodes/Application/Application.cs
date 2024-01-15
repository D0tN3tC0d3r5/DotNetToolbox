using static System.StringComparer;

namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class Application : IApplication {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;

    protected Application(IServiceProvider serviceProvider) {
        var assembly = Assembly.GetEntryAssembly()!;
        var assemblyName = assembly.GetName();
        AssemblyName = assemblyName.Name!;
        Version = assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version ?? assemblyName.Version!.ToString();
        Name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? AssemblyName;
        Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? string.Empty;

        ServiceProvider = serviceProvider;

        Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
        Output = ServiceProvider.GetRequiredService<IOutput>();
        Input = ServiceProvider.GetRequiredService<IInput>();
        DateTime = ServiceProvider.GetRequiredService<IDateTimeProvider>();
        Guid = ServiceProvider.GetRequiredService<IGuidProvider>();
        FileSystem = ServiceProvider.GetRequiredService<IFileSystem>();
    }

    public string AssemblyName { get; }
    public string Name { get; }
    public string[] Ids => [Name];

    public string Version { get; }
    public required string Description { get; init; }

    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }

    public IOutput Output { get; }
    public IInput Input { get; }
    public IDateTimeProvider DateTime { get; }
    public IGuidProvider Guid { get; }
    public IFileSystem FileSystem { get; }

    public ICollection<INode> Children { get; init; } = new HashSet<INode>();
    public IDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>();

    public abstract void AppendHelp(StringBuilder builder);
    public abstract void Exit(int exitCode = 0);

    public override string ToString() {
        var builder = new StringBuilder();
        builder.AppendJoin(null, GetType().Name, ": ", Name, " v", Version, " => ", Description);
        return builder.ToString();
    }
}

public abstract class Application<TApplication, TBuilder, TOptions>
    : Application, IApplication<TApplication, TBuilder, TOptions>
    where TApplication : Application<TApplication, TBuilder, TOptions>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>
                   , IHasDefault<TOptions>
                   , new() {
    protected bool IsRunning { get; private set; }
    private int _exitCode;

    protected Application(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(serviceProvider) {
        Arguments = args;
        var options = ServiceProvider.GetService<IOptions<TOptions>>();
        Options = options?.Value ?? TOptions.Default;
        Environment = environment ?? Options.Environment;

        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
        Logger = loggerFactory.CreateLogger<TApplication>();
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public void AppendVersion(StringBuilder builder)
        => builder.AppendLine($"{Name} v{Version}");

    public override void AppendHelp(StringBuilder builder) {
        AppendVersion(builder);
        builder.AppendLine(Description);
    }

    public string[] Arguments { get; }
    public string Environment { get; }
    public TOptions Options { get; }

    public ILogger Logger { get; init; }

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);

    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public TApplication AddCommand<TChildCommand>()
        where TChildCommand : CommandBase<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(ServiceProvider, this));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, Func<CancellationToken, Task<Result>> action) {
        Func<AsyncCommand, CancellationToken, Task<Result>> execute = (_, ct) => action(ct);
        Children.Add(CreateInstance.Of<AsyncCommand>(this, name, Array.Empty<string>(), execute));
        return (TApplication)this;
    }

    //public TApplication AddAction<TAction>()
    //    where TAction : Trigger<TAction> {
    //    Children.Add(CreateInstance.Of<TAction>(ServiceProvider, this));
    //    return (TApplication)this;
    //}

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

    public sealed override void Exit(int exitCode = 0) {
        _exitCode = exitCode;
        IsRunning = false;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        IsRunning = true;
        var taskRun = new CancellationTokenSource();
        var result = await ExecuteAsync(taskRun.Token);
        if (result.HasErrors) Output.WriteLine(result.Errors);
        return _exitCode;
    }

    protected virtual Task<Result> ExecuteAsync(CancellationToken ct) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        return ArgumentsReader.Read(Arguments, [.. Children], ct);
    }

    protected bool InputIsParsed(Result result) => IsSuccess(result, true);
    protected bool CanContinue(Result result) => IsSuccess(result, false);

    private bool IsSuccess(Result result, bool stopOnInvalidInput) {
        if (result.IsSuccess) return true;

        if (result.HasException) {
            var exitCode = result.InnerException is ConsoleException ce ? ce.ExitCode : DefaultErrorCode;
            Exit(exitCode);
            return false;
        }

        foreach (var error in result.Errors)
            Output.WriteLine("Exception: {0}", error);

        if (!stopOnInvalidInput) return true;
        Exit(DefaultErrorCode);
        return false;
    }

    protected async Task<Result> ProcessInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var executable = Children.OfType<IExecutable>().FirstOrDefault(FindChildById);
        if (executable is null) return Exception($"Command '{input[0]}' not found. For a list of available commands use 'help'.");
        var arguments = input.Length > 1 ? input.Skip(1).ToArray() : [];
        return await executable.ExecuteAsync(arguments, ct);

        bool FindChildById(IExecutable c) => c.Ids.Contains(input[0], InvariantCultureIgnoreCase);
    }

    public async ValueTask DisposeAsync() {
        await Dispose();
        GC.SuppressFinalize(this);
    }
}
