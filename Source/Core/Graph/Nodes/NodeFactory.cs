namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory(IServiceProvider services)
        : INodeFactory {
    public IConditionalNode CreateFork<TNode>(uint id)
        where TNode : ConditionalNode<TNode>
        => ConditionalNode.Create<TNode>(id, services);
    public IConditionalNode CreateFork<TNode>(uint id, string label)
        where TNode : ConditionalNode<TNode>
        => ConditionalNode.Create<TNode>(id, label, services);
    public IConditionalNode CreateFork(uint id,
                                       Func<Context, bool> predicate,
                                       WorkflowBuilder builder,
                                       Action<WorkflowBuilder> setTruePath,
                                       Action<WorkflowBuilder>? setFalsePath = null) {
        var node = new ConditionalNode(id, predicate, services);
        SetForkPaths(builder, node, setTruePath, setFalsePath);
        return node;
    }

    public IConditionalNode CreateFork(uint id,
                                       string label,
                                       Func<Context, bool> predicate,
                                       WorkflowBuilder builder,
                                       Action<WorkflowBuilder> setTruePath,
                                       Action<WorkflowBuilder>? setFalsePath = null) {
        var node = new ConditionalNode(id, label, predicate, services);
        SetForkPaths(builder, node, setTruePath, setFalsePath);
        return node;
    }

    private void SetForkPaths(WorkflowBuilder builder,
                              ConditionalNode node,
                              Action<WorkflowBuilder> setTruePath,
                              Action<WorkflowBuilder>? setFalsePath) {
        builder.Nodes.Add(node);
        var trueBuilder = new WorkflowBuilder(builder.Id, builder.Nodes, services);
        setTruePath(trueBuilder);
        node.IsTrue = trueBuilder.Start;
        if (setFalsePath == null)
            return;

        var falseBuilder = new WorkflowBuilder(builder.Id, builder.Nodes, services);
        setFalsePath(falseBuilder);
        node.IsFalse = falseBuilder.Start;
    }

    public IBranchingNode CreateChoice<TNode>(uint id)
        where TNode : BranchingNode<TNode>
        => BranchingNode.Create<TNode>(id, services);
    public IBranchingNode CreateChoice<TNode>(uint id, string label)
        where TNode : BranchingNode<TNode>
        => BranchingNode.Create<TNode>(id, label, services);
    public IBranchingNode CreateChoice(uint id,
                                       Func<Context, string> selectPath,
                                       WorkflowBuilder builder,
                                       Action<BranchesBuilder> setPaths) {
        var node = new BranchingNode(id, selectPath, services);
        builder.Nodes.Add(node);
        var branchesBuilder = new BranchesBuilder(builder, node, services);
        setPaths(branchesBuilder);
        return node;
    }
    public IBranchingNode CreateChoice(uint id,
                                       string label,
                                       Func<Context, string> selectPath,
                                       WorkflowBuilder builder,
                                       Action<BranchesBuilder> setPaths) {
        var node = new BranchingNode(id, label, selectPath, services);
        SetChoicePaths(builder, node, setPaths);
        return node;
    }

    private void SetChoicePaths(WorkflowBuilder builder,
                                IBranchingNode node,
                                Action<BranchesBuilder> setPaths) {
        builder.Nodes.Add(node);
        var branchesBuilder = new BranchesBuilder(builder, node, services);
        setPaths(branchesBuilder);
    }

    public IActionNode CreateAction<TNode>(uint id)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(id, services);
    public IActionNode CreateAction<TNode>(uint id, string label)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(id, label, services);
    public IActionNode CreateAction(uint id, Action<Context> action)
        => new ActionNode(id, action, services);
    public IActionNode CreateAction(uint id, string label, Action<Context> action)
        => new ActionNode(id, label, action, services);

    public ITerminationNode CreateStop(uint id)
        => TerminalNode.Create(id, services);
    public ITerminationNode CreateStop(uint id, string label)
        => TerminalNode.Create(id, label, services);
}
