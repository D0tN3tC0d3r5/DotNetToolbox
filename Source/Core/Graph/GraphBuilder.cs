namespace DotNetToolbox.Graph;
public class GraphBuilder(INode startingNode)
    : IGraphBuilder {
    private readonly HashSet<INode> _nodes = [startingNode];

    public IGraphBuilder AddNode(INode node) {
        _nodes.Add(node);
        return this;
    }

    public IGraphBuilder AddEdgeTo(INode from, INode to) {
        if (from is EndNode)
            throw new InvalidOperationException("Cannot add an edge to an end node.");
        RegisterEdge(from, to);
        return this;
    }

    public IGraphBuilder AddEndTo(INode from, string id = "-1") {
        var end = _nodes.FirstOrDefault(n => n.Id == id);
        if (end is not null && end is not EndNode)
            throw new InvalidOperationException("There is already another non-end node with same id.");
        end ??= new EndNode(id);
        RegisterEdge(from, end);
        return this;
    }

    public IGraph Build()
        => VerifyNodes()
            ? (IGraph)new Graph(startingNode)
            : throw new InvalidOperationException("Graph is not valid.");

    private bool VerifyNode(ISet<INode> visited, INode? node = null) {
        node ??= startingNode;
        return !visited.Add(node)
            || node is EndNode
            || (node.Exits.Count != 0 && node.Exits.All(e => VerifyNode(visited, e)));
    }

    private void RegisterEdge(INode from, INode to) {
        RegisterNode(from);
        RegisterNode(to);
        from.Exits.Add(to);
        to.Entries.Add(from);
    }

    private void RegisterNode(INode node)
        => _nodes.Add(node);

    private bool VerifyNodes() {
        var visited = new HashSet<INode>();
        return VerifyNode(visited) && visited.Count == _nodes.Count;
    }
}
