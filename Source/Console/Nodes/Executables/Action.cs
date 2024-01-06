namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

public abstract class Action<TAction>
    : Executable<TAction>
    , IAction
    where TAction : Action<TAction> {

    protected Action(IHasChildren parent, string name, params string[] aliases)
        : base(parent, name, aliases) {
    }
}
