namespace DotNetToolbox.TestUtilities.Logging;

public interface ITrackedLogger : ILogger {
    IReadOnlyList<Log> Logs { get; }
    bool IsTrackingAllLevel { get; }
    bool IsTracking { get; }

    void Clear();
    void StartTracking();
    void StopTracking();
}
