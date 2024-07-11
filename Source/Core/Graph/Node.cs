namespace DotNetToolbox.Graph;

public abstract class Node(string id)
    : INode {
    public string Id => id;
    public Map State { get; set; } = [];
    protected Dictionary<INode, object?> Exits { get; } = [];

    public virtual void ConnectTo(INode node, object? metadata = null) {
        if (!Exits.TryAdd(node, metadata))
            throw new InvalidOperationException($"Node '{Id}' already has a connection to node '{node.Id}' registered.");
    }

    public abstract INode? Execute(INode caller);

    public override int GetHashCode() => id.GetHashCode();
}
