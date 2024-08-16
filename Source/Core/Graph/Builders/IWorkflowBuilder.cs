namespace DotNetToolbox.Graph.Builders;

public interface IWorkflowBuilder
    : INodeBuilder {
    IWorkflowBuilder Do(string tag, Action<Context> action, string? label = null);
    IWorkflowBuilder Do(Action<Context> action, string? label = null);
    IWorkflowBuilder Do<TAction>(string? tag = null, string? label = null)
        where TAction : ActionNode<TAction>;

    IWorkflowBuilder If(string tag, Func<Context, bool> predicate, Action<IIfNodeBuilder> buildBranches, string? label = null);
    IWorkflowBuilder If(Func<Context, bool> predicate, Action<IIfNodeBuilder> buildBranches, string? label = null);

    IWorkflowBuilder Case(string tag, Func<Context, string> select, Action<ICaseNodeBuilder> buildChoices, string? label = null);
    IWorkflowBuilder Case(Func<Context, string> select, Action<ICaseNodeBuilder> buildChoices, string? label = null);

    IWorkflowBuilder JumpTo(string targetTag, string? label = null);
    IWorkflowBuilder Exit(string? tag = null, int exitCode = 0, string? label = null);
}
