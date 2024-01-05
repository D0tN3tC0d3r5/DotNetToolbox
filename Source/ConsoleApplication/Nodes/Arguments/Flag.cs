namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public class Flag
    : Flag<Flag> {
    internal Flag(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected override Task<Result> OnRead(CancellationToken ct)
        => SuccessTask();
}

public abstract class Flag<TFlag>
    : Argument<TFlag>
    , IFlag, IHasValue<bool>
    where TFlag : Flag<TFlag> {
    protected Flag(IHasChildren parent, string name, params string[] aliases)
        : base(parent, "Flag", name, aliases) {
    }

    public bool Value { get; private set; }
    public Task<Result> SetValue(string input, CancellationToken ct) {
        Value = true;
        return OnRead(ct);
    }
}
