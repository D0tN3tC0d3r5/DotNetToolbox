namespace DotNetToolbox.TestUtilities.Logging;

public interface ITrackedLoggerFactory
    : ILoggerFactory {
    IReadOnlyDictionary<string, ILogger> Loggers { get; }
}
