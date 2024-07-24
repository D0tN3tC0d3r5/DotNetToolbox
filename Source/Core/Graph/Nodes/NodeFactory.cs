namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory
    : INodeFactory {
    public IConditionalNode CreateFork(uint id,
                                       Func<Context, bool> predicate,
                                       WorkflowBuilder builder,
                                       Action<WorkflowBuilder> setTruePath,
                                       Action<WorkflowBuilder>? setFalsePath = null)
        => CreateFork(id, null!, predicate, builder, setTruePath, setFalsePath);
    public IConditionalNode CreateFork(uint id,
                                       string label,
                                       Func<Context, bool> predicate,
                                       WorkflowBuilder builder,
                                       Action<WorkflowBuilder> setTruePath,
                                       Action<WorkflowBuilder>? setFalsePath = null)
        => ConditionalNode.Create(id, label, predicate, builder, setTruePath, setFalsePath);
    public IConditionalNode CreateFork<TNode>(uint id,
                                              string? label = null)
        where TNode : ConditionalNode<TNode>
        => ConditionalNode.Create<TNode>(id, label);

    public IBranchingNode CreateChoice(uint id,
                                       Func<Context, string> selectPath,
                                       WorkflowBuilder builder,
                                       Action<BranchesBuilder> setPaths)
        => CreateChoice(id, null!, selectPath, builder, setPaths);
    public IBranchingNode CreateChoice(uint id,
                                       string label,
                                       Func<Context, string> selectPath,
                                       WorkflowBuilder builder,
                                       Action<BranchesBuilder> setPaths)
        => BranchingNode.Create(id, label, selectPath, builder, setPaths);
    public IBranchingNode CreateChoice<TNode>(uint id, string? label = null)
        where TNode : BranchingNode<TNode>
        => BranchingNode.Create<TNode>(id, label);

    public IActionNode CreateAction(uint id,
                                    string label,
                                    Action<Context> action,
                                    IPolicy? policy = null)
        => ActionNode.Create(id, label, action, policy);
    public IActionNode CreateAction(uint id,
                                    Action<Context> action,
                                    IPolicy? policy = null)
        => CreateAction(id, null!, action, policy);
    public IActionNode CreateAction<TNode>(uint id,
                                           string label,
                                           IPolicy? policy = null)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(id, label, policy);
    public IActionNode CreateAction<TNode>(uint id,
                                           IPolicy? policy = null)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(id, null, policy);

    public ITerminationNode CreateStop(uint id,
                                       string? label = null)
        => TerminalNode.Create(id, label);
}
