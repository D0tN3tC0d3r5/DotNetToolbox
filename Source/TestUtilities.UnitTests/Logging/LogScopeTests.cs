namespace DotNetToolbox.TestUtilities.Logging;

public class LogScopeTests {
    [Fact]
    public void Dispose_StateIsDisposable_DisposeCalled() {
        // Arrange
        var disposableState = new DisposableState();
        var logScope = new MockedDisposable<DisposableState>(disposableState);

        // Act
        logScope.Dispose();

        // Assert
        disposableState.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void Dispose_StateIsNotDisposable() {
        // Arrange
        var logScope = new MockedDisposable<string>("Some value");

        // Act
        var action = logScope.Dispose;

        // Assert
        action.Should().NotThrow();
    }

    private class DisposableState : IDisposable {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }
}
