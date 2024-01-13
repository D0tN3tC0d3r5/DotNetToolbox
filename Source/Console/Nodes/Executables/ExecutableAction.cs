namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public abstract class ExecutableAction<TAction>
    : Executable<TAction>
    , IAction
    where TAction : ExecutableAction<TAction> {

    protected ExecutableAction(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public override Task<Result> ExecuteAsync(string[] _, CancellationToken ct)
        => ExecuteAsync(ct);
}
