namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedLoggerTests {
    private readonly TrackedLogger<TrackedLoggerTests> _trackedLogger;
    private readonly ILogger<TrackedLoggerTests> _logger;

    private static string Formatter(object s, Exception? _) => s.ToString() ?? string.Empty;

    public TrackedLoggerTests() {
        _logger = Substitute.For<ILogger<TrackedLoggerTests>>();
        _trackedLogger = new(_logger);
    }

    [Fact]
    public void Log_WhenEnabled_AddsLogToList() {
        // Arrange
        const LogLevel logLevel = LogLevel.Information;
        var eventId = new EventId(1, "TestEvent");
        const string state = "TestState";
        var exception = new Exception("TestException");
        _logger.IsEnabled(Arg.Any<LogLevel>()).Returns(true);

        // Act
        _trackedLogger.Log(logLevel, eventId, state, exception, Formatter);

        // Assert
        _trackedLogger.Logs.Should().ContainSingle();
        var log = _trackedLogger.Logs[0];
        log.Level.Should().Be(logLevel);
        log.EventId.Should().Be(eventId);
        log.State.Should().Be(state);
        log.Exception.Should().Be(exception);
        log.Message.Should().Be(state);
    }

    [Fact]
    public void Log_WhenDisabled_AndNotTrackingAll_DoesNotAddLogToList() {
        // Arrange
        const LogLevel logLevel = LogLevel.Information;
        var eventId = new EventId(1, "TestEvent");
        const string state = "TestState";
        var exception = new Exception("TestException");
        _logger.IsEnabled(Arg.Any<LogLevel>()).Returns(false);

        // Act
        _trackedLogger.Log(logLevel, eventId, state, exception, Formatter);

        // Assert
        _trackedLogger.Logs.Should().BeEmpty();
    }

    [Fact]
    public void Log_WhenDisabled_ButTrackingAll_DoesNotAddLogToList() {
        // Arrange
        var trackedLogger = new TrackedLogger(_logger, true);
        const LogLevel logLevel = LogLevel.Information;
        var eventId = new EventId(1, "TestEvent");
        const string state = "TestState";
        var exception = new Exception("TestException");
        _logger.IsEnabled(Arg.Any<LogLevel>()).Returns(false);

        // Act
        trackedLogger.Log(logLevel, eventId, state, exception, Formatter);

        // Assert
        trackedLogger.Logs.Should().ContainSingle();
    }

    [Fact]
    public void Log_WhenPaused_DoesNotAddLogToList() {
        // Arrange
        var trackedLogger = new TrackedLogger(_logger, trackAllLevels: true, startsImmediately: false);

        // Act
        trackedLogger.LogInformation("Some log 1.");
        trackedLogger.LogInformation("Some log 2.");
        trackedLogger.StartTracking();
        trackedLogger.LogInformation("Some log 3.");
        trackedLogger.LogInformation("Some log 4.");
        trackedLogger.StopTracking();
        trackedLogger.LogInformation("Some log 5.");

        // Assert
        trackedLogger.Logs.Should().HaveCount(2);
        trackedLogger.Logs[0].Message.Should().Be("Some log 3.");
        trackedLogger.Logs[1].Message.Should().Be("Some log 4.");
    }

    [Fact]
    public void Clear_ClearsLogsList() {
        // Arrange
        _trackedLogger.LogInformation("Some log 1.");

        // Act
        _trackedLogger.Clear();

        // Assert
        _trackedLogger.Logs.Should().BeEmpty();
    }

    [Theory]
    [InlineData(LogLevel.Trace, false)]
    [InlineData(LogLevel.Debug, false)]
    [InlineData(LogLevel.Information, false)]
    [InlineData(LogLevel.Warning, true)]
    [InlineData(LogLevel.Error, true)]
    [InlineData(LogLevel.Critical, true)]
    [InlineData(LogLevel.None, true)]
    public void IsEnabled_WhenLevelNotBellowMinimumLevel_ReturnsTrue(LogLevel level, bool expectedResult) {
        // Arrange & Act
        _logger.IsEnabled(Arg.Any<LogLevel>()).Returns(ci => ci.ArgAt<LogLevel>(0) >= LogLevel.Warning);
        var isEnabled = _trackedLogger.IsEnabled(level);

        // Assert
        isEnabled.Should().Be(expectedResult);
    }

    [Fact]
    public void BeginScope_WhenStateIsDisposable_ReturnsState() {
        // Arrange
        var originalScope = Substitute.For<IDisposable>();
        _logger.BeginScope(Arg.Any<object>()).Returns(originalScope);

        // Act
        var scope = _trackedLogger.BeginScope<object>(new());

        // Assert
        scope.Should().BeSameAs(originalScope);
    }

    [Fact]
    public void BeginScope_WhenStateIsNotDisposable_ReturnsNull() {
        // Arrange
        var state = new object();
        _logger.BeginScope(Arg.Any<object>()).Returns(default(IDisposable));

        // Act
        var scope = _trackedLogger.BeginScope(state);

        // Assert
        scope.Should().BeNull();
    }
}
