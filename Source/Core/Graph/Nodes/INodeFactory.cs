namespace DotNetToolbox.Graph.Nodes;

public interface INodeFactory {
    string GenerateId();

    IConditionalNode If(Func<Context, bool> predicate,
                        Action<WorkflowBuilder> setTrueBranch,
                        Action<WorkflowBuilder>? setFalseBranch = null);

    IBranchingNode Select(Func<Context, string> selectPath,
                          Action<BranchesBuilder> setPaths);

    IActionNode Do(Action<Context> action,
                   Action<WorkflowBuilder>? buildNext = null,
                   IPolicy? policy = null);

    IActionNode Do<TAction>()
        where TAction : ActionNode<TAction>;

    INode Start { get; }

    INode End { get; }
}
