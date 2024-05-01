namespace DotNetToolbox.Data.Repositories.Entity;

public interface IOffsetQueryableEntityRepository<TItem, TKey, TOffsetMarker>
    : IQueryableEntityRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    #region Blocking

    IReadOnlyList<int> GetAllowedBlockSizes();
    IBlock<TItem, TOffsetMarker> GetBlock(uint blockSize, TOffsetMarker? marker = default);

    #endregion

    #region Async

    Task<IBlock<TItem, TOffsetMarker>> GetBlockAsync(uint blockSize, TOffsetMarker? marker = default, CancellationToken ct = default);

    #endregion
}
