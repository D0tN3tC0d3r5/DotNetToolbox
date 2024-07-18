namespace DotNetToolbox.TestUtilities.Logging;

public interface ITrackedLogger
    : ILogger {
    IReadOnlyList<Log> Logs { get; }

    void Clear();
    void StopTracking();
    void StartTracking();
}
