using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Data.Repositories.Entity;

public class OffsetEntityRepository<TItem, TKey>
    : OffsetEntityRepository<IEntityRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public OffsetEntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey>(), data) { }
    public OffsetEntityRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IEntityRepositoryStrategy<TItem, TKey>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public OffsetEntityRepository(IEntityRepositoryStrategy<TItem, TKey> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
}

public abstract class OffsetEntityRepository<TStrategy, TItem, TKey>
    : EntityRepository<TStrategy, TItem, TKey>
    , IOffsetQueryableRepository<TItem>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected OffsetEntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
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
