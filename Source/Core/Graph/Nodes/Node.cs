namespace DotNetToolbox.Graph.Nodes;

public abstract class Node(string id)
    : INode {
    public string Id => IsNotNull(id);
    public Map State { get; set; } = [];
    protected HashSet<INode> Exits { get; init; } = [];

    public virtual Result Validate(ICollection<INode> validatedNodes) {
        if (validatedNodes.Contains(this))
            return Success();
        var result = IsValid();
        if (result.IsInvalid)
            return result;
        validatedNodes.Add(this);
        return Exits.Aggregate(result, (current, node) => current + node.Validate(validatedNodes));
    }

    protected virtual Result IsValid() => Success();

    public INode? Run(Map state) {
        OnEntry(state);
        UpdateState(state);
        var exit = GetNext(state);
        OnExit(state);
        return exit;
    }

    protected virtual Result OnEntry(Map state) => Success();
    protected virtual void UpdateState(Map state) { }
    protected virtual INode? GetNext(Map state) => null;
    protected virtual Result OnExit(Map state) => Success();

    public override int GetHashCode() => id.GetHashCode();
}
