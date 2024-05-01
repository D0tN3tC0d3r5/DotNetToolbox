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
        : base(strategy, data ?? []) {
    }
}

public abstract class EntityRepository<TStrategy, TItem, TKey>
    : ValueObjectRepository<TItem>
    , IEntityRepository<TItem, TKey>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected EntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }

    #region Blocking

    public IReadOnlyList<TItem> GetAll() => throw new NotImplementedException();

    public TItem FindByKey(TKey key) => throw new NotImplementedException();

    public TKey GetNextKey(IReadOnlyDictionary<object, object?>? keyContext = null) => throw new NotImplementedException();

    public void Patch(TKey key, Action<TItem> setItem) => throw new NotImplementedException();

    public void Update(TItem updatedItem) => throw new NotImplementedException();

    public void Remove(TKey key) => throw new NotImplementedException();

    #endregion

    #region Async

    public Task<IReadOnlyList<TItem>> GetAllAsync(CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TKey> GetNextKeyAsync(IReadOnlyDictionary<object, object?>? keyContext = null) => throw new NotImplementedException();

    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();

    public Task UpdateAsync(TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();

    public Task RemoveAsync(TKey key, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
