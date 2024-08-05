namespace DotNetToolbox.Graph.Nodes;

public abstract class Node<TNode>(uint id, string label, IServiceProvider services)
    : INode
    where TNode : Node<TNode> {
    protected Node(uint id, IServiceProvider services)
        : this (id, typeof(TNode).Name, services) {
    }

    public uint Id { get; } = id;
    public string Label { get; } = IsNotNull(label);
    public INode? Next { get; set; }
    protected IServiceProvider Services { get; } = IsNotNull(services);

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
