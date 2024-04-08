namespace DotNetToolbox.ConsoleApplication.TestDoubles;

public sealed class TestChangeToken
    : IChangeToken {
    public IDisposable RegisterChangeCallback(Action<object?> callback, object? state)
        => new DisposableStateHolder(state);

    public bool HasChanged => false;
    public bool ActiveChangeCallbacks => false;
}
