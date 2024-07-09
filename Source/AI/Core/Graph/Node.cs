namespace DotNetToolbox.AI.Graph;

public abstract class Node(uint id, INode? caller = null, IEnumerable<INode?>? branches = null)
    : INode {
    public INode? Caller { get; } = caller;
    public HashSet<INode?>? Branches { get; } = branches?.ToHashSet() ?? [];
    public uint Id => id;
    public INode? Execute(Map state) {
        UpdateState(state);
        return SelectBranch(state);
    }

    protected abstract void UpdateState(Map state);
    protected abstract INode? SelectBranch(Map state);
}
