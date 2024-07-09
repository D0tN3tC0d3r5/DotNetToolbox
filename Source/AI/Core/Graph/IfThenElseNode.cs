namespace DotNetToolbox.AI.Graph;

public abstract class IfThenElseNode(string id, IEnumerable<INode?>? entries = null, INode? trueBranch = null, INode? falseBranch = null)
    : SwitchNode(id, entries, [trueBranch, falseBranch]) {
    protected sealed override INode? SelectExit(Map state, INode? caller = null)
        => Predicate(state, caller) ? trueBranch : falseBranch;

    protected abstract bool Predicate(Map state, INode? caller = null);
}
