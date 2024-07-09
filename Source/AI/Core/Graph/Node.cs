namespace DotNetToolbox.AI.Graph;

public abstract class Node(string id, IEnumerable<INode?>? entries = null, IEnumerable<INode?>? exits = null)
    : INode {
    public HashSet<INode?> Entries { get; } = entries?.ToHashSet() ?? [];
    public HashSet<INode?> Exits { get; } = exits?.ToHashSet() ?? [];
    public string Id => id;
    public INode? Execute(Map state, INode? caller = null) {
        UpdateState(state, caller);
        return SelectExit(state, caller);
    }

    protected abstract void UpdateState(Map state, INode? caller = null);
    protected abstract INode? SelectExit(Map state, INode? caller = null);

    public override int GetHashCode() => id.GetHashCode();
}
