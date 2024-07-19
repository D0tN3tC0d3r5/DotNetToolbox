namespace DotNetToolbox;

public class TrackedLoggerAssertionsTests {
    private readonly ILogger _trackedLogger;
    private readonly TrackedLoggerAssertions _trackedLoggerAssertions;

    public TrackedLoggerAssertionsTests() {
        var logger = Substitute.For<ILogger>();
        logger.IsEnabled(Arg.Any<LogLevel>()).Returns(x => x.Arg<LogLevel>() >= LogLevel.Information);
        _trackedLogger = new TrackedLogger(logger);
        _trackedLoggerAssertions = new(_trackedLogger);
    }

    [Fact]
    public void BeEmpty_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Debug)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Debug)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHaveBeenCalled()).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.HaveBeenCalled()).Should().Throw<Exception>();
    }

    [Fact]
    public void NotBeEmpty_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.Have(2).Logs()).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(3).Logs()).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(4).Logs()).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHaveBeenCalled()).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.HaveBeenCalled()).Should().NotThrow();
    }

    [Fact]
    public void HaveExactly_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.Have(2).Logs()).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(3).Logs()).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(4).Logs()).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Debug)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.HaveSingle().LogWith(LogLevel.Error)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(1).LogsWith(LogLevel.Error)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(2).LogsWith(LogLevel.Information)).Should().NotThrow();
    }

    [Fact]
    public void HaveAtLeast_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.HaveAtLeast(2).Logs()).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.HaveAtLeast(3).Logs()).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.HaveAtLeast(4).Logs()).Should().Throw<Exception>();
    }

    [Fact]
    public void HaveAtMost_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.HaveAtMost(2).Logs()).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.HaveAtMost(3).Logs()).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.HaveAtMost(4).Logs()).Should().NotThrow();
    }

    [Fact]
    public void Contain_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Trace)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Debug)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Information)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Warning)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Error)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Critical)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(null!)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith("Test 1.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith("Test 2.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith("Test 3.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Debug, "Test 1.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Information, "Test 1.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Error, "Test 2.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().LogsWith(LogLevel.Error, "Test 1.")).Should().Throw<Exception>();
    }

    [Fact]
    public void NotContain_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Trace)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Debug)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Information)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Warning)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Error)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Critical)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith("Test 1.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith("Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith("Test 3.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Debug, "Test 1.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Information, "Test 1.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Error, "Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotHave().LogsWith(LogLevel.Error, "Test 1.")).Should().NotThrow();
    }

    [Fact]
    public void ContainExactly_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.Have(0).LogsWith(LogLevel.Trace)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(1).LogsWith(LogLevel.Debug)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(1).LogsWith(LogLevel.Information)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(2).LogsWith(LogLevel.Information)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(3).LogsWith(LogLevel.Information)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(0).LogsWith("Test 4.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(1).LogsWith("Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(2).LogsWith("Test 2.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(3).LogsWith("Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(1).LogsWith(LogLevel.Information, "Test 1.")).Should().NotThrow();
    }
}
