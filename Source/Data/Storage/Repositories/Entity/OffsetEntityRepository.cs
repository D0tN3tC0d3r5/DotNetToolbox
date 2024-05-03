using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Repositories.Entity;

public class OffsetEntityRepository<TItem, TKey, TKeyHandler>
    : OffsetEntityRepository<IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>, TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    public OffsetEntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) { }
    public OffsetEntityRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public OffsetEntityRepository(IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public OffsetEntityRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) { }
    public OffsetEntityRepository(string name, IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base(name, (IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public OffsetEntityRepository(string name, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class OffsetEntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    : EntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    , IOffsetQueryableRepository<TItem>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    protected OffsetEntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    protected OffsetEntityRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }

    #region Blocking

    public Block<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize)
        => Strategy.GetBlock(isNotStart, blockSize);

    #endregion

    #region Async

    public ValueTask<Block<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetBlockAsync(findStart, blockSize, ct);

    #endregion
}
