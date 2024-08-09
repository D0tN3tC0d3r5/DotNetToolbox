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

    Func<IWorkflow, CancellationToken, Task>? OnStartingWorkflow { set; }
    Func<IWorkflow, INode, CancellationToken, Task<bool>>? OnExecutingNode { set; }
    Func<IWorkflow, INode, INode?, CancellationToken, Task<bool>>? OnNodeExecuted { set; }
    Func<IWorkflow, CancellationToken, Task>? OnWorkflowEnded { set; }
}
