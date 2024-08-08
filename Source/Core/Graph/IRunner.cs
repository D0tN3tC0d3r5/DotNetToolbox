namespace DotNetToolbox.Graph;

public interface IRunner {
    public string Id { get; }
    public bool IsRunning { get; }
    public DateTimeOffset? Start { get; }
    public DateTimeOffset? End { get; }
    public TimeSpan? ElapsedTime { get; }
    public bool HasStarted { get; }
    public bool HasStopped { get; }

    Task Run(CancellationToken ct = default);

    Func<IRunner, IWorkflow, CancellationToken, Task>? OnStartingWorkflow { get; }
    Func<IRunner, IWorkflow, INode, CancellationToken, Task<bool>>? OnExecutingNode { get; }
    Func<IRunner, IWorkflow, INode, INode?, CancellationToken, Task<bool>>? OnNodeExecuted { get; }
    Func<IRunner, IWorkflow, CancellationToken, Task>? OnWorkflowEnded { get; }
}
