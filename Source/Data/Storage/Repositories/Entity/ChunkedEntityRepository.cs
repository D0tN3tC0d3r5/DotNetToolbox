using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Repositories.Entity;

public class ChunkedEntityRepository<TItem, TKey, TKeyHandler>
    : ChunkedEntityRepository<IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>, TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    public ChunkedEntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) {
    }
    public ChunkedEntityRepository(IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public ChunkedEntityRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) {
    }
    public ChunkedEntityRepository(string name, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class ChunkedEntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    : EntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    , IChunkedQueryableRepository<TItem>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    protected ChunkedEntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    protected ChunkedEntityRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }

    #region Blocking

    public Chunk<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize)
        => Strategy.GetBlock(isNotStart, blockSize);

    #endregion

    #region Async

    public ValueTask<Chunk<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetBlockAsync(findStart, blockSize, ct);

    #endregion
}
