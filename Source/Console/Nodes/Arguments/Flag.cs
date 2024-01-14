namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public sealed class Flag
    : Flag<Flag> {
    internal Flag(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }
}

public abstract class Flag<TFlag>
    : Argument<TFlag>
    , IFlag
    where TFlag : Flag<TFlag> {
    protected Flag(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public Task<Result> SetValue(CancellationToken ct) {
        Application.Data[Name] = true;
        return OnDataRead(ct);
    }
}
