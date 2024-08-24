namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class ApplicationBase<TApplication, TBuilder>(string[] args, IServiceProvider services)
    : ApplicationBase(args, services),
      IApplication<TApplication, TBuilder>
    where TApplication : ApplicationBase<TApplication, TBuilder>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder> {
    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public static TBuilder CreateBuilder(Action<IConfigurationBuilder>? setConfiguration = null)
        => CreateBuilder([], setConfiguration);
    public static TBuilder CreateBuilder(string[] args, Action<IConfigurationBuilder>? setConfiguration = null) {
        Action<IConfigurationBuilder> defaultAction = _ => {};
        return  InstanceFactory.Create<TBuilder>(args, setConfiguration ?? defaultAction);
    }

    public static TApplication Create(string[] args, Action<IConfigurationBuilder> setConfiguration, Action<TBuilder> configureBuilder) {
        var builder = CreateBuilder(args, setConfiguration);
        configureBuilder?.Invoke(builder);
        return builder.Build();
    }

    public static TApplication Create(Action<IConfigurationBuilder> setConfiguration, Action<TBuilder> configureBuilder)
        => Create([], setConfiguration, configureBuilder);

    public static TApplication Create(string[] args, Action<IConfigurationBuilder> setConfiguration)
        => Create(args, setConfiguration, null!);

    public static TApplication Create(Action<IConfigurationBuilder> setConfiguration)
        => Create([], setConfiguration, null!);

    public static TApplication Create(string[] args, Action<TBuilder> configureBuilder)
        => Create(args, null!, configureBuilder);

    public static TApplication Create(Action<TBuilder> configureBuilder)
        => Create([], null!, configureBuilder);

    public static TApplication Create(string[] args)
        => Create(args, null!, null!);

    public static TApplication Create()
        => Create([], null!, null!);

    public override string ToString()
        => $"{GetType().Name}: {Name} v{Version} => {Description}";

    public int Run() => RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        try {
            IsRunning = true;
            var taskRun = new CancellationTokenSource();
            if (Context.TryGetValue("ClearScreenOnStart", out var clearScreen) && clearScreen is true)
                Output.ClearScreen();
            if (await TryParseArguments(taskRun.Token).ConfigureAwait(false))
                return IApplication.DefaultErrorCode;
            if (!IsRunning) return ExitCode;
            await Run(taskRun.Token).ConfigureAwait(false);
            return ExitCode;
        }
        catch (ConsoleException ex) {
            Output.WriteLine(ex.ToText());
            return ex.ExitCode;
        }
        catch (Exception ex) {
            Output.WriteLine(ex.ToText());
            return IApplication.DefaultErrorCode;
        }
    }

    protected void ProcessResult(Result result) {
        if (result.HasException) throw result.Exception!;
        if (!result.HasErrors) return;
        Output.WriteLine(result.Errors.ToText());
    }

    protected async Task<bool> TryParseArguments(CancellationToken ct) {
        var result = await ArgumentsParser.Parse(this, Arguments, ct).ConfigureAwait(false);
        ProcessResult(result);
        return result.HasErrors;
    }

    protected virtual async Task<Result> ProcessCommand(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = FindCommand((this as IHasChildren).Commands, input[0]);
        if (command is null) return Invalid($"Command '{input[0]}' not found. For a list of available commands use 'help'.");
        var arguments = input.Skip(1).ToArray();
        return await command.Set(arguments, ct).ConfigureAwait(false);
    }

    private static ICommand? FindCommand(IEnumerable<ICommand> commands, string token)
        => token.StartsWith('"') || token.StartsWith('-')
               ? null
               : commands.FirstOrDefault(c => c.Name.Equals(token, StringComparison.OrdinalIgnoreCase)
                                           || c.Aliases.Contains(token));

    public async ValueTask DisposeAsync() {
        await Dispose();
        GC.SuppressFinalize(this);
    }
}

public abstract class ApplicationBase : IApplication {
    public int ExitCode { get; protected set; } = IApplication.DefaultExitCode;

