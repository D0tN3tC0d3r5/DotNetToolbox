namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class ApplicationBase<TApplication, TBuilder>(string[] args, IServiceProvider services)
    : ApplicationBase(args, services),
      IApplication<TApplication, TBuilder>
    where TApplication : ApplicationBase<TApplication, TBuilder>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder> {
    private int _exitCode = DefaultExitCode;

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

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

    public int Run() => RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        try {
            IsRunning = true;
            var taskRun = new CancellationTokenSource();
            if (Context.TryGetValue("ClearScreenOnStart", out var clearScreen) && clearScreen == "true")
                Environment.Output.ClearScreen();
            if (await TryParseArguments(taskRun.Token).ConfigureAwait(false))
                return DefaultErrorCode;
            if (!IsRunning) return _exitCode;
            await Run(taskRun.Token).ConfigureAwait(false);
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
        var result = await ArgumentsParser.Parse(this, Arguments, ct).ConfigureAwait(false);
        ProcessResult(result);
        return result.HasErrors;
    }

    protected async Task<Result> ProcessCommand(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = FindCommand((this as IHasChildren).Commands, input[0]);
        if (command is null) return Invalid($"Command '{input[0]}' not found. For a list of available commands use 'help'.");
        var arguments = input.Skip(1).ToArray();
        return await command.Set(arguments, ct).ConfigureAwait(false);
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

public abstract class ApplicationBase : IApplication {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;

    protected ApplicationBase(string[] args, IServiceProvider services) {
        Services = services;
        Arguments = args;
        Logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().Name);
        Environment = services.GetRequiredService<IEnvironment>();
        Ask = new QuestionFactory(Environment.Output, Environment.Input);

        var assembly = services.GetRequiredKeyedService<IAssemblyDescriptor>(Environment.Name);
        AssemblyName = assembly.Name;
        Version = assembly.Version.ToString();
        var ata = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        Name = ata ?? AssemblyName;
        var ada = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
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
    public IEnvironment Environment { get; }
    public NodeContext Context { get; } = [];
    public IQuestionFactory Ask { get; }

    public ICollection<INode> Children { get; } = new HashSet<INode>();
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    public abstract void ExitWith(int exitCode = 0);

    protected string[] Arguments { get; }
    protected bool IsRunning { get; set; }

    IApplication INode.Application => this;
    string[] INode.Aliases => [];

    internal abstract Task Run(CancellationToken ct);

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
