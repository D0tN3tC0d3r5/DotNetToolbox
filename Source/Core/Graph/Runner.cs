namespace DotNetToolbox.Graph;

public sealed class Runner(Workflow workflow,
                           IDateTimeProvider? dateTime = null,
                           ILoggerFactory? loggerFactory = null)
    : IRunner {
    private readonly Workflow _workflow = IsNotNull(workflow);
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;
    private readonly ILogger _logger = loggerFactory?.CreateLogger<Runner>() ?? NullLogger<Runner>.Instance;

    public Func<Context, INode?, bool>? OnExecutingExecuting { get; }
    public Func<Context, INode?, bool>? OnNodeExecuted { get; }
    public Action<Workflow>? OnStartingWorkflow { get; }
    public Action<Workflow>? OnWorkflowEnded { get; }

    public DateTimeOffset? Start { get; private set; }
    public DateTimeOffset? End { get; private set; }
    public TimeSpan? ElapsedTime => End is null || Start is null ? null : End - Start;

    public bool HasStarted => Start is not null;
    public bool HasStopped => End is not null;
    public bool IsRunning => HasStarted && !HasStopped;

    public Task Run(CancellationToken ct = default) {
        try {
            if (IsRunning)
                throw new InvalidOperationException("This runner is already being executed.");
            Start = _dateTime.UtcNow;
            StartingRun(_workflow);
            var currentNode = _workflow.StartingNode;

            while (currentNode is not null) {
                if (!ExecutingNode(_workflow.Context, currentNode))
                    break;
                var result = currentNode.Run(_workflow.Context);
                if (!NodeExecuted(_workflow.Context, currentNode))
                    break;
                currentNode = result;
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, message: "An error occurred while running the workflow '{Id}'.", _workflow.Id);
            throw;
        }
        finally {
            End = _dateTime.UtcNow;
            RunEnded(_workflow);
        }
    }

    private void StartingRun(Workflow wf) {
        _logger.LogInformation(message: "Starting run of workflow '{Id}' at '{Start}'...", _workflow.Id, Start);
        OnStartingWorkflow?.Invoke(wf);
    }

    private bool ExecutingNode(Context ctx, INode? node) {
        _logger.LogInformation(message: "Executing node '{Id}'...", _workflow.Id);
        return OnExecutingExecuting?.Invoke(ctx, node) ?? node is not null;
    }

    private bool NodeExecuted(Context ctx, INode? node) {
        var cancel = OnNodeExecuted?.Invoke(ctx, node) ?? node is not null;
        _logger.LogInformation(message: "Node '{Id}' executed.", _workflow.Id);
        return !cancel;
    }

    private void RunEnded(Workflow wf) {
        OnWorkflowEnded?.Invoke(wf);
        _logger.LogInformation(message: "Run of workflow '{Id}' ended at '{End}' after '{ElapsedTime}' minutes.", _workflow.Id, End, ElapsedTime!.Value.TotalMinutes);
    }
}
