namespace DotNetToolbox.Sequences;

/// <summary>
///     Generates a value based on a context.
/// </summary>
public abstract class Sequence<TValue>
    : IEnumerator<TValue> {
    private bool _disposed;
    protected virtual void Dispose(bool disposing) { }

    public void Dispose() {
        if (_disposed) return;
        Dispose(true);
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    protected abstract bool TryGetNext([NotNullWhen(true)]out TValue? next);

    object IEnumerator.Current => Current!;

    /// <inheritdoc />
    public TValue Current { get; private set; } = default!;

    /// <inheritdoc />
    public bool MoveNext() {
        if (!TryGetNext(out var next)) return false;
        Current = next;
        return true;
    }

    /// <inheritdoc />
    public virtual void Reset() { }
}

/// <summary>
///     Generates a value based on a context.
/// </summary>
public abstract class Sequence<TValue, TContext>(TContext? context = null)
    : Sequence<TValue>
    where TContext : class {
    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        if (!disposing) return;
        if (context is IDisposable disposableContext) disposableContext.Dispose();
    }
}
