namespace DotNetToolbox.Data.Repositories;

public interface IOffsetQueryableRepository<TItem>
    : IQueryableRepository<TItem> {

    #region Blocking

    IReadOnlyList<int> GetAllowedBlockSizes();
    IBlock<TItem, TOffsetMarker> GetBlock<TOffsetMarker>(uint blockSize, TOffsetMarker? marker = default);

    #endregion

    #region Async

    Task<IBlock<TItem, TOffsetMarker>> GetBlockAsync<TOffsetMarker>(uint blockSize, TOffsetMarker? marker = default, CancellationToken ct = default);

    #endregion
}
