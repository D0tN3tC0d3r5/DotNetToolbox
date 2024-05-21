namespace DotNetToolbox;

public class LoggerExtensionsTests {
    [Fact]
    public void Should_ForNotTrackedLogger_Throws() {
        // Arrange
        ILogger logger = default!;

        // Act
        var action = () => logger.Should();

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_ForTrackedLogger_DoesNotThrows() {
        // Arrange
        ILogger logger = new TrackedNullLogger();

        // Act
        var action = () => logger.Should();

        // Assert
        action.Should().NotThrow();
    }
}
