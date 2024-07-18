namespace FluentAssertions;

public class LogsAssertions(string constraint, uint count, IEnumerable<Log> logs) {
    public AndWhichConstraint<LogsAssertions, IEnumerable<Log>> WithLevel(LogLevel level) {
        if (constraint == "Not") {
            logs.Should().NotContain(log => log.Matches(level, null));
            return new(this, logs);
        }
        logs.Should().Contain(log => log.Matches(level, null));
        var matches = logs.Where(log => log.Matches(level)).ToArray();
        CheckQuantity(matches);
        return new(this, matches);
    }

    public AndWhichConstraint<LogsAssertions, IEnumerable<Log>> WithMessage(string message) {
        if (constraint == "Not") {
            logs.Should().NotContain(log => log.Matches(message));
            return new(this, logs);
        }
        logs.Should().Contain(log => log.Matches(message));
        var matches = logs.Where(log => log.Matches(message)).ToArray();
        CheckQuantity(matches);
        return new(this, matches);
    }

    private void CheckQuantity(IEnumerable<Log> matches) {
        switch (constraint) {
            case "Exactly":
                matches.Should().HaveCount((int)count);
                break;
            case "AtLeast":
                matches.Should().HaveCountGreaterOrEqualTo((int)count);
                break;
            case "AtMost":
                matches.Should().HaveCountLessOrEqualTo((int)count);
                break;
        }
    }
}
