namespace DotNetToolbox.Graph.Nodes;

public static class Node {
    [return: NotNull]
    public static TNode Create<TNode>(IServiceProvider services, uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>
        => InstanceFactory.Create<TNode>(services, id, tag ?? string.Empty, label ?? string.Empty);
}

public abstract class Node<TNode>(uint id, string? tag = null, string? label = null)
    : INode
    where TNode : Node<TNode> {
    private readonly string? _tag = tag;
    private readonly string? _label = label;

    protected virtual string DefaultLabel { get; } = typeof(TNode).Name;

    public uint Id { get; } = id;
    public virtual string Tag => string.IsNullOrWhiteSpace(_tag) ? $"{Id}" : _tag;
    public string Label => string.IsNullOrWhiteSpace(_label) ? string.IsNullOrWhiteSpace(_tag) ? DefaultLabel : _tag : _label;
    public Token? Token { get; set; }
    public INode? Next { get; set; }

    public Result Validate(ISet<INode>? visited = null) {
        visited ??= new HashSet<INode>();
        return !visited.Add(this)
            ? Success()
            : IsValid(visited);
    }

    public abstract Result ConnectTo(INode? next);

    protected virtual Result IsValid(ISet<INode> visited) => Success();

    public async Task<INode?> Run(Context context, CancellationToken ct = default) {
        await UpdateState(context, ct);
        return await SelectPath(context, ct);
    }

    protected abstract Task UpdateState(Context context, CancellationToken ct = default);
    protected abstract Task<INode?> SelectPath(Context context, CancellationToken ct = default);

    public override int GetHashCode() => Id.GetHashCode();

    internal void MapTo(Token token, Dictionary<INode, Token> nodeMap)
        => nodeMap[this] = token;
}
