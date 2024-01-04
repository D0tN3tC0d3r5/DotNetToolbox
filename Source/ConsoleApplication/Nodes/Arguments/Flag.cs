namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Flag<TFlag>
    : Argument<TFlag>
    , IFlag
    where TFlag : Flag<TFlag> {
    protected Flag(ICommand owner, string name)
        : base(IsNotNull(owner), "Flag", name) {
    }

    public bool IsSet { get; private set; }
    public Task<Result> SetValue(string input, CancellationToken ct) {
        IsSet = true;
        return OnRead(ct);
    }
}
