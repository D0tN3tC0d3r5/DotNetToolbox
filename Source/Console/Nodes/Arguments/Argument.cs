namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Argument<TArgument>
    : Node<TArgument>, IArgument
    where TArgument : Argument<TArgument> {
    protected Argument(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public abstract Task<Result> ClearData(CancellationToken ct);
    public abstract Task<Result> ReadData(string? value, CancellationToken ct);
    protected virtual Task<Result> OnDataCleared(CancellationToken ct) => SuccessTask();
    protected virtual Task<Result> OnDataRead(CancellationToken ct) => SuccessTask();
}
