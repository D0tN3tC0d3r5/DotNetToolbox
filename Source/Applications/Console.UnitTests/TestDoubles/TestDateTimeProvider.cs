namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal sealed class TestDateTimeProvider : IDateTimeProvider {
    public DateTimeOffset FromFileTime(long fileTime) => throw new NotImplementedException();

    public DateTimeOffset FromUnixTimeMilliseconds(long ms) => throw new NotImplementedException();

    public DateTimeOffset FromUnixTimeSeconds(long s) => throw new NotImplementedException();

    public DateTimeOffset Parse(string s) => throw new NotImplementedException();

    public DateTimeOffset ParseExact(string s, string format) => throw new NotImplementedException();

    public bool TryParse(string s, out DateTimeOffset result) => throw new NotImplementedException();

    public bool TryParseExact(string s, string format, out DateTimeOffset result) => throw new NotImplementedException();

    public DateTimeOffset Maximum { get; } = DateTimeOffset.MaxValue;
    public DateTimeOffset Minimum { get; } = DateTimeOffset.MaxValue;
    public DateTimeOffset Now { get; } = DateTimeOffset.MaxValue;
    public TimeOnly TimeOfDay { get; } = TimeOnly.MaxValue;
    public DateOnly Today { get; } = DateOnly.MaxValue;
    public DateTimeOffset UnixEpoch { get; } = DateTimeOffset.MaxValue;
    public DateTimeOffset UtcNow { get; } = DateTimeOffset.MaxValue;
    public TimeOnly UtcTimeOfDay { get; } = TimeOnly.MaxValue;
    public DateOnly UtcToday { get; } = DateOnly.MaxValue;
}
