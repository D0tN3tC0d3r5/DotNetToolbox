namespace DotNetToolbox.Data.InMemory;

public class InMemoryEntityRepositoryStrategy<TItem, TKey>
    : InMemoryValueObjectRepositoryStrategy<TItem>
    , IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
#region Blocking

    public TKey GetNextKey(IReadOnlyDictionary<object, object?>? keyContext = null) => throw new NotImplementedException();

    public void Patch(TKey key, Action<TItem> setItem) => throw new NotImplementedException();

    public void Remove(TKey key) => throw new NotImplementedException();

#endregion

#region Async

    public Task<TKey> GetNextKeyAsync(IReadOnlyDictionary<object, object?>? keyContext = null) => throw new NotImplementedException();

    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();

    public Task RemoveAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

#endregion
}
