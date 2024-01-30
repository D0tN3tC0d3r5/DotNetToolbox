namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Command : Command<Command> {
    internal Command(IHasChildren parent, string name, string[] aliases, Func<Command, CancellationToken, Task<Result>> execute)
        : base(parent, name, aliases, execute) {
    }
}

public class Command<TCommand>(IHasChildren parent, string name, string[] aliases, Func<TCommand, CancellationToken, Task<Result>>? execute = null)
    : NodeWithChildren<TCommand>(parent, name, aliases)
    , ICommand
    where TCommand : Command<TCommand> {

    async Task<Result> ICommand.Set(IReadOnlyList<string> args, CancellationToken ct) {
        var result = await ArgumentsParser.Parse(this, args, ct);
        return result.IsSuccess
                   ? await Execute(ct)
                   : result;
    }

    public virtual Task<Result> Execute(CancellationToken ct = default)
        => execute?.Invoke((TCommand)this, ct) ?? SuccessTask();
}
