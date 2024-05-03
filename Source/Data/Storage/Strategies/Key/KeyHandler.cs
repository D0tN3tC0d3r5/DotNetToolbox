namespace DotNetToolbox.Data.Strategies.Key;

public class KeyHandler {
    private static readonly ConcurrentDictionary<string, object?> _context = new();
    protected ConcurrentDictionary<string, object?> Context => _context;
    private static readonly object _lock = new();
    protected object Lock => _lock;
}

public class KeyHandler<TKey>
    : KeyHandler,
      IKeyHandler<TKey> {

    private readonly IEqualityComparer<TKey> _comparer;
    public static KeyHandler<TKey> Default { get; } = new();

    protected KeyHandler(IEqualityComparer<TKey>? comparer = null) {
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    public virtual TKey GetNext(string contextKey, TKey proposedKey)
        => proposedKey;
    public virtual Task<TKey> GetNextAsync(string contextKey, TKey proposedKey, CancellationToken ct = default)
        => Task.FromResult(proposedKey);

    public bool Equals(TKey? x, TKey? y) => _comparer.Equals(x, y);
    public int GetHashCode(TKey obj) => obj is null ? 0 : _comparer.GetHashCode(obj);
}
