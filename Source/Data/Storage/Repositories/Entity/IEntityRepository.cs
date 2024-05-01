namespace DotNetToolbox.Data.Repositories.Entity;

public interface IEntityRepository<TItem, TKey>
    : IRepository
    , IQueryableEntityRepository<TItem, TKey>
    , IUpdatableEntityRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;
