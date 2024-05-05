namespace DotNetToolbox.Data.Repositories;

public interface IChunkedRepository<TItem>
    : IQueryableRepository<TItem> {

    #region Blocking

    Chunk<TItem> GetFirstChunk(uint blockSize = DefaultBlockSize);
    Chunk<TItem> GetNextChunk(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize);

    #endregion

    #region Async

    ValueTask<Chunk<TItem>> GetFirstChunkAsync(uint blockSize = DefaultBlockSize, CancellationToken ct = default);
    ValueTask<Chunk<TItem>> GetNextChunkAsync(Expression<Func<TItem, bool>> isChunkStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default);

    #endregion
}
