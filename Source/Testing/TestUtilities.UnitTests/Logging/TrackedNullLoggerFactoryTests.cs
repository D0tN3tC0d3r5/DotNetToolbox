namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedNullLoggerFactoryTests : IDisposable {
    private readonly TrackedNullLoggerFactory _trackedLoggerFactory = TrackedNullLoggerFactory.Instance;

    public void Dispose()
        => _trackedLoggerFactory.Dispose();

    [Fact]
    public void AddProvider_DoesNotThrows() {
        // Arrange
        var provider = Substitute.For<ILoggerProvider>();

        // Act
        var action = () => _trackedLoggerFactory.AddProvider(provider);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void CreateLogger_ReturnsTrackedNullLoggerInstance() {
        // Arrange
        const string categoryName = "TestCategory";

        // Act
        var trackedLogger = _trackedLoggerFactory.CreateLogger(categoryName);

        // Assert
        trackedLogger.Should().BeOfType<TrackedNullLogger>();
    }

    [Fact]
    public void Dispose_DisposesUnderlyingLoggerFactory() {
        // Arrange & Act
        var action = _trackedLoggerFactory.Dispose;

        // Assert
        action.Should().NotThrow();
    }
}
