namespace DotNetToolbox.TestUtilities.Logging;

public sealed class TrackedLoggerFactoryTests : IDisposable {
    private readonly ILoggerFactory _loggerFactory;
    private readonly TrackedLoggerFactory _trackedLoggerFactory;

    public TrackedLoggerFactoryTests() {
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _trackedLoggerFactory = new(_loggerFactory);
    }

    public void Dispose() {
        _loggerFactory.Dispose();
        _trackedLoggerFactory.Dispose();
    }

    [Fact]
    public void AddProvider_WrapsProviderWithTrackedLoggerProvider() {
        // Arrange
        var provider = Substitute.For<ILoggerProvider>();

        // Act
        _trackedLoggerFactory.AddProvider(provider);

        // Assert
        _loggerFactory.Received(1).AddProvider(Arg.Is<ILoggerProvider>(p => p is TrackedLoggerProvider));
    }

    [Fact]
    public void CreateLogger_ReturnsTrackedLoggerInstance() {
        // Arrange
        const string categoryName = "TestCategory";
        var logger = Substitute.For<ILogger>();
        _loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);

        // Act
        var trackedLogger = _trackedLoggerFactory.CreateLogger(categoryName);

        // Assert
        trackedLogger.Should().BeOfType<TrackedLogger>();
        _loggerFactory.Received(1).CreateLogger(Arg.Is<string>(s => s == categoryName));
    }

    [Fact]
    public void Dispose_DisposesUnderlyingLoggerFactory() {
        // Act
        _trackedLoggerFactory.Dispose();

        // Assert
        _loggerFactory.Received(1).Dispose();
    }
}
