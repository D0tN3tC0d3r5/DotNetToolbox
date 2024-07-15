namespace DotNetToolbox.Graph.Nodes;

public abstract class Node(string id)
    : INode {
    public string Id => IsNotNull(id);
    protected HashSet<INode?> Paths { get; init; } = [];

    public virtual Result Validate(ICollection<INode> validatedNodes) {
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
    protected virtual void UpdateState(Context state) { }
    protected virtual INode? GetNext(Context state) => null;
    protected virtual void OnExit(Context state) { }

    public override int GetHashCode() => id.GetHashCode();
}
