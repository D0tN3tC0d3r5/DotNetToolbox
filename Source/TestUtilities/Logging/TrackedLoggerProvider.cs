namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedLoggerProvider(ILoggerProvider loggerProvider) : ILoggerProvider {
    public ILogger CreateLogger(string categoryName) => new TrackedLogger(loggerProvider.CreateLogger(categoryName));
    public void Dispose() => loggerProvider.Dispose();
}