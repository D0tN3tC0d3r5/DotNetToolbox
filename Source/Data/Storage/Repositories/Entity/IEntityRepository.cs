using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Repositories.Entity;

public interface IEntityRepository<TItem, TKey, TKeyHandler>
    : IRepository<TItem>
    , IQueryableEntityRepository<TItem, TKey>
    , IUpdatableEntityRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull;
