namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services, string? id = null, Dictionary<INode, Token>? nodeMap = null)
    : INodeFactory {
    private readonly string _idScope = id ?? GuidProvider.Default.ToString()!;
    private readonly Dictionary<INode, Token> _nodeMap = nodeMap ?? [];

    public TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(id, services, tag, label);

    public IIfNode CreateFork(uint id,
                              Func<Context, bool> predicate,
                              Action<IIfNodeBuilder> setPaths,
                              string? tag = null,
                              string? label = null) {
        var node = new IfNode(id, services, predicate, tag, label);
        var conditionsBuilder = new WorkflowBuilder(services, _idScope, node, _nodeMap);
        setPaths(conditionsBuilder);
        return conditionsBuilder.Build<IIfNode>()!;
    }

    public ICaseNode CreateChoice(uint id,
                                  Func<Context, string> selectPath,
                                  Action<ICaseNodeBuilder> setPaths,
                                  string? tag = null,
                                  string? label = null) {
        var node = new CaseNode(id, services, selectPath, tag, label);
        var branchesBuilder = new WorkflowBuilder(services, _idScope, node, _nodeMap);
        setPaths(branchesBuilder);
        return branchesBuilder.Build<ICaseNode>()!;
    }

    public IActionNode CreateAction(uint id,
                                    Action<Context> action,
                                    string? tag = null,
                                    string? label = null)
        => new ActionNode(id, services, action, tag, label);

    public IJumpNode CreateJump(uint id,
                                string targetTag,
                                string? tag = null,
                                string? label = null)
        => new JumpNode(id, services, targetTag, label);

    public IExitNode CreateExit(uint id,
                               int exitCode = 0,
                               string? tag = null,
                               string? label = null)
        => new ExitNode(id, services, exitCode, tag, label);
}
