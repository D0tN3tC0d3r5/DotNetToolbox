namespace DotNetToolbox.Data.Repositories;

public interface IChunkedQueryableRepository<TItem>
    : IQueryableRepository<TItem> {

    #region Blocking

    Chunk<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize);

    #endregion

    #region Async

    ValueTask<Chunk<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default);

    #endregion
}
