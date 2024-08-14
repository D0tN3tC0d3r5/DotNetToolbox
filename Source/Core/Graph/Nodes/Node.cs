namespace DotNetToolbox.Graph.Nodes;

public static class Node {
    public static TNode Create<TNode>(uint id,
                                      IServiceProvider services,
                                      string? tag = null,
                                      string? label = null)
        where TNode : Node<TNode>
        => InstanceFactory.Create<TNode>(id, services, tag, label);
}

public abstract class Node<TNode>(uint id, IServiceProvider services, string? tag, string? label)
    : INode
    where TNode : Node<TNode> {
    private readonly string? _tag = tag;
    private readonly string? _label = label;

    protected virtual string DefaultLabel { get; } = typeof(TNode).Name;

    public uint Id { get; } = id;
    public string Tag => _tag ?? $"{Id}";
    public string Label => _label ?? _tag ?? DefaultLabel;
    public INode? Next { get; set; }
    protected IServiceProvider Services { get; } = IsNotNull(services);

    public Result Validate(ISet<INode>? visited = null) {
        visited ??= new HashSet<INode>();
        return !visited.Add(this)
            ? Success()
            : IsValid(visited);
    }

    public abstract void ConnectTo(INode? next);

    protected virtual Result IsValid(ISet<INode> visited) => Success();

    public async Task<INode?> Run(Context context, CancellationToken ct) {
        await UpdateState(context, ct);
        return await SelectPath(context, ct);
    }

    protected abstract Task UpdateState(Context context, CancellationToken ct);
    protected abstract Task<INode?> SelectPath(Context context, CancellationToken ct);

    public override int GetHashCode() => Id.GetHashCode();
}
