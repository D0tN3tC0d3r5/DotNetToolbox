namespace DotNetToolbox.Graph.Builders;

public interface IIfBuilder {
    IElseBuilder Then(Action<IWorkflowBuilder> setThen);
}
public interface IActionBuilder : IWorkflowBuilder {
    IWorkflowBuilder WithRetry(IRetryPolicy retry);
}
public interface IElseBuilder : IWorkflowBuilder {
    IWorkflowBuilder Else(Action<IWorkflowBuilder> setElse);
}

public interface ICaseBuilder : IWorkflowBuilder {
    IOtherwiseBuilder Is(string key, Action<IWorkflowBuilder> setCase);
}
public interface IOtherwiseBuilder : ICaseBuilder {
    IWorkflowBuilder Otherwise(Action<IWorkflowBuilder> setOtherwise);
}

public interface IExitBuilder
    : INodeBuilder;

public interface IWorkflowBuilder
    : INodeBuilder {
    IWorkflowBuilder AddNode(INode node);

    IWorkflowBuilder Do<TAction>(params object?[] args)
        where TAction : ActionNode<TAction>;
    IWorkflowBuilder Do<TAction>(string tag, params object?[] args)
        where TAction : ActionNode<TAction>;

    IWorkflowBuilder Do(Action<Context> action);
    IWorkflowBuilder Do(Func<Context, CancellationToken, Task> action);
    IWorkflowBuilder Do(string tag, Action<Context> action);
    IWorkflowBuilder Do(string tag, Func<Context, CancellationToken, Task> action);

    IIfBuilder If(Func<Context, bool> predicate);
    IIfBuilder If(Func<Context, CancellationToken, Task<bool>> predicate);
    IIfBuilder If(string tag, Func<Context, bool> predicate);
    IIfBuilder If(string tag, Func<Context, CancellationToken, Task<bool>> predicate);

    ICaseBuilder Case(Func<Context, string> select);
    ICaseBuilder Case(Func<Context, CancellationToken, Task<string>> select);
    ICaseBuilder Case(string tag, Func<Context, string> select);
    ICaseBuilder Case(string tag, Func<Context, CancellationToken, Task<string>> select);

    IWorkflowBuilder GoTo(string nodeTag);

    IExitBuilder Exit(int exitCode = 0);
    IExitBuilder Exit(string tag, int exitCode = 0);
}
