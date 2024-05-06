namespace DotNetToolbox.Data.Strategies.Key;

public abstract class KeyHandler<TKey>(IEqualityComparer<TKey>? comparer = null)
    : IKeyHandler<TKey> {

    private readonly IEqualityComparer<TKey> _comparer = comparer ?? EqualityComparer<TKey>.Default;

    public abstract bool IsInUse(TKey key);
    public abstract TKey GetNext(TKey candidate);
    public abstract Task<bool> IsInUseAsync(TKey key, CancellationToken ct = default);
    public abstract Task<TKey> GetNextAsync(TKey candidate, CancellationToken ct = default);

    public bool Equals(TKey? x, TKey? y) => _comparer.Equals(x, y);
    public int GetHashCode(TKey? obj) => obj is null ? 0 : _comparer.GetHashCode(obj);
}
