namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Argument<TArgument>
    : Node<TArgument>
    where TArgument : Argument<TArgument> {
    protected Argument(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    protected virtual Task<Result> OnDataRead(CancellationToken ct) => SuccessTask();
}
