namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory(IServiceProvider services)
        : INodeFactory {
    public TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(id, services, tag, label);

    public IConditionalNode CreateFork(uint id,
                                       Func<Context, bool> predicate,
                                       Action<IConditionalNode, ConditionalNodeBuilder> setPaths,
                                       string? tag = null,
                                       string? label = null) {
        var node = new ConditionalNode(id, services, predicate, tag, label);
        var conditionsBuilder = new ConditionalNodeBuilder(services);
        setPaths(node, conditionsBuilder);
        conditionsBuilder.Configure(node);
        return node;
    }

    public IBranchingNode CreateChoice(uint id,
                                       Func<Context, string> selectPath,
                                       Action<IBranchingNode, BranchingNodeBuilder> setPaths,
                                       string? tag = null,
                                       string? label = null) {
        var node = new BranchingNode(id, services, selectPath, tag, label);
        var branchesBuilder = new BranchingNodeBuilder(services);
        setPaths(node, branchesBuilder);
        branchesBuilder.Configure(node);
        return node;
    }

    public IActionNode CreateAction(uint id, Action<Context> action, string? tag = null, string? label = null)
        => new ActionNode(id, services, action, tag, label);

    public IJumpNode CreateJump(uint id, string targetTag, string? label = null)
        => new JumpNode(id, services, targetTag, label);

    public ITerminationNode CreateStop(uint id, int exitCode = 0, string? tag = null, string? label = null)
        => new TerminalNode(id, services, exitCode, tag, label);
}
