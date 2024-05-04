namespace DotNetToolbox.Data.Repositories;

public class ChunkedRepository<TItem>
    : ChunkedRepository<IRepositoryStrategy<TItem>, TItem> {
    public ChunkedRepository(IEnumerable<TItem>? data = null)
        : base(DefaultName, new InMemoryRepositoryStrategy<TItem>(), data) {
    }
    public ChunkedRepository(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public ChunkedRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryRepositoryStrategy<TItem>(), data) {
    }
    public ChunkedRepository(string name, IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class ChunkedRepository<TStrategy, TItem>
    : Repository<TItem>
    , IChunkedQueryableRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    protected ChunkedRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    protected ChunkedRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
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
