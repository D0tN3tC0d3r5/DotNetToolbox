namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public abstract class Action<TAction>
    : Executable<TAction>
    , IAction
    where TAction : Action<TAction> {

    protected Action(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }

    public string Type => "Action";

    protected sealed override Task<Result> ReadInput(string[] input, CancellationToken ct)
        => SuccessTask();
}
