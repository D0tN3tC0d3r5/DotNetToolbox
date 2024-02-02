namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class ApplicationBase : IApplication {
    public const int DefaultExitCode = 0;
    public const int DefaultErrorCode = 1;

    protected ApplicationBase(string[] args, IServiceProvider services) {
        Services = services;
        Arguments = args;
        Logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(GetType().Name);
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

    public ICollection<INode> Children { get; } = new HashSet<INode>();
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    public abstract void ExitWith(int exitCode = 0);

    protected string[] Arguments { get; }
    protected bool IsRunning { get; set; }

    IApplication INode.Application => this;
    string[] INode.Aliases => [];

    internal abstract Task Run(CancellationToken ct = default);
    protected abstract Task<Result> Execute(CancellationToken ct = default);

    public ICommand AddCommand(string name, Delegate action)
        => AddCommand(name, Array.Empty<string>(), action);
    public ICommand AddCommand(string name, string alias, Delegate action)
        => AddCommand(name, [alias], action);
    public ICommand AddCommand(string name, string[] aliases, Delegate action)
        => NodeFactory.Create<Command>(this, name, aliases, action);
    public ICommand AddCommand<TChildCommand>()
        where TChildCommand : Command<TChildCommand>, ICommand
        => NodeFactory.Create<TChildCommand>(this);
    public void AddCommand(ICommand command) => Children.Add(command);

    public IFlag AddFlag(string name, Delegate? action = null)
        => AddFlag(name, Array.Empty<string>(), action);
    public IFlag AddFlag(string name, string alias, Delegate? action = null)
        => AddFlag(name, [alias], action);
    public IFlag AddFlag(string name, string[] aliases, Delegate? action = null)
        => NodeFactory.Create<Flag>(this, name, aliases, action);
    public IFlag AddFlag<TFlag>()
        where TFlag : Flag<TFlag>, IFlag
        => NodeFactory.Create<TFlag>(this);
    public void AddFlag(IFlag flag) => Children.Add(flag);

    public IOption AddOption(string name)
        => AddOption(name, Array.Empty<string>());
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
