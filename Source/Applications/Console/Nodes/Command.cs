namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Command : Command<Command> {
    internal Command(IHasChildren parent, string name, Func<Command, CancellationToken, Task<Result>> executeAsync)
        : this(parent, name, [], executeAsync) {
    }
    internal Command(IHasChildren parent, string name, Func<Command, Result> execute)
        : this(parent, name, [], execute) {
    }
    internal Command(IHasChildren parent, string name, string[] aliases, Func<Command, CancellationToken, Task<Result>> executeAsync)
        : base(parent, name, aliases, executeAsync) {
    }
    internal Command(IHasChildren parent, string name, string[] aliases, Func<Command, Result> execute)
        : base(parent, name, aliases, execute) {
    }
    internal Command(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }
}

public class Command<TCommand>
    : Node<TCommand>,
      ICommand
    where TCommand : Command<TCommand> {
    private readonly Func<TCommand, CancellationToken, Task<Result>>? _executeAsync;

    public Command(IHasChildren parent, string name, Func<TCommand, CancellationToken, Task<Result>> executeAsync)
        : this(parent, name, [], executeAsync) {
    }

    public Command(IHasChildren parent, string name, Func<TCommand, Result> execute)
        : this(parent, name, [], execute) {
    }

    public Command(IHasChildren parent, string name, string[] aliases, Func<TCommand, CancellationToken, Task<Result>> executeAsync)
        : base(parent, name, aliases) {
        _executeAsync = executeAsync;
    }

    public Command(IHasChildren parent, string name, string[] aliases, Func<TCommand, Result> execute)
        : this(parent, name, aliases, (cmd, ct) => Task.Run(() => execute(cmd), ct)) {
    }

    public Command(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public IContext Context { get; } = new Context();

    public ICollection<INode> Children { get; } = [];
    public IParameter[] Parameters => [.. Children.OfType<IParameter>().OrderBy(i => i.Order)];
    public IArgument[] Options => [.. Children.OfType<IArgument>().OrderBy(i => i.Name)];
    public ICommand[] Commands => [.. Children.OfType<ICommand>().Except(Options.Cast<INode>()).Cast<ICommand>().OrderBy(i => i.Name)];

    protected virtual Task<Result> Prepare(IReadOnlyList<string> args, CancellationToken ct = default)
        => ArgumentsParser.Parse(this, args, ct);

    protected virtual Task<Result> ExecuteAsync(CancellationToken ct = default)
        => _executeAsync?.Invoke((TCommand)this, ct) ?? Task.Run(Execute, ct);
    protected virtual Result Execute() => Success();
    public async Task<Result> Execute(IReadOnlyList<string> args, CancellationToken ct = default) {
        var result = await Prepare(args, ct);
        return result.IsSuccess ? await ExecuteAsync(ct) : result;
    }

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
        => AddParameter(name, null);
    public IParameter AddParameter(string name, string? defaultValue)
        => NodeFactory.Create<Parameter>(this, name, defaultValue);
    public IParameter AddParameter<TParameter>()
        where TParameter : Parameter<TParameter>, IParameter
        => NodeFactory.Create<TParameter>(this);
    public void AddParameter(IParameter parameter) => Children.Add(parameter);
}
