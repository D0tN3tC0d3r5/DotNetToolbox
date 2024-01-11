namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedLoggerFactory(ILoggerFactory loggerFactory) : ILoggerFactory {
    public void AddProvider(ILoggerProvider provider) => loggerFactory.AddProvider(new TrackedLoggerProvider(provider));
    public ILogger CreateLogger(string categoryName) => new TrackedLogger(loggerFactory.CreateLogger(categoryName));
    public void Dispose() => loggerFactory.Dispose();
}
