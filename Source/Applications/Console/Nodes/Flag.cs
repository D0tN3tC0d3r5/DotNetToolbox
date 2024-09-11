namespace DotNetToolbox.ConsoleApplication.Nodes;

public sealed class Flag(IHasChildren parent, string name, string[] aliases, Func<Flag, CancellationToken, Task<Result>>? execute = null)
    : Flag<Flag>(parent, name, aliases, execute);

public abstract class Flag<TFlag>(IHasChildren parent, string name, string[] aliases, Func<TFlag, CancellationToken, Task<Result>>? execute = null)
    : Node<TFlag>(parent, name, aliases), IFlag
    where TFlag : Flag<TFlag> {
    Task<Result> IFlag.Read(IMap context, CancellationToken ct) {
        context[Name] = bool.TrueString;
        return Execute(ct);
    }

    protected virtual Task<Result> Execute(CancellationToken ct = default)
        => execute?.Invoke((TFlag)this, ct) ?? SuccessTask();
}
