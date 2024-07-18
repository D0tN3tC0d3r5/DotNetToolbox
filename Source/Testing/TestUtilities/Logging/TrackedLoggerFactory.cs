
namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedLoggerFactory(ILoggerFactory? loggerFactory = null)
    : ITrackedLoggerFactory {
    private readonly ILoggerFactory _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
    private readonly Dictionary<string, ILogger> _loggers = [];

    public IReadOnlyDictionary<string, ILogger> Loggers => _loggers.AsReadOnly();

    public void AddProvider(ILoggerProvider provider)
        => _loggerFactory.AddProvider(provider);

    public ILogger CreateLogger(string categoryName) {
        var logger = new TrackedLogger(_loggerFactory.CreateLogger(categoryName));
        _loggers[categoryName] = logger;
        return logger;
    }

    public void Dispose() {
        _loggerFactory.Dispose();
        _loggers.Clear();
    }
}
