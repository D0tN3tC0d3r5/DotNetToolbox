namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class ApplicationBase : IApplication {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;

    protected ApplicationBase(IServiceProvider services) {
        Context = [];
        Services = services;
        Environment = services.GetRequiredService<IEnvironment>();

        var assembly = services.GetRequiredKeyedService<IAssemblyDescriptor>(Environment.Name);
        AssemblyName = assembly.Name;
        Version = assembly.Version.ToString();
        var ata = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        Name = ata ?? AssemblyName;
        var ada = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        Description = ada ?? string.Empty;

        AddFlag<HelpFlag>();
        AddFlag<ClearScreenFlag>();
    }

    public string AssemblyName { get; }
    public string Name { get; }
    public string Version { get; }
    public string FullName => $"{Name} v{Version}";
    public string Description { get; init; }

    public IServiceProvider Services { get; }
    public IEnvironment Environment { get; }
    public NodeContext Context { get; }
    public ICollection<INode> Children { get; } = new HashSet<INode>();
    public abstract void ExitWith(int exitCode = 0);

    IApplication INode.Application => this;
    string[] INode.Aliases => [];

    internal abstract Task Run(CancellationToken ct);
    protected abstract Task<Result> Execute(CancellationToken ct);

    public ICommand AddCommand(string name, Delegate action)
        => AddCommand(name, Array.Empty<string>(), action);
    public ICommand AddCommand(string name, string alias, Delegate action)
        => AddCommand(name, [alias], action);
    public ICommand AddCommand(string name, string[] aliases, Delegate action)
        => NodeFactory.AddExecutableNode<Command>(this, name, aliases, action);
    public ICommand AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand>, ICommand
        => NodeFactory.AddNode<TChildCommand>(this);

    public IFlag AddFlag(string name, Delegate? action = null)
        => AddFlag(name, Array.Empty<string>(), action);
    public IFlag AddFlag(string name, string alias, Delegate? action = null)
        => AddFlag(name, [alias], action);
    public IFlag AddFlag(string name, string[] aliases, Delegate? action = null)
        => NodeFactory.AddExecutableNode<Flag>(this, name, aliases, action);
    public IFlag AddFlag<TFlag>()
        where TFlag : Flag<TFlag>, IFlag
        => NodeFactory.AddNode<TFlag>(this);

    public IOption AddOption(string name)
        => AddOption(name, Array.Empty<string>());
    public IOption AddOption(string name, string alias)
        => AddOption(name, [alias]);
    public IOption AddOption(string name, string[] aliases)
        => NodeFactory.AddNode<Option>(this, name, aliases);
    public IOption AddOption<TOption>()
        where TOption : Option<TOption>, IOption
        => NodeFactory.AddNode<TOption>(this);

    public IParameter AddParameter(string name)
        => AddParameter(name, null);
    public IParameter AddParameter(string name, string? defaultValue)
        => NodeFactory.AddNode<Parameter>(this, name, defaultValue);
    public IParameter AddParameter<TParameter>()
        where TParameter : Parameter<TParameter>, IParameter
        => NodeFactory.AddNode<TParameter>(this);

}

public abstract class Application<TApplication, TBuilder, TSettings>
    : ApplicationBase, IApplication<TApplication, TBuilder, TSettings>
    where TApplication : Application<TApplication, TBuilder, TSettings>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationOptions<TSettings>
                   , IHasDefault<TSettings>
                   , new() {
    private int _exitCode = DefaultExitCode;

    protected Application(string[] args, IServiceProvider services)
        : base(services) {
        Arguments = args;
        var settings = Services.GetService<IOptions<TSettings>>();
        Settings = settings?.Value ?? new TSettings();
        var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
        Logger = loggerFactory.CreateLogger<TApplication>();
    }

    public TSettings Settings { get; }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    protected string[] Arguments { get; }
    protected bool IsRunning { get; private set; }
    public ILogger<TApplication> Logger { get; init; }

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);

    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public override string ToString()
        => $"{GetType().Name}: {Name} v{Version} => {Description}";

    public sealed override void ExitWith(int exitCode = 0) {
        _exitCode = exitCode;
        IsRunning = false;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        try {
            IsRunning = true;
            var taskRun = new CancellationTokenSource();
            if (Settings.ClearScreenOnStart) Environment.Output.ClearScreen();
            if (await TryParseArguments(taskRun.Token)) return DefaultErrorCode;
            await Run(taskRun.Token);
            return _exitCode;
        }
        catch (ConsoleException ex) {
            Environment.Output.WriteLine(FormatException(ex));
            return ex.ExitCode;
        }
        catch (Exception ex) {
            Environment.Output.WriteLine(FormatException(ex));
            return DefaultErrorCode;
        }
    }

    protected void ProcessResult(Result result) {
        if (result.HasException) throw result.Exception!;
        if (!result.HasErrors) return;
        Environment.Output.WriteLine(FormatValidationErrors(result.Errors));
    }

    protected async Task<bool> TryParseArguments(CancellationToken ct) {
        var result = await ArgumentsParser.Parse(this, Arguments, ct);
        ProcessResult(result);
        return result.HasErrors;
    }

    protected async Task<Result> ProcessUserInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = FindCommand((this as IHasChildren).Commands, input[0]);
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
