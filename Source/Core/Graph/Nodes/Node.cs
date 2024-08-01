namespace DotNetToolbox.Graph.Nodes;

public abstract class Node<TNode>
    : INode
    where TNode : Node<TNode> {
    protected Node(uint id, string? label = null) {
        Id = id;
        Label = label ?? Label;
    }

    public uint Id { get; }
    public string Label { get; } = typeof(TNode).Name;
    public INode? Next { get; set; }

    public Result Validate(ISet<INode>? visited = null) {
        visited ??= new HashSet<INode>();
        return !visited.Add(this)
            ? Success()
            : IsValid(visited);
    }

    protected virtual Result IsValid(ISet<INode> visited) => Success();

    public INode? Run(Context context) {
        UpdateState(context);
        return GetNext(context);
    }

    protected abstract void UpdateState(Context context);
    protected abstract INode? GetNext(Context context);

    public override int GetHashCode() => Id.GetHashCode();
}
