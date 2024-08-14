using DotNetToolbox.Graph.Builders;

namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services)
        : INodeFactory {
    public TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(id, services, tag, label);

    public IIfNode CreateFork(uint id,
                                       Func<Context, bool> predicate,
                                       Action<IfNodeBuilder> setPaths,
                                       string? tag = null,
                                       string? label = null) {
        var node = new ConditionalNode(id, services, predicate, tag, label);
        var conditionsBuilder = new IfNodeBuilder(services, node);
        setPaths(conditionsBuilder);
        return conditionsBuilder.Build();
    }

    public ICaseNode CreateChoice(uint id,
                                       Func<Context, string> selectPath,
                                       Action<CaseNodeBuilder> setPaths,
                                       string? tag = null,
                                       string? label = null) {
        var node = new BranchingNode(id, services, selectPath, tag, label);
        var branchesBuilder = new CaseNodeBuilder(services, node);
        setPaths(branchesBuilder);
        return branchesBuilder.Build();
    }

    public IActionNode CreateAction(uint id, Action<Context> action, string? tag = null, string? label = null)
        => new ActionNode(id, services, action, tag, label);

    public IJumpNode CreateJump(uint id, string targetTag, string? label = null)
        => new JumpNode(id, services, targetTag, label);

    public IEndNode CreateStop(uint id, int exitCode = 0, string? tag = null, string? label = null)
        => new EndNode(id, services, exitCode, tag, label);
}
