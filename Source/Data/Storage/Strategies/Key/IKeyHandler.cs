namespace DotNetToolbox.Data.Strategies.Key;

public interface IKeyHandler<TKey>
    : IEqualityComparer<TKey> {
    bool IsInUse(TKey key);
    TKey GetNext(TKey candidate);
    Task<bool> IsInUseAsync(TKey key, CancellationToken ct = default);
    Task<TKey> GetNextAsync(TKey candidate, CancellationToken ct = default);
}
