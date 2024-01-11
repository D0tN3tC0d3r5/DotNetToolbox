namespace DotNetToolbox;

public class LoggerAssertionsTests {
    private readonly ITrackedLogger _logger;
    private readonly LoggerAssertions _loggerAssertions;

    public LoggerAssertionsTests() {
        _logger = new TrackedNullLogger(LogLevel.Information);
        _loggerAssertions = new(_logger);
    }

    [Fact]
    public void BeEmpty_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information

        // Act & Assert
        _loggerAssertions.Invoking(x => x.BeEmpty()).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainExactly(0)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotBeEmpty()).Should().Throw<Exception>();
    }

    [Fact]
    public void NotBeEmpty_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.BeEmpty()).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(0)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotBeEmpty()).Should().NotThrow();
    }

    [Fact]
    public void HaveExactly_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.ContainExactly(2)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(3)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainExactly(4)).Should().Throw<Exception>();
    }

    [Fact]
    public void HaveAtLeast_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.ContainAtLeast(2)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainAtLeast(3)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainAtLeast(4)).Should().Throw<Exception>();
    }

    [Fact]
    public void HaveAtMost_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.ContainAtMost(2)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainAtMost(3)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainAtMost(4)).Should().NotThrow();
    }

    [Fact]
    public void Contain_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Trace)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Debug)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Information)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Warning)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Error)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Critical)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain(null!)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain("Test 1.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.Contain("Test 2.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.Contain("Test 3.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Debug, "Test 1.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Information, "Test 1.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Error, "Test 2.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.Contain(LogLevel.Error, "Test 1.")).Should().Throw<Exception>();
    }

    [Fact]
    public void NotContain_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Trace)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Debug)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Information)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Warning)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Error)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Critical)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain(null!)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain("Test 1.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotContain("Test 2.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotContain("Test 3.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Debug, "Test 1.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Information, "Test 1.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Error, "Test 2.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.NotContain(LogLevel.Error, "Test 1.")).Should().NotThrow();
    }


    [Fact]
    public void ContainExactly_ThrowsOnlyWhenFails() {
        // Arrange
        _logger.LogDebug("Test 1."); // Not added, because MinimumLevel is Information
        _logger.LogInformation("Test 1.");
        _logger.LogInformation("Test 2.");
        _logger.LogError("Test 2.");

        // Act & Assert
        _loggerAssertions.Invoking(x => x.ContainExactly(0, LogLevel.Trace)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Debug)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Information)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(2, LogLevel.Information)).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainExactly(3, LogLevel.Information)).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(0, "Test 4.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainExactly(1, "Test 2.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(2, "Test 2.")).Should().NotThrow();
        _loggerAssertions.Invoking(x => x.ContainExactly(3, "Test 2.")).Should().Throw<Exception>();
        _loggerAssertions.Invoking(x => x.ContainExactly(1, LogLevel.Information, "Test 1.")).Should().NotThrow();
    }
}
