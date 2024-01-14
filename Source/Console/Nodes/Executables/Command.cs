namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public sealed class Command : Command<Command> {
    internal Command(IHasChildren node, string name, string[] aliases, Func<Command, Result> execute)
        : base(node, name, aliases, execute) {
    }
}

public class Command<TCommand>
    : CommandBase<TCommand>
    , ICommand
    where TCommand : Command<TCommand> {
    private readonly Func<TCommand, Result>? _execute;

    protected Command(IHasChildren node, string name, params string[] aliases)
        : base(node, name, aliases) {
    }

    internal Command(IHasChildren node, string name, string[] aliases, Func<TCommand, Result> execute)
        : this(node, name, aliases) {
        _execute = IsNotNull(execute);
    }

    public sealed override async Task<Result> ExecuteAsync(string[] args, CancellationToken ct = default) {
        var result = await ArgumentsReader.Read(args, Children.ToArray(), ct);
        return result.IsSuccess
                   ? await Task.Run(() => _execute?.Invoke((TCommand)this) ?? Execute(), ct)
                   : result;
    }

    protected virtual Result Execute() => Success();
}
