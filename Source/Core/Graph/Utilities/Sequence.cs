namespace DotNetToolbox.Graph.Utilities;

public abstract class Sequence<TSequence, TValue>
    : ISequence<TValue>
    where TSequence : Sequence<TSequence, TValue>
    where TValue : notnull {
    private static readonly ConcurrentDictionary<string, TSequence> _providers = [];
    private bool _disposed;
    private bool _isFirstTime = true;

    public static TSequence Transient => InstanceFactory.Create<TSequence>();

    public static TSequence Singleton { get; } = InstanceFactory.Create<TSequence>();

    public static TSequence Keyed(string key)
        => _providers.GetOrAdd(IsNotNullOrWhiteSpace(key), _ => InstanceFactory.Create<TSequence>());

    protected Sequence(TValue first) {
        First = first;
        Current = First;
    }

    public TValue First { get; }

    public TValue Current { get; protected set; }

    public TValue Next {
        get => MoveNext()
                ? Current
                : throw new InvalidOperationException("The sequence has no more elements.");
        set => SetNext(value);
    }

    object IEnumerator.Current => Current;

    public bool HasNext() => !_isFirstTime || TryGenerateNext(out _);
    public TValue PeekNext()
        => !_isFirstTime
            ? Current
            : TryGenerateNext(out var next)
                ? next
                : throw new InvalidOperationException("The sequence has no more elements.");

    public void Reset()
        => Current = First;

    public bool MoveNext() {
        if (_isFirstTime) {
            Current = First;
            _isFirstTime = false;
            return true;
        }

        var nextGenerated = TryGenerateNext(out var next);
        Current = nextGenerated ? next : Current;
        return nextGenerated;
    }

    protected abstract void SetNext(TValue value);
    protected abstract bool TryGenerateNext([NotNullWhen(true)] out TValue next);

    protected virtual void Dispose(bool disposing) {
        if (_disposed) return;
        if (disposing) {
            if (First is IDisposable disposableFirst) disposableFirst.Dispose();
            if (Current is IDisposable disposableCurrent) disposableCurrent.Dispose();
            _providers.TryRemove(string.Empty, out _);
            _providers.Clear();
        }

        _disposed = true;
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
