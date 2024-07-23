namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    IConditionalNode CreateFork(Func<Context, bool> predicate,
                                Action<WorkflowBuilder> setTrueBranch,
                                Action<WorkflowBuilder>? setFalseBranch = null,
                                HashSet<INode?>? nodes = null);
    IConditionalNode CreateFork(string label,
                                Func<Context, bool> predicate,
                                Action<WorkflowBuilder> setTrueBranch,
                                Action<WorkflowBuilder>? setFalseBranch = null,
                                HashSet<INode?>? nodes = null);
    IConditionalNode CreateFork<TNode>(string? label = null)
        where TNode : ConditionalNode<TNode>;

    IBranchingNode CreateChoice(Func<Context, string> selectPath,
                                Action<BranchesBuilder> setPaths,
                                HashSet<INode?>? nodes = null);
    IBranchingNode CreateChoice(string label,
                                Func<Context, string> selectPath,
                                Action<BranchesBuilder> setPaths,
                                HashSet<INode?>? nodes = null);
    IBranchingNode CreateChoice<TNode>(string? label = null)
        where TNode : BranchingNode<TNode>;

    IActionNode CreateAction(string label, Action<Context> action, IPolicy? policy = null);
    IActionNode CreateAction(Action<Context> action, IPolicy? policy = null);
    IActionNode CreateAction<TAction>(string label, IPolicy? policy = null)
        where TAction : ActionNode<TAction>;
    IActionNode CreateAction<TAction>(IPolicy? policy = null)
        where TAction : ActionNode<TAction>;

    IStartingNode CreateStart(string? label = null);
    IStartingNode CreateStart<TNode>(string? label = null)
        where TNode : StartingNode<TNode>;

    ITerminationNode End(string? label = null);
    ITerminationNode End<TNode>(string? label = null)
        where TNode : TerminationNode<TNode>;
}
