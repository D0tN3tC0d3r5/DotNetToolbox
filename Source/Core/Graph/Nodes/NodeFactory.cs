namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory(IServiceProvider services)
        : INodeFactory {
    public IConditionalNode CreateFork(uint id,
                                       Func<Context, bool> predicate,
                                       WorkflowBuilder builder,
                                       Action<WorkflowBuilder> setTruePath,
                                       Action<WorkflowBuilder>? setFalsePath = null)
        => ConditionalNode.Create(id, null, predicate, builder, setTruePath, setFalsePath, services);
    public IConditionalNode CreateFork(uint id,
                                       string label,
                                       Func<Context, bool> predicate,
                                       WorkflowBuilder builder,
                                       Action<WorkflowBuilder> setTruePath,
                                       Action<WorkflowBuilder>? setFalsePath = null)
        => ConditionalNode.Create(id, label, predicate, builder, setTruePath, setFalsePath, services);
    public IConditionalNode CreateFork<TNode>(uint id, string label)
        where TNode : ConditionalNode<TNode>
        => ConditionalNode.Create<TNode>(id, label, services);
    public IConditionalNode CreateFork<TNode>(uint id)
        where TNode : ConditionalNode<TNode>
        => ConditionalNode.Create<TNode>(id, services);

    public IBranchingNode CreateChoice(uint id,
                                       Func<Context, string> selectPath,
                                       WorkflowBuilder builder,
                                       Action<BranchesBuilder> setPaths)
        => BranchingNode.Create(id, null, selectPath, builder, setPaths, services);
    public IBranchingNode CreateChoice(uint id,
                                       string label,
                                       Func<Context, string> selectPath,
                                       WorkflowBuilder builder,
                                       Action<BranchesBuilder> setPaths)
        => BranchingNode.Create(id, label, selectPath, builder, setPaths, services);
    public IBranchingNode CreateChoice<TNode>(uint id, string label)
        where TNode : BranchingNode<TNode>
        => BranchingNode.Create<TNode>(id, label, services);
    public IBranchingNode CreateChoice<TNode>(uint id)
        where TNode : BranchingNode<TNode>
        => BranchingNode.Create<TNode>(id, services);

    public IActionNode CreateAction(uint id, string label, Action<Context> action)
        => ActionNode.Create(id, label, action, services);
    public IActionNode CreateAction(uint id, Action<Context> action)
        => ActionNode.Create(id, null, action, services);
    public IActionNode CreateAction<TNode>(uint id, string label)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(id, label, services);
    public IActionNode CreateAction<TNode>(uint id)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(id, services);

    public ITerminationNode CreateStop(uint id, string label)
        => TerminalNode.Create(id, label, services);
    public ITerminationNode CreateStop(uint id)
        => TerminalNode.Create(id, null, services);
}
