namespace DotNetToolbox.Graph.Builders;

public interface IWorkflowBuilder
    : INodeBuilder {
    IWorkflowBuilder AddNode(INode node);

    IWorkflowBuilder Do<TAction>(params object[] args)
        where TAction : ActionNode<TAction>;
    IWorkflowBuilder Do<TAction>(string tag, params object[] args)
        where TAction : ActionNode<TAction>;

    IActionBuilder Do(string name);
    IActionBuilder Do(string tag, string name);

    IActionBuilder Do(Action<Map> action);
    IActionBuilder Do(Func<Map, CancellationToken, Task> action);
    IActionBuilder Do(string tag, Action<Map> action);
    IActionBuilder Do(string tag, Func<Map, CancellationToken, Task> action);

    IIfBuilder If(string name);
    IIfBuilder If(string tag, string name);
    IIfBuilder If(Func<Map, bool> predicate);
    IIfBuilder If(Func<Map, CancellationToken, Task<bool>> predicate);
    IIfBuilder If(string tag, Func<Map, bool> predicate);
    IIfBuilder If(string tag, Func<Map, CancellationToken, Task<bool>> predicate);

    ICaseBuilder Case(string selector);
    ICaseBuilder Case(string tag, string selector);
    ICaseBuilder Case(Func<Map, string> select);
    ICaseBuilder Case(Func<Map, CancellationToken, Task<string>> select);
    ICaseBuilder Case(string tag, Func<Map, string> select);
    ICaseBuilder Case(string tag, Func<Map, CancellationToken, Task<string>> select);

    IWorkflowBuilder GoTo(string nodeTag);

    IExitBuilder Exit(int exitCode = 0);
    IExitBuilder Exit(string tag, int exitCode = 0);
}
