//namespace DotNetToolbox.Graph;

//public class GraphBuilder
//    : IGraphBuilder {
//    private readonly HashSet<INode> _nodes;
//    private readonly INode _startingNode;
//    private readonly INode _currentNode;

//    private GraphBuilder(INode startingNode) {
//        _currentNode = _startingNode = startingNode;
//        _nodes = [startingNode];
//    }

//    //public IGraphBuilder AddNode(INode node) {
//    //    _nodes.Add(node);
//    //    return this;
//    //}

//    public static IGraphBuilder From(INode startingNode)
//        => new GraphBuilder(startingNode);

//    public IGraphBuilder Switch(string id,
//                            Func<Context, bool> predicate,
//                            Action<IGraphBuilder> buildTruePath,
//                            Action<IGraphBuilder> buildFalsePath) {
//        IsNotNullOrWhiteSpace(id);
//        IsNotNull(predicate);
//        IsNotNull(buildTruePath);
//        IsNotNull(buildFalsePath);
//        var node = new IfNode(id);
//        var truePathBuilder = new GraphBuilder(node);
//        buildTruePath(truePathBuilder);
//        node.SetTruePath(truePathBuilder.Build())
//        var falsePathBuilder = new GraphBuilder(_currentNode);
//        _currentNode.AddExit(node);
//        return this;
//    }

//    //public IGraphBuilder AddEdgeTo(INode from, INode to) {
//    //    if (from is EndNode)
//    //        throw new InvalidOperationException("Cannot add an edge to an end node.");
//    //    RegisterEdge(from, to);
//    //    return this;
//    //}

//    //public IGraphBuilder AddEndTo(INode from, string id = "-1") {
//    //    var end = _nodes.FirstOrDefault(n => n.Id == id);
//    //    if (end is not null && end is not EndNode)
//    //        throw new InvalidOperationException("There is already another non-end node with same id.");
//    //    end ??= new EndNode(id);
//    //    RegisterEdge(from, end);
//    //    return this;
//    //}

//    public IGraph Build() {
//        var validationResult = Validate();
//        if (validationResult.IsInvalid)
//            throw new ValidationException(validationResult.Errors);
//        return new Graph(_startingNode);
//    }

//    private Result Validate() {
//        var validatedNodes = new HashSet<INode>();
//        var result = _startingNode.Validate(validatedNodes);
//        if (validatedNodes.Count != _nodes.Count)
//            result += Invalid("Graph has unreachable nodes.");
//        return result;
//    }

//    //private void RegisterEdge(INode from, INode to, object? metadata = null) {
//    //    RegisterNode(from);
//    //    RegisterNode(to);
//    //    from.AddExit(metadata, to);
//    //}

//    private void RegisterNode(INode node)
//        => _nodes.Add(node);
//}
