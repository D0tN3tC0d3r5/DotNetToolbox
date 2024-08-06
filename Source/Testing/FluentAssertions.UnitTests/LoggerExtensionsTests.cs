namespace FluentAssertions;

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
        var loggerFactory = new TrackedLoggerFactory(Substitute.For<ILoggerFactory>());
        var logger = loggerFactory.CreateLogger("Test");

        // Act
        var action = () => logger.Should();

        // Assert
        action.Should().NotThrow();
    }
}
