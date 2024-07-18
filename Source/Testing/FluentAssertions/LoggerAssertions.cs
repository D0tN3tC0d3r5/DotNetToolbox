namespace FluentAssertions;

public class LoggerAssertions(ITrackedLogger logger) {
    private readonly IReadOnlyList<Log> _logs = logger.Logs;

    public void BeEmpty() => _logs.Should().BeEmpty();

    public AndConstraint<LoggerAssertions> NotBeEmpty() {
        _logs.Should().NotBeEmpty();
        return new(this);
    }

    public AndWhichConstraint<LoggerAssertions, IEnumerable<Log>> Contain(LogLevel level, string? message = null) {
        _logs.Should().Contain(log => log.With(level, message));
        var matches = _logs.Where(log => log.With(level, message)).ToArray();
        return new(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, IEnumerable<Log>> Contain(string message) {
        _logs.Should().Contain(log => log.With(null, EmptyIfNull(message)));
        var matches = _logs.Where(log => log.With(null, EmptyIfNull(message))).ToArray();
        return new(this, matches);
    }

    public AndConstraint<LoggerAssertions> NotContain(LogLevel level, string? message = null) {
        _logs.Should().NotContain(log => log.With(level, message));
        return new(this);
    }

    public AndConstraint<LoggerAssertions> NotContain(string? message) {
        _logs.Should().NotContain(log => log.With(null, EmptyIfNull(message)));
        return new(this);
    }

    public AndConstraint<LoggerAssertions> ContainExactly(ushort expectedCount) {
        if (expectedCount == 0) {
            _logs.Should().BeEmpty();
            return new(this);
        }

        _logs.Should().HaveCount(expectedCount);
        return new(this);
    }

    public AndWhichConstraint<LoggerAssertions, IEnumerable<Log>> ContainExactly(ushort expectedCount, LogLevel level, string? message = null) {
        if (expectedCount == 0) {
            _logs.Should().NotContain(log => log.With(level, message));
            return new(this, Array.Empty<Log>());
        }

        _logs.Should().Contain(log => log.With(level, message));
        var matches = _logs.Where(log => log.With(level, message)).ToArray();
        matches.Should().HaveCount(expectedCount);
        return new(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, IEnumerable<Log>> ContainExactly(ushort expectedCount, string message) {
        if (expectedCount == 0) {
            _logs.Should().NotContain(log => log.With(null, EmptyIfNull(message)));
            return new(this, Array.Empty<Log>());
        }

        _logs.Should().Contain(log => log.With(null, EmptyIfNull(message)));
        var matches = _logs.Where(log => log.With(null, EmptyIfNull(message))).ToArray();
        matches.Should().HaveCount(expectedCount);
        return new(this, matches);
    }

    public AndConstraint<LoggerAssertions> ContainAtLeast(ushort expectedCount) {
        _logs.Should().HaveCountGreaterOrEqualTo(expectedCount);
        return new(this);
    }

    public AndConstraint<LoggerAssertions> ContainAtMost(ushort expectedCount) {
        _logs.Should().HaveCountLessOrEqualTo(expectedCount);
        return new(this);
    }

    private static string EmptyIfNull(string? message) => message ?? string.Empty;
}
