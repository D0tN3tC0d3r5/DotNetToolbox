namespace DotNetToolbox.Graph.Builders;

public interface IWorkflowBuilder
    : INodeBuilder {
    IWorkflowBuilder AddNode(INode node, Token? token = null);

    IWorkflowBuilder Do<TAction>(string ìd,
                                 string? label = null,
                                 Token? token = null)
        where TAction : ActionNode<TAction>;

    IWorkflowBuilder Do(string ìd,
                        Action<Context> action,
                        string? label = null,
                        Token? token = null);
    IWorkflowBuilder Do(Action<Context> action,
                        string? label = null,
                        Token? token = null);

    IWorkflowBuilder If(string id,
                        Func<Context, bool> predicate,
                        string? label = null,
                        Token? token = null);
    IWorkflowBuilder If(string id,
                        Func<Context, bool> predicate,
                        Action<IWorkflowBuilder> setThen,
                        Action<IWorkflowBuilder>? setElse = null,
                        string? label = null,
                        Token? token = null);
    IWorkflowBuilder If(Func<Context, bool> predicate,
                        Action<IWorkflowBuilder> setThen,
                        Action<IWorkflowBuilder>? setElse = null,
                        string? label = null,
                        Token? token = null);

    IWorkflowBuilder Case(string id,
                          Func<Context, string> select,
                          string? label = null,
                          Token? token = null);
    IWorkflowBuilder Case(string id,
                          Func<Context, string> select,
                          Dictionary<string, Action<IWorkflowBuilder>> setCases,
                          Action<IWorkflowBuilder>? setDefault = null,
                          string? label = null,
                          Token? token = null);
    IWorkflowBuilder Case(Func<Context, string> select,
                          Dictionary<string, Action<IWorkflowBuilder>> setCases,
                          Action<IWorkflowBuilder>? setDefault = null,
                          string? label = null,
                          Token? token = null);

    IWorkflowBuilder JumpTo(string id,
                            string targetNodeId,
                            string? label = null,
                            Token? token = null);
    IWorkflowBuilder JumpTo(string targetNodeId,
                            string? label = null,
                            Token? token = null);

    IWorkflowBuilder Exit(string id,
                          int exitCode = 0,
                          string? label = null,
                          Token? token = null);
    IWorkflowBuilder Exit(int exitCode = 0,
                          string? label = null,
                          Token? token = null);
}
