namespace DotNetToolbox.Data.InMemory;

public class InMemoryEntityRepositoryStrategy<TItem, TKey>
    : InMemoryValueObjectRepositoryStrategy<TItem>
    , IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    public void SetKeyComparer(IEqualityComparer<TKey> comparer)
        => KeyComparer = IsNotNull(comparer);
    protected IEqualityComparer<TKey> KeyComparer { get; private set; } = EqualityComparer<TKey>.Default;

    #region Blocking

    public TItem? FindByKey(TKey key)
        => Find(x => KeyComparer.Equals(x.Id, key));

    public TKey GetNextKey(IReadOnlyDictionary<object, object?>? keyContext = null)
        => throw new NotImplementedException();

    public void Update(TItem updatedItem)
        => Update(x => KeyComparer.Equals(x.Id, updatedItem.Id), updatedItem);

    public void Patch(TKey key, Action<TItem> setItem)
        => Patch(x => KeyComparer.Equals(x.Id, key), setItem);

    public void Remove(TKey key)
        => Remove(x => KeyComparer.Equals(x.Id, key));

#endregion

    #region Async

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => FindAsync(x => KeyComparer.Equals(x.Id, key), ct);

    public Task<TKey> GetNextKeyAsync(IReadOnlyDictionary<object, object?>? keyContext = null, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => UpdateAsync(x => KeyComparer.Equals(x.Id, updatedItem.Id), updatedItem, ct);

    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => PatchAsync(x => KeyComparer.Equals(x.Id, key), setItem, ct);

    public Task RemoveAsync(TKey key, CancellationToken ct = default)
        => RemoveAsync(x => KeyComparer.Equals(x.Id, key), ct);


    #endregion
}
