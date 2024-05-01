namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IOffsetQueryableValueObjectRepository<TItem, TOffsetMarker>
    : IQueryableValueObjectRepository<TItem> {

    #region Blocking

    IReadOnlyList<int> GetAllowedBlockSizes();
    IBlock<TItem, TOffsetMarker> GetBlock(uint blockSize, TOffsetMarker? marker = default);

    #endregion

    #region Async

    Task<IBlock<TItem, TOffsetMarker>> GetBlockAsync(uint blockSize, TOffsetMarker? marker = default, CancellationToken ct = default);

    #endregion
}
