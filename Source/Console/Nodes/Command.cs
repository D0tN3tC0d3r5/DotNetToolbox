namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Command : Command<Command> {
    internal Command(IHasChildren parent, string name, string[] aliases, Func<Command, CancellationToken, Task<Result>> execute)
        : base(parent, name, aliases, execute) {
    }
}

public class Command<TCommand>(IHasChildren parent, string name, string[] aliases, Func<TCommand, CancellationToken, Task<Result>>? execute = null)
    : Node<TCommand>(parent, name, aliases), ICommand
    where TCommand : Command<TCommand> {
    public NodeContext Context { get; } = [];

    public ICollection<INode> Children { get; } = new HashSet<INode>();

    async Task<Result> ICommand.Set(IReadOnlyList<string> args, CancellationToken ct) {
        var result = await ArgumentsParser.Parse(this, args, ct);
        return result.IsSuccess
                   ? await Execute(ct)
                   : result;
    }

    public virtual Task<Result> Execute(CancellationToken ct = default)
        => execute?.Invoke((TCommand)this, ct) ?? SuccessTask();

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
