namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public sealed class Command : Command<Command> {
    internal Command(IHasChildren node, string name, Func<Command, Result> execute)
        : this(node, name, Array.Empty<string>(), execute) {
    }

    internal Command(IHasChildren node, string name, string alias, Func<Command, Result> execute)
        : this(node, name, [ alias ], execute) {
    }

    internal Command(IHasChildren node, string name, string[] aliases, Func<Command, Result> execute)
        : base(node, name, aliases, execute) {
    }

    internal Command(IHasChildren node, string name, Func<Command, CancellationToken, Task<Result>> execute)
        : this(node, name, Array.Empty<string>(), execute) {
    }

    internal Command(IHasChildren node, string name, string alias, Func<Command, CancellationToken, Task<Result>> execute)
        : this(node, name, [alias], execute) {
    }

    internal Command(IHasChildren node, string name, string[] aliases, Func<Command, CancellationToken, Task<Result>> execute)
        : base(node, name, aliases, execute) {
    }
}

public class Command<TCommand>
    : NodeWithArguments<TCommand>
    , ICommand
    where TCommand : Command<TCommand> {
    private readonly Func<TCommand, CancellationToken, Task<Result>>? _execute;

    protected Command(IHasChildren node, string name, params string[] aliases)
        : base(node, name, aliases) {
    }

    internal Command(IHasChildren node, string name, string[] aliases, Func<TCommand, Result> execute)
        : this(node, name, aliases) {
        _execute = (cmd, ct) => Task.Run(() => IsNotNull(execute)(cmd), ct);
    }

    internal Command(IHasChildren node, string name, string[] aliases, Func<TCommand, Task<Result>> execute)
        : this(node, name, aliases) {
        _execute = (cmd, _) => IsNotNull(execute(cmd));
    }

    internal Command(IHasChildren node, string name, string[] aliases, Func<TCommand, CancellationToken, Task<Result>> execute)
        : this(node, name, aliases) {
        _execute = IsNotNull(execute);
    }

    public sealed override async Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default) {
        var result = await ArgumentsReader.Read(args, Children.ToArray(), ct);
        return result.IsSuccess
                   ? await (_execute?.Invoke((TCommand)this, ct) ?? Execute())
                   : result;
    }

    protected virtual Task<Result> Execute() => SuccessTask();
}
