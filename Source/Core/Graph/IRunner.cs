namespace DotNetToolbox.Graph;

public interface IRunner {
    public bool IsRunning { get; }
    public DateTimeOffset? Start { get; }
    public DateTimeOffset? End { get; }
    public TimeSpan? ElapsedTime { get; }
    public bool HasStarted { get; }
    public bool HasStopped { get; }

    Task Run(CancellationToken ct = default);

    Func<Context, INode, bool>? OnExecutingExecuting { get; }
    Func<Context, INode, bool>? OnNodeExecuted { get; }
    Action<Workflow>? OnStartingWorkflow { get; }
    Action<Workflow>? OnWorkflowEnded { get; }
}
