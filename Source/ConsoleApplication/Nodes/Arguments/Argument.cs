namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Argument<TArgument>
    : Node<TArgument>, IArgument
    where TArgument : Argument<TArgument> {
    protected Argument(IHasChildren parent, string type, string name, params string[] aliases)
        : base(parent, name, aliases) {
        Type = type;
    }

    public string Type { get; }

    protected abstract Task<Result> OnRead(CancellationToken ct);
}
