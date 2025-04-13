namespace DotNetToolbox.Sequences;

/// <summary>
///     Generates a value based on a context asynchronously.
/// </summary>
public abstract class AsyncSequence<TValue, TContext>(TContext? context = null)
    : IAsyncEnumerator<TValue>
    where TContext : class {
    private bool _disposed;
    protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        if (_disposed) return;
        if (context is IDisposable disposableContext) disposableContext.Dispose();
        if (context is IAsyncDisposable asyncDisposableContext) await asyncDisposableContext.DisposeAsync();
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    protected abstract ValueTask<Result<TValue>> TryGetNextAsync();

    /// <inheritdoc />
    public async ValueTask<bool> MoveNextAsync() {
        var result = await TryGetNextAsync();
        if (!result.IsSuccessful) return false;
        Current = result.Value;
        return true;
    }

    /// <inheritdoc />
    public TValue Current { get; private set; } = default!;
}
