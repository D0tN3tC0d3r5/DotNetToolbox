namespace DotNetToolbox.Data.Strategies.Entity;

public interface IEntityRepositoryStrategy<TItem, TKey>
    : IValueObjectRepositoryStrategy<TItem>
    , IEntityRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;
