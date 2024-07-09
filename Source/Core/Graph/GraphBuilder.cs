namespace DotNetToolbox.AI.Graph;
public class GraphBuilder(INode startingNode) : IGraphBuilder {
    private readonly HashSet<INode> _nodes = [startingNode];

    public IGraphBuilder AddEdge(INode from, INode to) {
        if (from is EndNode || to is EndNode)
            throw new InvalidOperationException("Use AddEnding to connect an ending node.");
        _nodes.Add(from);
        _nodes.Add(to);
        from.Exits.Add(to);
        to.Entries.Add(from);
        return this;
    }

    public IGraphBuilder AddEnding(INode from, string id) {
        var node = _nodes.FirstOrDefault(n => n.Id == id);
        if (node is not null && node is not EndNode)
            throw new InvalidOperationException("There is already another node with same id.");
        if (node is null)
            node = new EndNode(id, [from]);
        else
            node.Entries.Add(from);
        return this;
    }

    public IGraphRunner Build() => VerifyNodes() ? (IGraphRunner)new GraphRunner(startingNode) : throw new InvalidOperationException("Graph is not valid.");

    private bool VerifyNode(ISet<INode> visited, INode? node = null) {
        node ??= startingNode;
        if (!visited.Add(node))
            return true;
        return node is EndNode ? true : node.Exits.Count != 0 && node.Exits.All(e => VerifyNode(visited, e));
    }

    private bool VerifyNodes() {
        var visited = new HashSet<INode>();
        return VerifyNode(visited) && visited.Count == _nodes.Count;
    }
}
