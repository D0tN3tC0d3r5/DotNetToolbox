namespace DotNetToolbox.TestUtilities.Logging;

public class TrackedLogger<T>(ILogger<T> logger, bool isEnable = true, bool forceCapture = false)
    : TrackedLogger(logger, isEnable, forceCapture)
    , ILogger<T> {
    public TrackedLogger(bool isEnable = true)
        : this(NullLogger<T>.Instance, isEnable, true) {
    }
}

public class TrackedLogger(ILogger logger, bool isEnable = true, bool forceCapture = false)
    : ITrackedLogger {
    private readonly List<Log> _logs = [];
    private readonly bool _forceCapture = forceCapture || logger is NullLogger;
    private bool _isEnable = isEnable;

    public TrackedLogger(bool isEnable = true)
        : this(NullLogger.Instance, isEnable, true) {
    }

    public IReadOnlyList<Log> Logs => _logs.AsReadOnly();

    public void Clear()
        => _logs.Clear();
    public void StartTracking()
        => _isEnable = true;
    public void StopTracking()
        => _isEnable = false;

    public void Log<TState>(LogLevel logLevel,
                            EventId eventId,
                            TState state,
                            Exception? exception,
                            Func<TState, Exception?, string> formatter) {
        if (IsEnabled(logLevel))
            _logs.Add(new(logLevel, eventId, state!, exception, formatter(state, exception)));
        logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
        => _isEnable && (_forceCapture || logger.IsEnabled(logLevel));

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => logger.BeginScope(state);
}
