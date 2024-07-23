namespace DotNetToolbox.Graph.Nodes;

internal sealed class NodeFactory
    : INodeFactory {
    public IConditionalNode CreateFork(string label, Func<Context, bool> predicate, Action<WorkflowBuilder> setTruePath, Action<WorkflowBuilder>? setFalsePath = null, HashSet<INode?>? nodes = null)
        => ConditionalNode.Create(label, predicate, setTruePath, setFalsePath, nodes);
    public IConditionalNode CreateFork(Func<Context, bool> predicate, Action<WorkflowBuilder> setTruePath, Action<WorkflowBuilder>? setFalsePath = null, HashSet<INode?>? nodes = null)
        => CreateFork(null!, predicate, setTruePath, setFalsePath, nodes);
    public IConditionalNode CreateFork<TNode>(string? label = null)
        where TNode : ConditionalNode<TNode>
        => ConditionalNode.Create<TNode>(label);

    public IBranchingNode CreateChoice(string label, Func<Context, string> selectPath, Action<BranchesBuilder> setPaths, HashSet<INode?>? nodes = null)
        => BranchingNode.Create(label, selectPath, setPaths, nodes);
    public IBranchingNode CreateChoice(Func<Context, string> selectPath, Action<BranchesBuilder> setPaths, HashSet<INode?>? nodes = null)
        => CreateChoice(null!, selectPath, setPaths, nodes);
    public IBranchingNode CreateChoice<TNode>(string? label = null)
        where TNode : BranchingNode<TNode>
        => BranchingNode.Create<TNode>(label);

    public IActionNode CreateAction(string label, Action<Context> action, IPolicy? policy = null)
        => ActionNode.Create(label, action, policy);
    public IActionNode CreateAction(Action<Context> action, IPolicy? policy = null)
        => CreateAction(null!, action, policy);
    public IActionNode CreateAction<TNode>(string label, IPolicy? policy = null)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(label, policy);
    public IActionNode CreateAction<TNode>(IPolicy? policy = null)
        where TNode : ActionNode<TNode>
        => ActionNode.Create<TNode>(null, policy);

    public IStartingNode CreateStart(string? label = null)
        => StartingNode.Create(label);
    public IStartingNode CreateStart<TNode>(string? label = null)
        where TNode : StartingNode<TNode>
        => StartingNode.Create<TNode>(label);

    public ITerminationNode End(string? label = null)
        => TerminationNode.Create(label);
    public ITerminationNode End<TNode>(string? label = null)
        where TNode : TerminationNode<TNode>
        => TerminationNode.Create<TNode>(label);
}
