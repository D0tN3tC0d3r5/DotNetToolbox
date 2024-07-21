namespace DotNetToolbox.Graph;

public sealed class Runner(INode startingNode,
                           IGuidProvider? guid = null,
                           IDateTimeProvider? dateTime = null,
                           ILoggerFactory? loggerFactory = null)
    : IRunner {
    private readonly INode _startingNode = IsNotNull(startingNode);
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;
    private readonly ILogger _logger = loggerFactory?.CreateLogger<Runner>() ?? NullLogger<Runner>.Instance;

    public event EventHandler<NodeEventArgs>? NodeExecuting;
    public event EventHandler<NodeEventArgs>? NodeExecuted;

    public string Id { get; } = (guid ?? GuidProvider.Default).AsSortable.Create().ToString();
    public DateTimeOffset? Start { get; private set; }
    public DateTimeOffset? End { get; private set; }
    public TimeSpan? ElapsedTime => End is null || Start is null ? null : End - Start;

    public bool HasStarted => Start is not null;
    public bool HasStopped => End is not null;
    public bool IsRunning => HasStarted && !HasStopped;

    public Context Run(Context? context = null) {
        _logger.LogInformation(message: "Workflow '{id}' starting...", Id);
        var currentNode = _startingNode;
        context ??= [];
        try {
            if (IsRunning)
                throw new InvalidOperationException("This runner is already being executed.");

            Start = _dateTime.UtcNow;
            while (OnNodeExecuting(new(context, currentNode))) {
                currentNode = currentNode!.Run(context);
                if (!OnNodeExecuted(new(context, currentNode)))
                    break;
            }

            return context;
        }
        catch (Exception ex) {
            _logger.LogError(ex, message: "An error occurred while running the workflow '{id}'.", Id);
            throw;
        }
        finally {
            End = _dateTime.UtcNow;
            _logger.LogInformation(message: "Workflow '{id}' ended.", Id);
        }
    }

    public override int GetHashCode() => Id.GetHashCode();

    private bool OnNodeExecuting(NodeEventArgs e) {
        if (NodeExecuting is null)
            return e.Node is not null;

        NodeExecuting.Invoke(this, e);
        return e.Continue;
    }

    private bool OnNodeExecuted(NodeEventArgs e) {
        if (NodeExecuted is null)
            return e.Node is not null;

        NodeExecuted.Invoke(this, e);
        return e.Continue;
    }
}
