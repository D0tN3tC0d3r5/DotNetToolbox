namespace Sophia.Data;

public interface IJsonFileStorage<TData, in TKey>
    where TData : class, ISimpleKeyEntity<TData, TKey>, new() {
    void SetBasePath(string path);
    Task<IEnumerable<TData>> GetAllAsync(Func<TData, bool>? filter = null, CancellationToken ct = default);
    Task<TData?> GetByIdAsync(TKey id, CancellationToken ct = default);
    Task CreateAsync(TData data, CancellationToken ct = default);
    Task UpdateAsync(TData data, CancellationToken ct = default);
    bool Delete(TKey id);
}
