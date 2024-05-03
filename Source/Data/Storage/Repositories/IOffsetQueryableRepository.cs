namespace DotNetToolbox.Data.Repositories;

public interface IOffsetQueryableRepository<TItem>
    : IQueryableRepository<TItem> {

    #region Blocking

    Block<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize);

    #endregion

    #region Async

    ValueTask<Block<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = DefaultBlockSize, CancellationToken ct = default);

    #endregion
}
