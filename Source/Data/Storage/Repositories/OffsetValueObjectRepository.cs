namespace DotNetToolbox.Data.Repositories;

public abstract class OffsetValueObjectRepository<TStrategy, TItem, TOffsetMarker>
    : ValueObjectRepository<TItem>
    , IOffsetQueryableValueObjectRepository<TItem, TOffsetMarker>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected OffsetValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null) {
        Strategy = IsNotNull(strategy);
        if (data is null)
            return;
        var list = data as List<TItem> ?? data.ToList();
        Strategy.Seed(list);
    }

#region Blocking

    public IReadOnlyList<int> GetAllowedBlockSizes() => throw new NotImplementedException();
    public IBlock<TItem, TOffsetMarker> GetBlock(uint blockSize, TOffsetMarker? marker = default) => throw new NotImplementedException();

#endregion

#region Async

    public Task<IBlock<TItem, TOffsetMarker>> GetBlockAsync(uint blockSize, TOffsetMarker? marker = default, CancellationToken ct = default) => throw new NotImplementedException();

#endregion
}
