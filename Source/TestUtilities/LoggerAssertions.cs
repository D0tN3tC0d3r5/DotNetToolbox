namespace DotNetToolbox.TestUtilities;

public class LoggerAssertions {
    private readonly IEnumerable<ICall> _calls;

    public LoggerAssertions(IEnumerable<ICall> calls) {
        var actualValue = calls as ICall[] ?? calls.ToArray();
        _calls = actualValue;
    }

    public void BeEmpty()
        => _calls.Should().BeEmpty();
    public AndConstraint<LoggerAssertions> NotBeEmpty() {
        _calls.Should().NotBeEmpty();
        return new AndConstraint<LoggerAssertions>(this);
    }
    public AndConstraint<LoggerAssertions> HaveExactly(ushort expectedCount) {
        _calls.Should().HaveCount(expectedCount);
        return new AndConstraint<LoggerAssertions>(this);
    }
    public AndConstraint<LoggerAssertions> HaveAtLeast(ushort expectedCount) {
        _calls.Should().HaveCountGreaterOrEqualTo(expectedCount);
        return new AndConstraint<LoggerAssertions>(this);
    }
    public AndConstraint<LoggerAssertions> HaveAtMost(ushort expectedCount) {
        _calls.Should().HaveCountLessOrEqualTo(expectedCount);
        return new AndConstraint<LoggerAssertions>(this);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> Contain(LogLevel level, string? message = null) {
        _calls.Should().Contain(c => With(c, level, message));
        var matches = _calls.Where(c => With(c, level, message)).ToArray();
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndConstraint<LoggerAssertions> NotContain(LogLevel level, string? message = null) {
        _calls.Should().NotContain(c => With(c, level, message));
        return new AndConstraint<LoggerAssertions>(this);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainSingle(LogLevel level, string? message = null) {
        var match = _calls.Should().ContainSingle(c => With(c, level, message)).Which;
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, match);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainExactly(ushort expectedCount, LogLevel level, string? message = null) {
        _calls.Should().Contain(c => With(c, level, message));
        var matches = _calls.Where(c => With(c, level, message)).ToArray();
        matches.Should().HaveCount(expectedCount);
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainAtLeast(ushort expectedCount, LogLevel level, string? message = null) {
        _calls.Should().Contain(c => With(c, level, message));
        var matches = _calls.Where(c => With(c, level, message)).ToArray();
        matches.Should().HaveCountGreaterOrEqualTo(expectedCount);
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainAtMost(ushort expectedCount, LogLevel level, string? message = null) {
        _calls.Should().Contain(c => With(c, level, message));
        var matches = _calls.Where(c => With(c, level, message)).ToArray();
        matches.Should().HaveCountLessOrEqualTo(expectedCount);
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> Contain(string message) {
        _calls.Should().Contain(c => With(c, null, message));
        var matches = _calls.Where(c => With(c, null, message)).ToArray();
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndConstraint<LoggerAssertions> NotContain(string message) {
        _calls.Should().NotContain(c => With(c, null, message));
        return new AndConstraint<LoggerAssertions>(this);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainSingle(string message) {
        var match = _calls.Should().ContainSingle(c => With(c, null, message)).Which;
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, match);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainExactly(ushort expectedCount, string message) {
        _calls.Should().Contain(c => With(c, null, message));
        var matches = _calls.Where(c => With(c, null, message)).ToArray();
        matches.Should().HaveCount(expectedCount);
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainAtLeast(ushort expectedCount, string message) {
        _calls.Should().Contain(c => With(c, null, message));
        var matches = _calls.Where(c => With(c, null, message)).ToArray();
        matches.Should().HaveCountGreaterOrEqualTo(expectedCount);
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    public AndWhichConstraint<LoggerAssertions, ICall> ContainAtMost(ushort expectedCount, string message) {
        _calls.Should().Contain(c => With(c, null, message));
        var matches = _calls.Where(c => With(c, null, message)).ToArray();
        matches.Should().HaveCountLessOrEqualTo(expectedCount);
        return new AndWhichConstraint<LoggerAssertions, ICall>(this, matches);
    }

    private static bool With(ICall call, LogLevel? level, string? message)
        => call.GetArguments() is [LogLevel lvl, EventId _, { } msg, ..]
        && (!level.HasValue || level.Value == lvl)
        && (message is null || $"{msg}" == message);
}
