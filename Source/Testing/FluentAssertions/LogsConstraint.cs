namespace FluentAssertions;

public class LogsConstraint(string constraint, int count, IEnumerable<Log> logs) {
    public void Logs() {
        if (constraint == "Not") logs.Should().NotHaveCount(count);
        else CheckQuantity(logs);
    }

    public void LogsWith(LogLevel level, string? messagePattern = null) {
        if (constraint == "Not" || count == 0) {
            logs.Should().NotContain(log => log.Matches(level, messagePattern));
        }
        else {
            logs.Should().Contain(log => log.Matches(level, messagePattern));
            var matches = logs.Where(log => log.Matches(level, messagePattern)).ToArray();
            CheckQuantity(matches);
        }
    }

    public void LogsWith(string messagePattern) {
        if (constraint == "Not" || count == 0) {
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
                matches.Should().HaveCount(count);
                break;
            case "AtLeast":
                matches.Should().HaveCountGreaterThanOrEqualTo(count);
                break;
            case "AtMost":
                matches.Should().HaveCountLessThanOrEqualTo(count);
                break;
        }
    }
}
