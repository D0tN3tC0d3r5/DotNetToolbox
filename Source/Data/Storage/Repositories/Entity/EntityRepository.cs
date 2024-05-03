using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Repositories.Entity;

public class EntityRepository<TItem, TKey, TKeyHandler>
    : EntityRepository<IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>, TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    public EntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) { }
    public EntityRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public EntityRepository(IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public EntityRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) { }
    public EntityRepository(string name, IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base(name, (IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public EntityRepository(string name, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class EntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    : ValueObjectRepository<IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>, TItem>
    , IEntityRepository<TItem, TKey, TKeyHandler>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {

    protected EntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null, TKeyHandler? keyHandler = null)
        : base(strategy, data) {
        Strategy.KeyHandler = keyHandler ?? TKeyHandler.Default;
    }
    protected EntityRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null, TKeyHandler? keyHandler = null)
        : base(name, strategy, data) {
        Strategy.KeyHandler = keyHandler ?? TKeyHandler.Default;
    }

    #region Blocking

    public TItem? FindByKey(TKey key)
        => Strategy.FindByKey(key);
    public void Update(TItem updatedItem)
        => Strategy.Update(updatedItem);
    public void Patch(TKey key, Action<TItem> setItem)
        => Strategy.Patch(key, setItem);
    public void Remove(TKey key)
        => Strategy.Remove(key);

    #endregion

    #region Async

    public ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default)
        => Strategy.FindByKeyAsync(key, ct);
    public Task UpdateAsync(TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(updatedItem, ct);
    public Task PatchAsync(TKey key, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default)
        => Strategy.PatchAsync(key, setItem, ct);
    public Task RemoveAsync(TKey key, CancellationToken ct = default)
        => Strategy.RemoveAsync(key, ct);

    #endregion
}
