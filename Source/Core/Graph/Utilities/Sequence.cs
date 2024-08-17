namespace DotNetToolbox.Graph.Utilities;

public abstract class Sequence<TSequence, TValue>
    : ISequence<TValue>
    where TSequence : Sequence<TSequence, TValue> {
    private static readonly ConcurrentDictionary<string, TSequence> _providers = [];
    private bool _disposed;
    private bool _doneOnce;

    public static TSequence Shared
        => _providers.GetOrAdd(string.Empty, _ => InstanceFactory.Create<TSequence>());

    public static TSequence Keyed(string key)
        => _providers.GetOrAdd(IsNotNullOrWhiteSpace(key), _ => InstanceFactory.Create<TSequence>());

    protected Sequence(TValue first) {
        First = IsNotNull(first);
        Reset();
    }

    [NotNull]
    public required TValue First { get; init; }

    [NotNull]
    public TValue Current { get; protected set; }

    [NotNull]
    public TValue? Next {
        get => MoveNext()
                ? Current
                : throw new InvalidOperationException("The sequence has no more elements.");
        set => SetNext(value);
    }
    [NotNull]
    object IEnumerator.Current => Current;

    public bool HasNext() => !_doneOnce || TryGenerateNext(out _);
    public TValue PeekNext()
        => !_doneOnce
            ? Current
            : TryGenerateNext(out var next)
                ? next
                : throw new InvalidOperationException("The sequence has no more elements.");

    [MemberNotNull(nameof(Current))]
    public void Reset()
        => Current = First;

    [MemberNotNullWhen(true, nameof(Current))]
    public bool MoveNext() {
        if (!_doneOnce) {
#pragma warning disable CS8775 // Member must have a non-null value when exiting in some condition.
            return true;
#pragma warning restore CS8775
        }
        if (TryGenerateNext(out var next)) {
            Current = next;
            return true;
        }
        return false;
    }

    protected abstract void SetNext(TValue? value);
    protected abstract bool TryGenerateNext([NotNullWhen(true)] out TValue next);

    protected virtual void Dispose(bool disposing) {
        if (_disposed) return;
        if (disposing) {
            if (First is IDisposable disposableFirst) disposableFirst.Dispose();
            if (Current is IDisposable disposableCurrent) disposableCurrent.Dispose();
            _providers.TryRemove(string.Empty, out _);
            _providers.Clear();
            _disposed = true;
        }
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
