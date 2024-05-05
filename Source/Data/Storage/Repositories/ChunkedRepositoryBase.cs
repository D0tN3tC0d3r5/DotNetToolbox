namespace DotNetToolbox.Data.Repositories;

public abstract class ChunkedRepositoryBase<TStrategy, TItem, TKey>
    : RepositoryBase<TStrategy, TItem, TKey>
    , IChunkedRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected ChunkedRepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) {
    }
    protected ChunkedRepositoryBase(string name, IEnumerable<TItem>? data = null)
        : base(name, data) {
    }

#region Blocking

    public Chunk<TItem> GetFirstChunk(uint blockSize = DefaultBlockSize)
        => Strategy.GetFirstChunk(blockSize);
    public Chunk<TItem> GetNextChunk(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize)
        => Strategy.GetNextChunk(isChunkStart, blockSize);

#endregion

#region Async

    public ValueTask<Chunk<TItem>> GetFirstChunkAsync(uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetFirstChunkAsync(blockSize, ct);
    public ValueTask<Chunk<TItem>> GetNextChunkAsync(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetNextChunkAsync(isChunkStart, blockSize, ct);

#endregion
}

public abstract class ChunkedRepositoryBase<TStrategy, TItem>
    : RepositoryBase<TStrategy, TItem>, IChunkedRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem>, new() {
    protected ChunkedRepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) { }

    protected ChunkedRepositoryBase(string name, IEnumerable<TItem>? data = null)
        : base(name, data) { }

#region Blocking

    public Chunk<TItem> GetFirstChunk(uint blockSize = DefaultBlockSize) => Strategy.GetFirstChunk(blockSize);

    public Chunk<TItem> GetNextChunk(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize)
        => Strategy.GetNextChunk(isChunkStart, blockSize);

#endregion

#region Async

    public ValueTask<Chunk<TItem>> GetFirstChunkAsync(uint blockSize = DefaultBlockSize, CancellationToken ct = default)
        => Strategy.GetFirstChunkAsync(blockSize, ct);

    public ValueTask<Chunk<TItem>> GetNextChunkAsync(Expression<Func<TItem, bool>> isChunkStart,
                                                     uint blockSize = DefaultBlockSize,
                                                     CancellationToken ct = default)
        => Strategy.GetNextChunkAsync(isChunkStart, blockSize, ct);

#endregion
}
