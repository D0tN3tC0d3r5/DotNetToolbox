namespace DotNetToolbox.Graph.Nodes;

public abstract class Node(INodeFactory? factory = null)
    : Node<int>(factory);

public abstract class Node<TKey>
    : INode<TKey>
    where TKey : notnull {
    protected Node(INodeFactory? factory = null) {
        Factory = factory ?? new NodeFactory();
        Id = Factory.GenerateId();
    }

    public string Id { get; }
    public INode? Next { get; set; }

    public Dictionary<TKey, INode?> Branches { get; } = [];

    protected INodeFactory Factory { get; }

    public virtual Result Validate(ICollection<INode>? validatedNodes = null) {
        validatedNodes ??= new HashSet<INode>();
        if (validatedNodes.Contains(this))
            return Success();

        var result = IsValid();
        if (result.IsInvalid)
            return result;

        validatedNodes.Add(this);
        return Branches.Values.Where(node => node is not null).Distinct()
                    .Aggregate(result, (current, node) => current + node!.Validate(validatedNodes));
    }

    protected virtual Result IsValid() => Success();

    public INode? Run(Context context) {
        UpdateState(context);
        return GetNext(context);
    }

    protected abstract void UpdateState(Context context);
    protected abstract INode? GetNext(Context context);

    public override int GetHashCode() => Id.GetHashCode();
}
