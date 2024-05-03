namespace DotNetToolbox.Data.Strategies.Key;

public interface IKeyHandler<TKey>
    : IEqualityComparer<TKey> {
    TKey GetNext(string contextKey, TKey proposedKey);
    Task<TKey> GetNextAsync(string contextKey, TKey proposedKey, CancellationToken ct = default);
}
