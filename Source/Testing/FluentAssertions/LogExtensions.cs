namespace FluentAssertions;

internal static class LogExtensions {
    public static bool With(this Log log, LogLevel? level, string? message) {
        var result = true;
        result &= !level.HasValue || log.Level == level.Value;
        result &= message is null || log.Message == message;
        return result;
    }
}
