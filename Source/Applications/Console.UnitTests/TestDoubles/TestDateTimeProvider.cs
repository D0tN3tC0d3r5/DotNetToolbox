namespace DotNetToolbox.ConsoleApplication.TestDoubles;

internal class TestDateTimeProvider() : IDateTimeProvider {
    public DateTimeOffset FromFileTime(long fileTime) => throw new NotImplementedException();

    public DateTimeOffset FromUnixTimeMilliseconds(long ms) => throw new NotImplementedException();

    public DateTimeOffset FromUnixTimeSeconds(long s) => throw new NotImplementedException();

    public DateTimeOffset Parse(string s) => throw new NotImplementedException();

    public DateTimeOffset ParseExact(string s, string format) => throw new NotImplementedException();

    public bool TryParse(string s, out DateTimeOffset result) => throw new NotImplementedException();

    public bool TryParseExact(string s, string format, out DateTimeOffset result) => throw new NotImplementedException();

    public DateTimeOffset Maximum { get; }
    public DateTimeOffset Minimum { get; }
    public DateTimeOffset Now { get; }
    public TimeOnly TimeOfDay { get; }
    public DateOnly Today { get; }
    public DateTimeOffset UnixEpoch { get; }
    public DateTimeOffset UtcNow { get; }
    public TimeOnly UtcTimeOfDay { get; }
    public DateOnly UtcToday { get; }
}