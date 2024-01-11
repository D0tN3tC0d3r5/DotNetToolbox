namespace DotNetToolbox.TestUtilities.Logging;

public class TrackedNullLoggerTests {
    [Fact]
    public void Constructor_ForGeneric_CreatesLog() {
        // Arrange & Act
        var logger = new TrackedNullLogger<TrackedNullLoggerTests>();

        // Assert
        logger.Should().BeAssignableTo<ILogger<TrackedNullLoggerTests>>();
        logger.MinimumLevel.Should().Be(LogLevel.Trace);
    }

    [Fact]
    public void Instance_ForGeneric_CreatesLog() {
        // Arrange & Act
        var logger = TrackedNullLogger<TrackedNullLoggerTests>.Instance;

        // Assert
        logger.Should().BeAssignableTo<ILogger<TrackedNullLoggerTests>>();
        logger.MinimumLevel.Should().Be(LogLevel.Trace);
    }

    [Fact]
    public void Instance_CreatesLog() {
        // Arrange & Act
        var logger = TrackedNullLogger.Instance;

        // Assert
        logger.Should().BeAssignableTo<ILogger>();
        logger.MinimumLevel.Should().Be(LogLevel.Trace);
    }

    [Theory]
    [InlineData(LogLevel.Trace, false)]
    [InlineData(LogLevel.Debug, false)]
    [InlineData(LogLevel.Information, false)]
    [InlineData(LogLevel.Warning, true)]
    [InlineData(LogLevel.Error, true)]
    [InlineData(LogLevel.Critical, true)]
    [InlineData(LogLevel.None, true)]
    public void IsEnabled_ForGeneric_ReturnsTrue_IfAboveOrEqualMinimum(LogLevel level, bool expectedResult) {
        // Arrange
        var logger = new TrackedNullLogger<TrackedNullLoggerTests>(LogLevel.Warning);

        // Act
        var isEnabled = logger.IsEnabled(level);

        // Assert
        isEnabled.Should().Be(expectedResult);
    }
}
