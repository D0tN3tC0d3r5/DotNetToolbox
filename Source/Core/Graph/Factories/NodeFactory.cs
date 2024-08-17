namespace DotNetToolbox.Graph.Factories;

internal sealed class NodeFactory(IServiceProvider services, string? id = null)
    : INodeFactory {
    private readonly string _builderId = id ?? GuidProvider.Default.Create().ToString()!;

    public TNode Create<TNode>(uint id, string? tag = null, string? label = null)
        where TNode : Node<TNode>
        => Node.Create<TNode>(services, id, tag, label);

    internal Result<INode> GetCreateForkResult(uint id,
                                               Func<Context, bool> predicate,
                                               Action<IIfNodeBuilder> setPaths,
                                               string? tag = null,
                                               string? label = null) {
        var node = new IfNode(id, predicate, tag, label);
        var conditionsBuilder = new WorkflowBuilder(services, _builderId, node);
        setPaths(conditionsBuilder);
        return conditionsBuilder.Build()!;
    }

    internal Result<INode> GetCreateChoiceResult(uint id,
                                                 Func<Context, string> selectPath,
                                                 Action<ICaseNodeBuilder> setPaths,
                                                 string? tag = null,
                                                 string? label = null) {
        var node = new CaseNode(id, selectPath, tag, label);
        var branchesBuilder = new WorkflowBuilder(services, _builderId, node);
        setPaths(branchesBuilder);
        return branchesBuilder.Build()!;
    }

    public INode CreateFork(uint id,
                            Func<Context, bool> predicate,
                            Action<IIfNodeBuilder> setPaths,
                            string? tag = null,
                            string? label = null) {
        var result = GetCreateForkResult(id, predicate, setPaths, tag, label);
        return result.IsSuccess
            ? result.Value!
            : throw new ValidationException(result.Errors);
    }

    public INode CreateChoice(uint id,
                              Func<Context, string> selectPath,
                              Action<ICaseNodeBuilder> setPaths,
                              string? tag = null,
                              string? label = null) {
        var result = GetCreateChoiceResult(id, selectPath, setPaths, tag, label);
        return result.IsSuccess
            ? result.Value!
            : throw new ValidationException(result.Errors);
    }

    public INode CreateAction(uint id,
                              Action<Context> action,
                              string? tag = null,
                              string? label = null) {
        var policy = services.GetService<IPolicy>() ?? Policy.Default;
        return new ActionNode(id, action, tag, label, policy);
    }

    public INode CreateJump(uint id,
                            string targetTag,
                            string? tag = null,
                            string? label = null)
        => new JumpNode(id, targetTag, label);

    public INode CreateExit(uint id,
                            int exitCode = 0,
                            string? tag = null,
                            string? label = null)
        => new ExitNode(id, exitCode, tag, label);
}
