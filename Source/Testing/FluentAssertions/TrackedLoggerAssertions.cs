namespace FluentAssertions;

public class TrackedLoggerAssertions(ILogger logger) {
    private readonly IReadOnlyList<Log> _logs = logger is ITrackedLogger tracked
        ? tracked.Logs
        : throw new ArgumentException("Logger does not support tracking", nameof(logger));

    public void NotHaveBeenCalled() => _logs.Should().BeEmpty();

    public AndConstraint<TrackedLoggerAssertions> HaveBeenCalled() {
        _logs.Should().NotBeEmpty();
        return new(this);
    }

    public LogsConstraint Have()
        => new(string.Empty, 0, _logs);

    public LogsConstraint HaveASingleLog()
        => new("Exactly", 1, _logs);

    public LogsConstraint Have(uint count)
        => count switch {
            0 => throw new ArgumentException("Use NotHave instead", nameof(count)),
            1 => throw new ArgumentException("Use HaveASingleLog instead", nameof(count)),
            _ => new("Exactly", count, _logs)
        };

    public LogsConstraint HaveAtLeast(uint count)
        => new("AtLeast", count, _logs);

    public LogsConstraint HaveAtMost(uint count)
        => new("AtMost", count, _logs);

    public LogsConstraint NotHave()
        => new("Not", 0, _logs);
}
