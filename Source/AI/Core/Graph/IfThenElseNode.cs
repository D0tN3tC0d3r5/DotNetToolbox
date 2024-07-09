namespace DotNetToolbox.AI.Graph;

public abstract class IfThenElseNode(uint id, INode? caller = null, INode? trueBranch = null, INode? falseBranch = null)
    : SwitchNode(id, caller, [trueBranch, falseBranch]) {
    protected sealed override INode? SelectBranch(Map state)
        => Predicate(state) ? trueBranch : falseBranch;

    protected abstract bool Predicate(Map state);
}
