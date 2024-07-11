namespace DotNetToolbox.Graph.PathBuilder;

public class PathBuilder
    : IPathBuilder {
    private readonly INode _caller;
    private readonly string _id;

    private PathBuilder(INode caller, string id) {
        _caller = caller;
        _id = id;
    }

    //public IGraphBuilder AddNode(INode node) {
    //    _nodes.Add(node);
    //    return this;
    //}

    public static IPathBuilder From(INode caller, string? id = null)
        => new PathBuilder(caller, id ?? Guid.NewGuid().ToString());

    public IIfBuilder If(Func<Map, bool> predicate) {
        IsNotNull(predicate);
        var node = new IfThenElseNode(_id, predicate: predicate);
        return new IfBuilder(node);
    }

    //public IGraphBuilder AddEdgeTo(INode from, INode to) {
    //    if (from is EndNode)
    //        throw new InvalidOperationException("Cannot add an edge to an end node.");
    //    RegisterEdge(from, to);
    //    return this;
    //}

    //public IGraphBuilder AddEndTo(INode from, string id = "-1") {
    //    var end = _nodes.FirstOrDefault(n => n.Id == id);
    //    if (end is not null && end is not EndNode)
    //        throw new InvalidOperationException("There is already another non-end node with same id.");
    //    end ??= new EndNode(id);
    //    RegisterEdge(from, end);
    //    return this;
    //}

    public IGraph Build() {
        var validationResult = Validate();
        if (validationResult.IsInvalid)
            throw new ValidationException(validationResult.Errors);
        return new Graph(_startingNode);
    }

    private Result Validate() {
        var validatedNodes = new HashSet<INode>();
        var result = _startingNode.Validate(validatedNodes);
        if (validatedNodes.Count != _nodes.Count)
            result += Invalid("Graph has unreachable nodes.");
        return result;
    }

    //private void RegisterEdge(INode from, INode to, object? metadata = null) {
    //    RegisterNode(from);
    //    RegisterNode(to);
    //    from.AddExit(metadata, to);
    //}

    private void RegisterNode(INode node)
        => _nodes.Add(node);
}
