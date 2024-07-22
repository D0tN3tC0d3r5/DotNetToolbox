namespace DotNetToolbox.Graph.Nodes;

public abstract class Node
    : INode {
    protected static INodeFactory Factory { get; } = new NodeFactory();

    protected Node(string id) {
        Id = IsNotNull(id);
    }

    protected Node(IGuidProvider? guid = null) {
        guid ??= new GuidProvider();
        Id = guid.AsSortable.Create().ToString();
    }

    public string Id { get; }
    protected List<INode?> Paths { get; init; } = [];

    public virtual Result Validate(ICollection<INode>? validatedNodes = null) {
        validatedNodes ??= new HashSet<INode>();
        if (validatedNodes.Contains(this))
            return Success();

        var result = IsValid();
        if (result.IsInvalid)
            return result;

        validatedNodes.Add(this);
        return Paths.Where(node => node is not null)
                    .Aggregate(result, (current, node) => current + node!.Validate(validatedNodes));
    }

    protected virtual Result IsValid() => Success();

    public virtual INode? Run(Context context) {
        OnEntry(context);
        UpdateState(context);
        var exitPath = GetNext(context);
        OnExit(context);
        return exitPath;
    }

    protected virtual void OnEntry(Context state) { }
    protected abstract void UpdateState(Context context);
    protected abstract INode? GetNext(Context context);
    protected virtual void OnExit(Context state) { }

    public override int GetHashCode() => Id.GetHashCode();
}
