namespace DotNetToolbox.Data.Repositories.Entity;

public interface IUpdatableEntityRepository<TItem, TKey>
    : IUpdatableValueObjectRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    #region Blocking

    TKey GetNextKey(IReadOnlyDictionary<object, object?>? keyContext = null);
    void Patch(TKey key, Action<TItem> setItem);
    void Update(TItem updatedItem);
    void Remove(TKey key);

    #endregion

    #region Async

    Task<TKey> GetNextKeyAsync(IReadOnlyDictionary<object, object?>? keyContext = null, CancellationToken ct = default);
    Task UpdateAsync(TItem updatedItem, CancellationToken ct = default);
    Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default);
    Task RemoveAsync(TKey key, CancellationToken ct = default);

    #endregion
}
