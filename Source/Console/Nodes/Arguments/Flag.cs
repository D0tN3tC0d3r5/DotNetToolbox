namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public sealed class Flag
    : Flag<Flag> {
    internal Flag(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }
}

public abstract class Flag<TFlag>(IHasChildren parent, string name, params string[] aliases)
    : Node<TFlag>(parent, name, aliases), IFlag
    where TFlag : Flag<TFlag> {

    public sealed override Task<Result> ExecuteAsync(IReadOnlyList<string> args, CancellationToken ct = default) {
        Application.Data[Name] = bool.TrueString;
        return SuccessTask();
    }
}
