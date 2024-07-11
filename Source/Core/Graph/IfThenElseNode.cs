namespace DotNetToolbox.Graph;

public abstract class IfThenElseNode(string id, Func<Map, bool>? predicate = null)
    : Node(id) {
    public void SetTrue(INode node) => ConnectTo(node, true);
    public void SetFalse(INode node) => ConnectTo(node, false);

    public override void ConnectTo(INode node, object? metadata = null) {
        if (metadata is bool condition && Exits.Any(e => e.Value == condition))
            return;
        base.ConnectTo(node, metadata);
    }

    public sealed override INode? Execute(INode caller)
        => IsNotNull(predicate)(State)
                   ? Exits[0]
                   : Exits[1];

    protected abstract bool Predicate(Map state, INode? caller = null);
}
