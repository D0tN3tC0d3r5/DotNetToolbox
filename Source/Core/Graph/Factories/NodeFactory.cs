namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services, string? nodeSequenceKey = null, Dictionary<string, INode>? tagMap = null)
        : INodeFactory {
    private readonly string _nodeSequenceKey = nodeSequenceKey ?? nameof(NodeFactory);
    private readonly Dictionary<string, INode> _tagMap = tagMap ?? [];

    public TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(id, services, tag, label);

    public IIfNode CreateFork(uint id,
                              Func<Context, bool> predicate,
                              Action<IfNodeBuilder> setPaths,
                              string? tag = null,
                              string? label = null) {
        var node = new IfNode(id, services, predicate, tag, label);
        var conditionsBuilder = new IfNodeBuilder(services, node, _nodeSequenceKey, _tagMap);
        setPaths(conditionsBuilder);
        return conditionsBuilder.Build();
    }

    public ICaseNode CreateChoice(uint id,
                                       Func<Context, string> selectPath,
                                       Action<CaseNodeBuilder> setPaths,
                                       string? tag = null,
                                       string? label = null) {
        var node = new CaseNode(id, services, selectPath, tag, label);
        var branchesBuilder = new CaseNodeBuilder(services, node, _nodeSequenceKey, _tagMap);
        setPaths(branchesBuilder);
        return branchesBuilder.Build();
    }

    public IActionNode CreateAction(uint id, Action<Context> action, string? tag = null, string? label = null)
        => new ActionNode(id, services, action, tag, label);

    public IJumpNode CreateJump(uint id, string targetTag, string? label = null)
        => new JumpNode(id, services, targetTag, label);

    public IEndNode CreateExit(uint id, int exitCode = 0, string? tag = null, string? label = null)
        => new ExitNode(id, services, exitCode, tag, label);
}
