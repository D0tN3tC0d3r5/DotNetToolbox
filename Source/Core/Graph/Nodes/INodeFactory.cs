namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    IConditionalNode CreateFork(uint id,
                                Func<Context, bool> predicate,
                                WorkflowBuilder builder,
                                Action<WorkflowBuilder> setTrueBranch,
                                Action<WorkflowBuilder>? setFalseBranch = null);
    IConditionalNode CreateFork(uint id,
                                string label,
                                Func<Context, bool> predicate,
                                WorkflowBuilder builder,
                                Action<WorkflowBuilder> setTrueBranch,
                                Action<WorkflowBuilder>? setFalseBranch = null);
    IConditionalNode CreateFork<TNode>(uint id,
                                       string? label = null)
        where TNode : ConditionalNode<TNode>;

    IBranchingNode CreateChoice(uint id,
                                Func<Context, string> selectPath,
                                WorkflowBuilder builder,
                                Action<BranchesBuilder> setPaths);
    IBranchingNode CreateChoice(uint id,
                                string label,
                                Func<Context, string> selectPath,
                                WorkflowBuilder builder,
                                Action<BranchesBuilder> setPaths);
    IBranchingNode CreateChoice<TNode>(uint id,
                                       string? label = null)
        where TNode : BranchingNode<TNode>;

    IActionNode CreateAction(uint id,
                             string label,
                             Action<Context> action,
                             IPolicy? policy = null);
    IActionNode CreateAction(uint id,
                             Action<Context> action,
                             IPolicy? policy = null);
    IActionNode CreateAction<TAction>(uint id,
                                      string label,
                                      IPolicy? policy = null)
        where TAction : ActionNode<TAction>;
    IActionNode CreateAction<TAction>(uint id,
                                      IPolicy? policy = null)
        where TAction : ActionNode<TAction>;

    ITerminationNode CreateStop(uint id,
                                string? label = null);
}
