namespace FluentAssertions;

internal static class LogExtensions {
    public static bool Matches(this Log log, string messagePattern)
        => IsMatch(log, null, messagePattern ?? throw new ArgumentNullException(nameof(messagePattern)));

    public static bool Matches(this Log log, LogLevel level, string? messagePattern = null)
        => IsMatch(log, level, messagePattern);

    private static bool IsMatch(Log log, LogLevel? level, string? pattern = null)
        => LevelIsValid(log, level) && MessageIsValid(log, pattern);

    private static bool MessageIsValid(Log log, string? pattern) {
        if (pattern is null) return true;

        var matchKey = (pattern.StartsWith("*") ? 1 : 0)
                     + (pattern.EndsWith("*") ? 2 : 0);
        var text = pattern.Trim('*');
        return matchKey switch {
            3 => log.Message.Contains(text),
            2 => log.Message.StartsWith(text),
            1 => log.Message.EndsWith(text),
            _ => log.Message == pattern
        };
    }

    private static bool LevelIsValid(Log log, LogLevel? level)
        => !level.HasValue
        || log.Level == level.Value;
}
