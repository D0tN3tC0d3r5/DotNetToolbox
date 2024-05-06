namespace DotNetToolbox.Data.Strategies.Key;

public abstract class InMemoryKeyHandler<TKey>(string contextKey, IEqualityComparer<TKey>? comparer = null)
    : KeyHandler<TKey>(comparer)
    where TKey : notnull {
    protected string ContextKey { get; } = IsNotNull(contextKey);
    protected abstract TKey GenerateNewKey(TKey lastUsedKey);

    public override bool IsInUse(TKey key)
        => InMemoryKeys.IsInUse(ContextKey, key);
    public override TKey GetNext(TKey candidate)
        => InMemoryKeys.GetNext(ContextKey, candidate, GenerateNewKey);
    public override Task<bool> IsInUseAsync(TKey key, CancellationToken ct = default)
        => Task.FromResult(IsInUse(key));
    public override Task<TKey> GetNextAsync(TKey candidate, CancellationToken ct = default)
        => Task.FromResult(GetNext(candidate));
}
