namespace DotNetToolbox.TestUtilities.Logging;

public record Log() {
    [SetsRequiredMembers]
    public Log(LogLevel level, EventId eventId, object state, Exception? exception, string message)
        : this() {
        Level = level;
        EventId = eventId;
        State = state;
        Exception = exception;
        Message = message;
    }

    public LogLevel Level { get; init; }
    public EventId EventId { get; init; }
    public object State { get; init; } = default!;
    public Exception? Exception { get; init; }
    public string Message { get; init; } = string.Empty;
}
