namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class Application : IApplication {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;

    protected Application(IServiceProvider serviceProvider) {
        Services = serviceProvider;

        var accessor = Services.GetRequiredService<IAssemblyAccessor>();
        var assembly = accessor.GetEntryAssembly()!;
        AssemblyName = assembly.Name;
        Version = assembly.Version.ToString();
        var ata = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        Name = ata ?? AssemblyName;
        var ada = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        Description = ada ?? string.Empty;

        Configuration = Services.GetRequiredService<IConfiguration>();
        Output = Services.GetRequiredService<IOutput>();
        Input = Services.GetRequiredService<IInput>();
        DateTime = Services.GetRequiredService<IDateTimeProvider>();
        Guid = Services.GetRequiredService<IGuidProvider>();
        FileSystem = Services.GetRequiredService<IFileSystem>();
    }

    public string AssemblyName { get; }
    public string Name { get; }
    public string[] Ids => [Name];
    public string FullName => string.Join(" v", Ids[0], Version);

    public string Version { get; }
    public string Description { get; init; }

    public IServiceProvider Services { get; }
    public IConfiguration Configuration { get; }

    public IOutput Output { get; }
    public IInput Input { get; }
    public IDateTimeProvider DateTime { get; }
    public IGuidProvider Guid { get; }
    public IFileSystem FileSystem { get; }

    public ICollection<INode> Children { get; } = new HashSet<INode>();
    public IDictionary<string, object?> Data { get; } = new Dictionary<string, object?>();

    public abstract void AppendHelp(StringBuilder builder);
    Task<Result> INode.ExecuteAsync(CancellationToken ct) => throw new InvalidOperationException();
    Task<Result> INode.ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct) => throw new InvalidOperationException();
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
        var options = Services.GetService<IOptions<TOptions>>();
        Options = options?.Value ?? new TOptions();
        Environment = environment ?? Options.Environment;

        var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
        Logger = loggerFactory.CreateLogger<TApplication>();

        AddCommand<HelpOption>();
        AddCommand<VersionOption>();
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public void AppendVersion(StringBuilder builder)
        => builder.Append(Name).Append(" v").AppendLine(Version);

    public override void AppendHelp(StringBuilder builder) {
        AppendVersion(builder);
        builder.AppendLine(Description);
    }

    public string[] Arguments { get; }
    public string Environment { get; }
    public TOptions Options { get; }

    public ILogger<TApplication> Logger { get; init; }

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);

    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public TApplication AddCommand(string name, Func<Result> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, (Func<Command, Result>)((_) => action())));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, Func<Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, (Command _, CancellationToken _) => action()));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, Func<CancellationToken, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, (Command _, CancellationToken ct) => action(ct)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, Func<Command, Result> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, (Command cmd, CancellationToken _) => action(cmd)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, Func<Command, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, (Command cmd, CancellationToken _) => action(cmd)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, Func<Command, CancellationToken, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, action));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string alias, Func<Result> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, alias, (Command _, CancellationToken _) => action()));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string alias, Func<Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, alias, (Command _, CancellationToken _) => action()));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string alias, Func<CancellationToken, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, alias, (Command _, CancellationToken ct) => action(ct)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string alias, Func<Command, Result> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, alias, (Command cmd, CancellationToken _) => action(cmd)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string alias, Func<Command, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, alias, (Command cmd, CancellationToken _) => action(cmd)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string alias, Func<Command, CancellationToken, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, alias, action));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string[] aliases, Func<Result> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, aliases, (Command _, CancellationToken _) => action()));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string[] aliases, Func<Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, aliases, (Command _, CancellationToken _) => action()));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string[] aliases, Func<CancellationToken, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, aliases, (Command _, CancellationToken ct) => action(ct)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string[] aliases, Func<Command, Result> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, aliases, (Command cmd, CancellationToken _) => action(cmd)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string[] aliases, Func<Command, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, aliases, (Command cmd, CancellationToken _) => action(cmd)));
        return (TApplication)this;
    }

    public TApplication AddCommand(string name, string[] aliases, Func<Command, CancellationToken, Task<Result>> action) {
        Children.Add(CreateInstance.Of<Command>(this, name, aliases, action));
        return (TApplication)this;
    }

    public TApplication AddCommand<TChildCommand>()
        where TChildCommand : NodeWithArguments<TChildCommand> {
        Children.Add(CreateInstance.Of<TChildCommand>(Services, this));
        return (TApplication)this;
    }

    public TApplication AddOption(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Option>(this, name, aliases));
        return (TApplication)this;
    }

    public TApplication AddOption<TOption>()
        where TOption : Option<TOption>, IOption {
        Children.Add(CreateInstance.Of<TOption>(Services, this));
        return (TApplication)this;
    }

    public TApplication AddParameter(string name, object? defaultValue = default) {
        Children.Add(CreateInstance.Of<Parameter>(this, name, defaultValue));
        return (TApplication)this;
    }

    public TApplication AddParameter<TParameter>()
        where TParameter : Parameter<TParameter> {
        Children.Add(CreateInstance.Of<TParameter>(Services, this));
        return (TApplication)this;
    }

    public TApplication AddFlag(string name, params string[] aliases) {
        Children.Add(CreateInstance.Of<Flag>(this, name, aliases));
        return (TApplication)this;
    }

    public TApplication AddFlag<TFlag>()
        where TFlag : Flag<TFlag> {
        Children.Add(CreateInstance.Of<TFlag>(Services, this));
        return (TApplication)this;
    }

    public sealed override void Exit(int exitCode = 0) {
        _exitCode = exitCode;
        IsRunning = false;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        try {
            IsRunning = true;
            var taskRun = new CancellationTokenSource();
            await ExecuteInternalAsync(taskRun.Token);
            return _exitCode;
        }
        catch (ConsoleException ex) {
            Output.WriteLine(FormatException(ex));
            return ex.ExitCode;
        }
        catch (Exception ex) {
            Output.WriteLine(FormatException(ex));
            return DefaultErrorCode;
        }
    }

    protected abstract Task ExecuteInternalAsync(CancellationToken ct);

    protected void ProcessResult(Result result) {
        if (result.HasException) throw result.Exception!;
        if (!result.HasErrors) return;
        Output.WriteLine(FormatValidationErrors(result.Errors));
    }

    protected async Task<bool> HasInvalidArguments(CancellationToken ct) {
        var result = await ArgumentsReader.Read(Arguments, [.. Children], ct);
        if (!result.HasErrors) return false;
        Output.WriteLine(FormatValidationErrors(result.Errors));
        return true;
    }

    protected async Task<Result> ProcessUserInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var child = Children.FirstOrDefault(FindChildById);
        if (child is null) return Invalid($"Command '{input[0]}' not found. For a list of available commands use 'help'.");
        var arguments = input.Length > 1 ? input.Skip(1).ToArray() : [];
        return await child.ExecuteAsync(arguments, ct);

        bool FindChildById(INode c) => c.Ids.Contains(input[0], InvariantCultureIgnoreCase);
    }

    public async ValueTask DisposeAsync() {
        await Dispose();
        GC.SuppressFinalize(this);
    }
}
