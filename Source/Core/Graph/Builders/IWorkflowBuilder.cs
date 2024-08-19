namespace DotNetToolbox.Graph.Builders;

public interface IWorkflowBuilder
    : INodeBuilder {
    IWorkflowBuilder AddNode(INode node);

    IWorkflowBuilder Do<TAction>(string ìd,
                                 string? label = null)
        where TAction : ActionNode<TAction>;

    IWorkflowBuilder Do(string ìd,
                        Action<Context> action,
                        string? label = null);
    IWorkflowBuilder Do(Action<Context> action,
                        string? label = null);

    IWorkflowBuilder If(string id,
                        Func<Context, bool> predicate,
                        Action<IWorkflowBuilder> setThen,
                        Action<IWorkflowBuilder>? setElse = null,
                        string? label = null);
    IWorkflowBuilder If(Func<Context, bool> predicate,
                        Action<IWorkflowBuilder> setThen,
                        Action<IWorkflowBuilder>? setElse = null,
                        string? label = null);

    IWorkflowBuilder Case(string id,
                          Func<Context, string> select,
                          Dictionary<string, Action<IWorkflowBuilder>> setCases,
                          Action<IWorkflowBuilder>? setDefault = null,
                          string? label = null);
    IWorkflowBuilder Case(Func<Context, string> select,
                          Dictionary<string, Action<IWorkflowBuilder>> setCases,
                          Action<IWorkflowBuilder>? setDefault = null,
                          string? label = null);

    IWorkflowBuilder JumpTo(string targetNodeId,
                            string? label = null);

    IWorkflowBuilder Exit(string id,
                          int exitCode = 0,
                          string? label = null);
    IWorkflowBuilder Exit(int exitCode = 0,
                          string? label = null);
}
