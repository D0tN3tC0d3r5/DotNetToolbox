namespace FluentAssertions;

public static class LoggerExtensions {
    public static LoggerAssertions Should(this ILogger logger)
        => logger is ITrackedLogger trackedLogger
            ? new(trackedLogger)
            : throw new ArgumentException($"The logger to be tested must be of type {nameof(ITrackedLogger)}", nameof(logger));
}
