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
        => new(string.Empty, -1, _logs);

    public LogConstraint HaveSingle()
        => new("Exactly", _logs);

    public LogsConstraint Have(uint count)
        => new("Exactly", (int)count, _logs);

    public LogsConstraint HaveAtLeast(uint count)
        => new("AtLeast", (int)count, _logs);

    public LogsConstraint HaveAtMost(uint count)
        => new("AtMost", (int)count, _logs);

    public LogsConstraint NotHave()
        => new("Not", 0, _logs);
}
