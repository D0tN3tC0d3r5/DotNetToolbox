namespace DotNetToolbox.Data.Repositories;

public abstract class OffsetEntityRepository<TStrategy, TItem, TKey, TOffsetMarker>
    : OffsetValueObjectRepository<TStrategy, TItem, TOffsetMarker>
    , IOffsetQueryableEntityRepository<TItem, TKey, TOffsetMarker>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected OffsetEntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
}
