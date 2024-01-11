namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedNullLoggerFactory : ILoggerFactory {
    public static TrackedNullLoggerFactory Instance { get; } = new();

    private TrackedNullLoggerFactory() { }

    public void AddProvider(ILoggerProvider provider) => NullLoggerFactory.Instance.AddProvider(provider);
    public ILogger CreateLogger(string categoryName) => TrackedNullLogger.Instance;
    public void Dispose() => NullLoggerFactory.Instance.Dispose();
}
