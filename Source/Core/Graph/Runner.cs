namespace DotNetToolbox.Graph;

public sealed class Runner(string id,
                           Workflow workflow,
                           IDateTimeProvider? dateTime = null,
                           ILoggerFactory? loggerFactory = null)
    : IRunner {
    private readonly Workflow _workflow = IsNotNull(workflow);
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;
    private readonly ILogger _logger = loggerFactory?.CreateLogger<Runner>() ?? NullLogger<Runner>.Instance;

    public Runner(Workflow workflow,
                  IDateTimeProvider? dateTime = null,
                  IGuidProvider? guid = null,
                  ILoggerFactory? loggerFactory = null)
        : this((guid ?? GuidProvider.Default).AsSortable.Create().ToString(),
               workflow,
               dateTime,
               loggerFactory) {
    }

    public string Id { get; } = id;

    public Func<IRunner, IWorkflow, CancellationToken, Task>? OnStartingWorkflow { get; set; }
    public Func<IRunner, IWorkflow, INode, CancellationToken, Task<bool>>? OnExecutingNode { get; set; }
    public Func<IRunner, IWorkflow, INode, INode?, CancellationToken, Task<bool>>? OnNodeExecuted { get; set; }
    public Func<IRunner, IWorkflow, CancellationToken, Task>? OnWorkflowEnded { get; set; }

    public DateTimeOffset? Start { get; private set; }
    public DateTimeOffset? End { get; private set; }
    public TimeSpan? ElapsedTime => End is null || Start is null ? null : End - Start;

    public bool HasStarted => Start is not null;
    public bool HasStopped => End is not null;
    public bool IsRunning => HasStarted && !HasStopped;

    public async Task Run(CancellationToken ct = default) {
        try {
            Start = _dateTime.UtcNow;
            if (IsRunning) throw new InvalidOperationException("This runner is already being executed.");
            await StartingRun(_workflow, ct);
            var currentNode = _workflow.StartNode;

            while (currentNode is not null) {
                if (!await ExecutingNode(_workflow, currentNode, ct)) break;
                var nextNode = await currentNode.Run(_workflow.Context, ct);
                if (!await NodeExecuted(_workflow, currentNode, nextNode, ct)) break;
                currentNode = nextNode;
            }
        }
        catch (OperationCanceledException ex) {
            _logger.LogError(ex, message: "The run '{RunId}' of the workflow '{WorkFlowId}' was cancelled.", Id, _workflow.Id);
            throw;
        }
        catch (Exception ex) {
            _logger.LogError(ex, message: "An error occurred during the run '{RunId}' the workflow '{WorkFlowId}'.", Id, _workflow.Id);
            throw;
        }
        finally {
            await RunEnded(_workflow, ct);
            End = _dateTime.UtcNow;
        }
    }

    private Task StartingRun(IWorkflow workflow, CancellationToken ct = default) {
        _logger.LogInformation(message: "Starting run {RunId} of workflow '{WorkFlowId}' at '{Start}'...", Id, _workflow.Id, Start);
        return OnStartingWorkflow is null
                   ? Task.CompletedTask
                   : OnStartingWorkflow(this, workflow, ct);
    }

    private Task<bool> ExecutingNode(IWorkflow workflow, INode currentNode, CancellationToken ct = default) {
        _logger.LogInformation(message: "Executing node '{Id}' during the run '{RunId}' of the workflow '{WorkFlowId}'...", currentNode.Id, Id, _workflow.Id);
        return OnExecutingNode is null
                   ? Task.FromResult(true)
                   : OnExecutingNode(this, workflow, currentNode, ct);
    }

    private Task<bool> NodeExecuted(IWorkflow workflow, INode currentNode, INode? nextNode, CancellationToken ct = default) {
        _logger.LogInformation(message: "Node '{NodeId}' executed during the run '{RunId}' of the workflow '{WorkFlowId}'.", currentNode.Id, Id, _workflow.Id);
        return OnNodeExecuted is null
                   ? Task.FromResult(true)
                   : OnNodeExecuted(this, workflow, currentNode, nextNode, ct);
    }

    private Task RunEnded(IWorkflow workflow, CancellationToken ct = default) {
        _logger.LogInformation(message: "The run '{RunId}' of the workflow '{WorkFlowId}' ended at '{End}' after '{ElapsedTimeInMilliseconds}' milliseconds.", Id, _workflow.Id, End, ElapsedTime!.Value.TotalMilliseconds);
        return OnWorkflowEnded is null
            ? Task.CompletedTask
            : OnWorkflowEnded(this, workflow, ct);
    }
}
