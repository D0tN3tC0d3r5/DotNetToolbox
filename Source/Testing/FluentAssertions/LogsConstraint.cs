namespace FluentAssertions;

public class LogsConstraint(string constraint, uint count, IEnumerable<Log> logs) {
    public LogsAssertions Logs() => new(constraint, count, logs);
}
