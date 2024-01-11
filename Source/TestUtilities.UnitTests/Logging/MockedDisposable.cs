namespace DotNetToolbox.TestUtilities.Logging;

internal sealed class MockedDisposable<TState>(TState state) : IDisposable
    where TState : notnull {
    public TState State { get; } = state;

    public void Dispose() {
        if (State is IDisposable disposable)
            disposable.Dispose();
    }
}
