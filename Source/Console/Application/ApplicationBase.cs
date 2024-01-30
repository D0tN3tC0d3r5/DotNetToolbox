namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class ApplicationBase : IApplication {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;

    protected ApplicationBase(IServiceProvider serviceProvider) {
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
    public string Version { get; }
    public string FullName => $"{Name} v{Version}";
    public string Description { get; init; }

    public IServiceProvider Services { get; }
    public IConfiguration Configuration { get; }

    public IOutput Output { get; }
    public IInput Input { get; }
    public IDateTimeProvider DateTime { get; }
    public IGuidProvider Guid { get; }
    public IFileSystem FileSystem { get; }

    public ICollection<INode> Children { get; } = new HashSet<INode>();
    public IDictionary<string, string?> Data { get; } = new Dictionary<string, string?>(CurrentCultureIgnoreCase);
    string[] INode.Aliases => [];

    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    internal abstract Task Run(CancellationToken ct);

    protected abstract Task<Result> Execute(CancellationToken ct);

    public abstract void ExitWith(int exitCode = 0);
}

public abstract class Application<TApplication, TBuilder, TSettings>
    : ApplicationBase, IApplication<TApplication, TBuilder, TSettings>
    where TApplication : Application<TApplication, TBuilder, TSettings>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationOptions<TSettings>
                   , IHasDefault<TSettings>
                   , new() {
    protected bool IsRunning { get; private set; }
    private int _exitCode = DefaultExitCode;

    protected Application(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(serviceProvider) {
        Arguments = args;
        var options = Services.GetService<IOptions<TSettings>>();
        Settings = options?.Value ?? new TSettings();
        Environment = environment ?? Settings.Environment;

        var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
        Logger = loggerFactory.CreateLogger<TApplication>();

        AddFlag<HelpFlag>();
        AddFlag<ClearScreenFlag>();
    }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public void AppendVersion(StringBuilder builder)
        => builder.Append(Name).Append(" v").AppendLine(Version);

    public override string ToString()
        => $"{GetType().Name}: {Name} v{Version} => {Description}";

    public string[] Arguments { get; }
    public string Environment { get; }
    public TSettings Settings { get; }

    public ILogger<TApplication> Logger { get; init; }

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);

    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public ICommand AddCommand(string name, Delegate action) => AddCommand(name, Array.Empty<string>(), action);
    public ICommand AddCommand(string name, string alias, Delegate action) => AddCommand(name, [alias], action);
    internal ICommand AddCommand(string name, string[] aliases, Delegate action) {
        var actionWrapper = ConvertToActionWrapper<Command>(action);
        var child = CreateInstance.Of<Command>(this, name, aliases, actionWrapper);
        Children.Add(child);
        return child;
    }

    public ICommand AddCommand<TChildCommand>()
        where TChildCommand : NodeWithChildren<TChildCommand>, ICommand {
        var child = CreateInstance.Of<TChildCommand>(Services, this);
        Children.Add(child);
        return child;
    }

    public IFlag AddFlag(string name, Delegate? action = null) => AddFlag(name, Array.Empty<string>(), action);
    public IFlag AddFlag(string name, string alias, Delegate? action = null) => AddFlag(name, [alias], action);
    internal IFlag AddFlag(string name, string[] aliases, Delegate? action = null) {
        var actionWrapper = ConvertToActionWrapper<Flag>(action);
        var child = CreateInstance.Of<Flag>(this, name, aliases, actionWrapper);
        Children.Add(child);
        return child;
    }

    public IFlag AddFlag<TFlag>()
        where TFlag : Flag<TFlag>, IFlag {
        var child = CreateInstance.Of<TFlag>(Services, this);
        Children.Add(child);
        return child;
    }

    public IOption AddOption(string name) => AddOption(name, Array.Empty<string>());
    public IOption AddOption(string name, string alias) => AddOption(name, [alias]);
    internal IOption AddOption(string name, string[] aliases) {
        var child = CreateInstance.Of<Option>(this, name, aliases);
        Children.Add(child);
        return child;
    }

    public IOption AddOption<TOption>()
        where TOption : Option<TOption>, IOption {
        var child = CreateInstance.Of<TOption>(Services, this);
        Children.Add(child);
        return child;
    }

    public IParameter AddParameter(string name) => AddParameter(name, null);
    internal IParameter AddParameter(string name, string? defaultValue) {
        var child = CreateInstance.Of<Parameter>(this, name, defaultValue);
        Children.Add(child);
        return child;
    }

    public IParameter AddParameter<TParameter>()
        where TParameter : Parameter<TParameter>, IParameter {
        var child = CreateInstance.Of<TParameter>(Services, this);
        Children.Add(child);
        return child;
    }

    private static Func<TNode, CancellationToken, Task<Result>> ConvertToActionWrapper<TNode>(Delegate? action)
        => action switch {
            null => (_, _) => SuccessTask(),
            Action func => (_, ct) => Task.Run(() => func(), ct).ContinueWith(_ => Success(), ct, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current),
            Action<TNode> func => (c, ct) => Task.Run(() => func(c), ct).ContinueWith(_ => Success(), ct, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current),
            Func<Result> func => (_, _) => Task.FromResult(func()),
            Func<Task<Result>> func => (_, _) => func(),
            Func<Task> func => (_, ct) => func().ContinueWith(_ => Success(), ct),
            Func<CancellationToken, Task<Result>> func => (_, ct) => func(ct),
            Func<CancellationToken, Task> func => (_, ct) => func(ct).ContinueWith(_ => Success(), ct),
            Func<TNode, Result> func => (cmd, _) => Task.FromResult(func(cmd)),
            Func<TNode, Task<Result>> func => (cmd, _) => func(cmd),
            Func<TNode, Task> func => (cmd, ct) => func(cmd).ContinueWith(_ => Success(), ct),
            Func<TNode, CancellationToken, Task<Result>> func => (cmd, ct) => func(cmd, ct),
            Func<TNode, CancellationToken, Task> func => (cmd, ct) => func(cmd, ct).ContinueWith(_ => Success(), ct),
            _ => throw new ArgumentException("Unsupported delegate type for action", nameof(action)),
        };

    public sealed override void ExitWith(int exitCode = 0) {
        _exitCode = exitCode;
        IsRunning = false;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        try {
            IsRunning = true;
            var taskRun = new CancellationTokenSource();
            if (Settings.ClearScreenOnStart) Output.ClearScreen();
            if (await TryParseArguments(taskRun.Token)) return DefaultErrorCode;
            await Run(taskRun.Token);
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

    protected void ProcessResult(Result result) {
        if (result.HasException) throw result.Exception!;
        if (!result.HasErrors) return;
        Output.WriteLine(FormatValidationErrors(result.Errors));
    }

    protected async Task<bool> TryParseArguments(CancellationToken ct) {
        var result = await ArgumentsParser.Parse(this, Arguments, ct);
        ProcessResult(result);
        return result.HasErrors;
    }

    protected async Task<Result> ProcessUserInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = FindCommand(Commands, input[0]);
        if (command is null) return Invalid($"Command '{input[0]}' not found. For a list of available commands use 'help'.");
        var arguments = input.Skip(1).ToArray();
        return await command.Set(arguments, ct);
    }

    private static ICommand? FindCommand(IEnumerable<ICommand> commands, string token)
        => token.StartsWith('"') || token.StartsWith('-')
               ? null
               : commands.FirstOrDefault(c => c.Name.Equals(token, StringComparison.CurrentCultureIgnoreCase)
                                           || c.Aliases.Contains(token));

    public async ValueTask DisposeAsync() {
        await Dispose();
        GC.SuppressFinalize(this);
    }
}
