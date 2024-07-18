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
        _trackedLoggerAssertions.Invoking(x => x.NotHave().Logs().WithLevel(LogLevel.Debug)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have().Logs().WithLevel(LogLevel.Debug)).Should().Throw<Exception>();
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
        _trackedLoggerAssertions.Invoking(x => x.Have(2).Logs).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(3).Logs).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(4).Logs).Should().Throw<Exception>();
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
        _trackedLoggerAssertions.Invoking(x => x.Have(2)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Have(3)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Have(4)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(0, LogLevel.Debug)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Error)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(2, LogLevel.Information)).Should().NotThrow();
    }

    [Fact]
    public void HaveAtLeast_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.ContainAtLeast(2)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainAtLeast(3)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainAtLeast(4)).Should().Throw<Exception>();
    }

    [Fact]
    public void HaveAtMost_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.ContainAtMost(2)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainAtMost(3)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainAtMost(4)).Should().NotThrow();
    }

    [Fact]
    public void Contain_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Trace)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Debug)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Information)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Warning)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Error)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Critical)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain(null!)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain("Test 1.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Contain("Test 2.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Contain("Test 3.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Debug, "Test 1.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Information, "Test 1.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Error, "Test 2.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.Contain(LogLevel.Error, "Test 1.")).Should().Throw<Exception>();
    }

    [Fact]
    public void NotContain_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Trace)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Debug)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Information)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Warning)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Error)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Critical)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(null)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain("Test 1.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotContain("Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotContain("Test 3.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Debug, "Test 1.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Information, "Test 1.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Error, "Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.NotContain(LogLevel.Error, "Test 1.")).Should().NotThrow();
    }

    [Fact]
    public void ContainExactly_ThrowsOnlyWhenFails() {
        // Arrange
        _trackedLogger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _trackedLogger.LogInformation("Test 1.");
        _trackedLogger.LogInformation("Test 2.");
        _trackedLogger.LogError("Test 2.");

        // Act & Assert
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(0, LogLevel.Trace)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Debug)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Information)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(2, LogLevel.Information)).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(3, LogLevel.Information)).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(0, "Test 4.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(1, "Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(2, "Test 2.")).Should().NotThrow();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(3, "Test 2.")).Should().Throw<Exception>();
        _trackedLoggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Information, "Test 1.")).Should().NotThrow();
    }
}
