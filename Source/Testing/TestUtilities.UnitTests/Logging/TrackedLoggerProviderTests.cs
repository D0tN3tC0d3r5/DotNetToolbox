namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedLoggerProviderTests : IDisposable {
    private readonly ILoggerProvider _loggerProvider;
    private readonly TrackedLoggerProvider _trackedLoggerProvider;

    public TrackedLoggerProviderTests() {
        _loggerProvider = Substitute.For<ILoggerProvider>();
        _trackedLoggerProvider = new(_loggerProvider);
    }

    public void Dispose() {
        _loggerProvider.Dispose();
        _trackedLoggerProvider.Dispose();
    }

    [Fact]
    public void CreateLogger_ReturnsTrackedLoggerInstance() {
        // Arrange
        const string categoryName = "TestCategory";
        var logger = Substitute.For<ILogger>();
        _loggerProvider.CreateLogger(Arg.Any<string>()).Returns(logger);

        // Act
        var trackedLogger = _trackedLoggerProvider.CreateLogger(categoryName);

        // Assert
        trackedLogger.Should().BeOfType<TrackedLogger>();
        _loggerProvider.Received(1).CreateLogger(Arg.Is<string>(s => s == categoryName));
    }

    [Fact]
    public void Dispose_DisposesUnderlyingLoggerProvider() {
        // Act
        _trackedLoggerProvider.Dispose();

        // Assert
        _loggerProvider.Received(1).Dispose();
    }
}
