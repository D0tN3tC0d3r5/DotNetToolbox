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

    public IReadOnlyList<int> GetAllowedBlockSizes()
        => Strategy.GetAllowedBlockSizes();
    public IBlock<TItem, TOffsetMarker> GetBlock<TOffsetMarker>(uint blockSize, TOffsetMarker? marker = default)
        => Strategy.GetBlock(blockSize, marker);

    #endregion

    #region Async

    public Task<IBlock<TItem, TOffsetMarker>> GetBlockAsync<TOffsetMarker>(uint blockSize, TOffsetMarker? marker = default, CancellationToken ct = default)
        => Strategy.GetBlockAsync(blockSize, marker, ct);

    #endregion
}
