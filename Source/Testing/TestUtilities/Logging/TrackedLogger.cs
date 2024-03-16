namespace DotNetToolbox.TestUtilities.Logging;

public class TrackedLogger<T>(ILogger<T> logger, bool trackAllLevels = false, bool startsImmediately = true)
    : TrackedLogger(logger, trackAllLevels, startsImmediately), ILogger<T>;

public class TrackedLogger(ILogger logger, bool trackAllLevels = false, bool startsImmediately = true)
    : ITrackedLogger {
    private readonly List<Log> _logs = [];
    public IReadOnlyList<Log> Logs => _logs.AsReadOnly();
    public bool IsTrackingAllLevel { get; } = trackAllLevels;
    public bool IsTracking { get; private set; } = startsImmediately;

    public void Clear() => _logs.Clear();
    public void StartTracking() => IsTracking = true;
    public void StopTracking() => IsTracking = false;

    public void Log<TState>(LogLevel logLevel,
                            EventId eventId,
                            TState state,
                            Exception? exception,
                            Func<TState, Exception?, string> formatter) {
        if (IsTracking && IsEnabled(logLevel))
            _logs.Add(new(logLevel, eventId, state!, exception, formatter(state, exception)));
        logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public virtual bool IsEnabled(LogLevel logLevel) => IsTrackingAllLevel || logger.IsEnabled(logLevel);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => logger.BeginScope(state);
}
