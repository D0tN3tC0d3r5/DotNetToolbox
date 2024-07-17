namespace DotNetToolbox.Graph;

public sealed class Runner(INode startingNode,
                           IGuidProvider? guid = null,
                           IDateTimeProvider? dateTime = null,
                           ILoggerFactory? loggerFactory = null)
    : IRunner {
    private readonly INode _startingNode = IsNotNull(startingNode);
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;
    private readonly ILogger _logger = loggerFactory?.CreateLogger<Runner>() ?? NullLogger<Runner>.Instance;

    public string Id { get; } = (guid ?? GuidProvider.Default).Create().ToString();
    public DateTimeOffset? Start { get; private set; }
    public DateTimeOffset? End { get; private set; }
    public TimeSpan? ElapsedTime => End is null || Start is null ? null : End - Start;

    public bool HasStarted => Start is not null;
    public bool HasStopped => End is not null;
    public bool IsRunning => HasStarted && !HasStopped;

    public Context Run(Context? context = null) {
        _logger.LogInformation("Workflow '{id}' starting...", Id);
        var currentNode = _startingNode;
        context ??= [];
        try {
            if (IsRunning)
                throw new InvalidOperationException("This runner is already being executed.");
            Start = _dateTime.UtcNow;
            while (currentNode is not null)
                currentNode = currentNode.Run(context);
            return context;
        }
        catch (RunnerException ex) {
            _logger.LogError(ex, "An error occurred while running the workflow '{id}'.", Id);
            throw;
        }
        finally {
            End = _dateTime.UtcNow;
            _logger.LogInformation("Workflow '{id}' ended.", Id);
        }
    }

    public override int GetHashCode() => Id.GetHashCode();
}

[Serializable]
internal class RunnerException(int errorCode, string message, Exception? innerException = null)
    : Exception(IsNotNullOrWhiteSpace(message), innerException) {
    public RunnerException(int errorCode = 1, Exception? innerException = null)
        : this(errorCode, "An error occurred while running the workflow.", innerException) {
        ErrorCode = errorCode;
    }

    public RunnerException(string message, Exception? innerException = null)
        : this(1, IsNotNullOrWhiteSpace(message), innerException) {
    }

    public int ErrorCode { get; } = errorCode;
}
