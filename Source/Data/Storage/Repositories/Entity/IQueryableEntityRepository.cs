namespace DotNetToolbox.Data.Repositories.Entity;

public interface IQueryableEntityRepository<out TItem, TKey>
    : IQueryableValueObjectRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull;
