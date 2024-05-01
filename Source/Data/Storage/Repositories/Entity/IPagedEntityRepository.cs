namespace DotNetToolbox.Data.Repositories.Entity;

public interface IPagedEntityRepository<TItem, TKey>
    : IEntityRepository<TItem, TKey>
    , IPagedQueryableEntityRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull { }
