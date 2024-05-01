namespace DotNetToolbox.Data.Repositories.Entity;

public class EntityRepository<TItem, TKey>
    : EntityRepository<IEntityRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public EntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey>(), data) { }
    public EntityRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IEntityRepositoryStrategy<TItem, TKey>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public EntityRepository(IEntityRepositoryStrategy<TItem, TKey> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
}

public abstract class EntityRepository<TStrategy, TItem, TKey>
    : ValueObjectRepository<IEntityRepositoryStrategy<TItem, TKey>, TItem>
    , IEntityRepository<TItem, TKey>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected EntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }

    #region Blocking

    public TItem? FindByKey(TKey key)
        => Strategy.FindByKey(key);
    public TKey GetNextKey(IReadOnlyDictionary<object, object?>? keyContext = null)
        => Strategy.GetNextKey(keyContext);
    public void Update(TItem updatedItem)
        => Strategy.Update(updatedItem);
    public void Patch(TKey key, Action<TItem> setItem)
        => Strategy.Patch(key, setItem);
    public void Remove(TKey key)
        => Strategy.Remove(key);

    #endregion

    #region Async

    public Task<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Strategy.FindByKeyAsync(key, ct);
    public Task<TKey> GetNextKeyAsync(IReadOnlyDictionary<object, object?>? keyContext = null, CancellationToken ct = default)
        => Strategy.GetNextKeyAsync(keyContext, ct);
    public Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(updatedItem, ct);
    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.PatchAsync(key, setItem, ct);
    public Task RemoveAsync(TKey key, CancellationToken ct = default)
        => Strategy.RemoveAsync(key, ct);

    #endregion
}
