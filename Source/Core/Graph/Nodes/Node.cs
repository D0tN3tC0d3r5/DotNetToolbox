namespace DotNetToolbox.Graph.Nodes;

public abstract class Node<TNode>(uint id, string label, IServiceProvider services)
    : INode
    where TNode : Node<TNode> {
    protected Node(uint id, IServiceProvider services)
        : this(id, typeof(TNode).Name, services) {
    }

    public uint Id { get; } = id;
    public string Label { get; } = IsNotNull(label);
    protected IServiceProvider Services { get; } = IsNotNull(services);

    public Result Validate(ISet<INode>? visited = null) {
        visited ??= new HashSet<INode>();
        return !visited.Add(this)
            ? Success()
            : IsValid(visited);
    }

    protected virtual Result IsValid(ISet<INode> visited) => Success();

    public async Task<INode?> Run(Context context, CancellationToken ct) {
        await UpdateState(context, ct);
        return await GetNext(context, ct);
    }

    protected abstract Task UpdateState(Context context, CancellationToken ct);
    protected abstract Task<INode?> GetNext(Context context, CancellationToken ct);

    public override int GetHashCode() => Id.GetHashCode();
}