    protected ApplicationBase(string[] args, IServiceProvider services) {
        Services = services;
        Arguments = args;
        Logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().Name);
        Environment = services.GetRequiredService<IApplicationEnvironment>();
        Configuration = services.GetRequiredService<IConfigurationRoot>();
        PromptFactory = services.GetRequiredService<IPromptFactory>();

        AssemblyName = Environment.Assembly.Name;
        Version = Environment.Assembly.Version.ToString();
        var ata = Environment.Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        Name = ata ?? AssemblyName;
        var ada = Environment.Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        Description = ada ?? string.Empty;

        AddFlag<HelpFlag>();
        AddFlag<ClearScreenFlag>();
        AddFlag<VersionFlag>();
        IsRunning = false;
    }

    public string AssemblyName { get; }
    public string Name { get; }
    public string Version { get; }
    public string FullName => $"{Name} v{Version}";
    public string Description { get; init; }
    public ILogger Logger { get; init; }

    public IServiceProvider Services { get; }
    public IConfigurationRoot Configuration { get; }
    public IApplicationEnvironment Environment { get; }
    protected IOutput Output => Environment.OperatingSystem.Output;
    protected IInput Input => Environment.OperatingSystem.Input;
    protected IFileSystemAccessor FileSystem => Environment.OperatingSystem.FileSystem;
    protected IAssemblyDescriptor Assembly => Environment.Assembly;
    protected IDateTimeProvider DateTime => Environment.OperatingSystem.DateTime;
    protected IGuidProvider Guid => Environment.OperatingSystem.Guid;
    public IPromptFactory PromptFactory { get; }

    public NodeContext Context { get; } = [];

    public ICollection<INode> Children { get; } = [];
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    public virtual void Exit(int code = IApplication.DefaultExitCode) {
        ExitCode = code;
        IsRunning = false;
    }

    protected string[] Arguments { get; }
    protected bool IsRunning { get; set; }

    IApplication INode.Application => this;
    string[] INode.Aliases => [];

    internal abstract Task Run(CancellationToken ct = default);

    public ICommand AddCommand(string name, Delegate action)
        => AddCommand(name, aliases: [], action);
    public ICommand AddCommand(string name, string alias, Delegate action)
        => AddCommand(name, [alias], action);
    public ICommand AddCommand(string name, string[] aliases, Delegate action)
        => NodeFactory.Create<Command>(this, name, aliases, action);
    public ICommand AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand>, ICommand
        => NodeFactory.Create<TChildCommand>(this);
    public void AddCommand(ICommand command) => Children.Add(command);

    public IFlag AddFlag(string name, Delegate? action = null)
        => AddFlag(name, aliases: [], action);
    public IFlag AddFlag(string name, string alias, Delegate? action = null)
        => AddFlag(name, [alias], action);
    public IFlag AddFlag(string name, string[] aliases, Delegate? action = null)
        => NodeFactory.Create<Flag>(this, name, aliases, action);
    public IFlag AddFlag<TFlag>()
        where TFlag : Flag<TFlag>, IFlag
        => NodeFactory.Create<TFlag>(this);
    public void AddFlag(IFlag flag) => Children.Add(flag);

    public IOption AddOption(string name)
        => AddOption(name, aliases: []);
    public IOption AddOption(string name, string alias)
        => AddOption(name, [alias]);
    public IOption AddOption(string name, string[] aliases)
        => NodeFactory.Create<Option>(this, name, aliases);
    public IOption AddOption<TOption>()
        where TOption : Option<TOption>, IOption
        => NodeFactory.Create<TOption>(this);
    public void AddOption(IOption option) => Children.Add(option);

    public IParameter AddParameter(string name)
        => NodeFactory.Create<Parameter>(this, name, default(string));
    public IParameter AddParameter(string name, string defaultValue)
        => NodeFactory.Create<Parameter>(this, name, IsNotNull(defaultValue));
    public IParameter AddParameter<TParameter>()
        where TParameter : Parameter<TParameter>, IParameter
        => NodeFactory.Create<TParameter>(this);
    public void AddParameter(IParameter parameter) => Children.Add(parameter);
}
