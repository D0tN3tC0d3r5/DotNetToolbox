namespace DotNetToolbox.Data.Repositories.Entity;

public interface IOffsetEntityRepository<TItem, TKey, TOffsetMarker>
    : IEntityRepository<TItem, TKey>, IOffsetQueryableEntityRepository<TItem, TKey, TOffsetMarker>
    where TItem : IEntity<TKey>
    where TKey : notnull { }
