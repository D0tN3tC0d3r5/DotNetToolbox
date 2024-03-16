namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedNullLogger<T>(LogLevel minimumLevel = default, bool startsImmediately = true)
    : TrackedNullLogger(NullLogger<T>.Instance, minimumLevel, startsImmediately: startsImmediately), ILogger<T> {
    // This static method intentionally does not create a singleton for testing isolation.
    public static new TrackedNullLogger<T> Instance => new();
}

public class TrackedNullLogger
    : TrackedLogger {
    protected TrackedNullLogger(ILogger logger, LogLevel minimumLevel = default, bool startsImmediately = true)
        : base(logger, startsImmediately: startsImmediately) {
        MinimumLevel = minimumLevel;
    }

    public TrackedNullLogger(LogLevel minimumLevel = default, bool startsImmediately = true)
        : this(NullLogger.Instance, minimumLevel, startsImmediately) {
    }

    // This static method intentionally does not create a singleton for testing isolation.
    public static TrackedNullLogger Instance => new();

    public LogLevel MinimumLevel { get; }
    public override bool IsEnabled(LogLevel logLevel) => logLevel >= MinimumLevel;
}
