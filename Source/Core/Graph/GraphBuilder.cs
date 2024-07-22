namespace DotNetToolbox.Graph;

public class GraphBuilder {
    private readonly Dictionary<string, INode> _nodeMap = new();
    private readonly INodeFactory _nodeFactory;
    private INode _currentNode;

    public GraphBuilder(INodeFactory? nodeFactory = null, IGuidProvider? guid = null) {
        _nodeFactory = nodeFactory ?? new NodeFactory(guid);
        Path = _nodeFactory.Start;
        _currentNode = Path;
    }

    internal INode Path { get; }

    public GraphBuilder Do(string label, Action<Context> action, IPolicy? policy = null) {
        _nodeMap[label] = BuildActionNode(action, policy);
        return this;
    }

    public GraphBuilder Do(Action<Context> action, IPolicy? policy = null) {
        BuildActionNode(action, policy);
        return this;
    }

    private INode BuildActionNode(Action<Context> action, IPolicy? policy) {
        var newNode = _nodeFactory.Do(action, policy: policy);
        ConnectNode(newNode);
        return newNode;
    }

    public GraphBuilder Do<TAction>(string label)
        where TAction : ActionNode<TAction> {
        _nodeMap[label] = BuildActionNode<TAction>();
        return this;
    }

    public GraphBuilder Do<TAction>()
        where TAction : ActionNode<TAction> {
        BuildActionNode<TAction>();
        return this;
    }

    private INode BuildActionNode<TAction>()
        where TAction : ActionNode<TAction> {
        var newNode = _nodeFactory.Do<TAction>();
        ConnectNode(newNode);
        return newNode;
    }

    public GraphBuilder If(string label,
                           Func<Context, bool> predicate,
                           Action<GraphBuilder> setTrueBranch,
                           Action<GraphBuilder>? setFalseBranch = null) {
        _nodeMap[label] = BuildConditionalNode(predicate, setTrueBranch, setFalseBranch);
        return this;
    }

    public GraphBuilder If(Func<Context, bool> predicate,
                           Action<GraphBuilder> setTrueBranch,
                           Action<GraphBuilder>? setFalseBranch = null) {
        BuildConditionalNode(predicate, setTrueBranch, setFalseBranch);
        return this;
    }

    private INode BuildConditionalNode(Func<Context, bool> predicate, Action<GraphBuilder> setTrueBranch, Action<GraphBuilder>? setFalseBranch) {
        var ifNode = _nodeFactory.If(predicate, _nodeFactory.Void);

        var trueBuilder = new GraphBuilder(_nodeFactory);
        setTrueBranch(trueBuilder);
        ifNode.True = trueBuilder.Path;

        if (setFalseBranch != null) {
            var falseBuilder = new GraphBuilder(_nodeFactory);
            setFalseBranch(falseBuilder);
            ifNode.False = falseBuilder.Path;
        }

        ConnectNode(ifNode);
        return ifNode;
    }

    public GraphBuilder Select<TKey>(string label,
                                     Func<Context, TKey> selector,
                                     Action<MapOptions<TKey>> options)
        where TKey : notnull {
        _nodeMap[label] = BuildBranchingNode(selector, options);
        return this;
    }

    public GraphBuilder Select<TKey>(Func<Context, TKey> selector,
                                     Action<MapOptions<TKey>> options)
        where TKey : notnull {
        BuildBranchingNode(selector, options);
        return this;
    }

    private INode BuildBranchingNode<TKey>(Func<Context, TKey> selector, Action<MapOptions<TKey>> options)
        where TKey : notnull {
        var mapOptions = new MapOptions<TKey>(_nodeFactory);
        options(mapOptions);

        var mapNode = _nodeFactory.Select(selector, mapOptions.Branches);

        ConnectNode(mapNode);
        return mapNode;
    }

    public GraphBuilder JumpTo(string label) {
        if (!_nodeMap.TryGetValue(label, out var targetNode))
            throw new InvalidOperationException($"Label '{label}' not found.");

        ConnectNode(targetNode);
        return this;
    }

    private void ConnectNode(INode newNode) {
        if (_currentNode is IActionNode actionNode)
            actionNode.Next = newNode;

        _currentNode = newNode;
    }

    public IRunner BuildRunner()
        => new Runner(Path);

    public class MapOptions<TKey>(INodeFactory nodeFactory)
        where TKey : notnull {
        internal Dictionary<TKey, INode?> Branches { get; } = new();

        public MapOptions<TKey> Branch(TKey key, Action<GraphBuilder> branch) {
            var branchBuilder = new GraphBuilder(nodeFactory);
            branch(branchBuilder);
            Branches[key] = branchBuilder.Path;
            return this;
        }
    }
}
