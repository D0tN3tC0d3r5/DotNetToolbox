namespace FluentAssertions;

public class LogConstraint(string constraint, IEnumerable<Log> logs) {
    public void Log() {
        if (constraint == "Not") logs.Should().NotHaveCount(1);
        else CheckQuantity(logs);
    }

    public void LogWith(LogLevel level, string? messagePattern = null) {
        if (constraint == "Not") {
            logs.Should().NotContain(log => log.Matches(level, messagePattern));
        }
        else {
            logs.Should().Contain(log => log.Matches(level, messagePattern));
            var matches = logs.Where(log => log.Matches(level)).ToArray();
            CheckQuantity(matches);
        }
    }

    public void LogWith(string messagePattern) {
        if (constraint == "Not") {
            logs.Should().NotContain(log => log.Matches(messagePattern));
        }
        else {
            logs.Should().Contain(log => log.Matches(messagePattern));
            var matches = logs.Where(log => log.Matches(messagePattern)).ToArray();
            CheckQuantity(matches);
        }
    }

    private void CheckQuantity(IEnumerable<Log> matches) {
        switch (constraint) {
            case "Exactly":
                matches.Should().HaveCount(1);
                break;
            case "AtLeast":
                matches.Should().HaveCountGreaterOrEqualTo(1);
                break;
            case "AtMost":
                matches.Should().HaveCountLessOrEqualTo(1);
                break;
        }
    }
}
