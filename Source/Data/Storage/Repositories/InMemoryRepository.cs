namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem, TKey>
    : Repository<InMemoryRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;

public class InMemoryRepository<TItem>
    : Repository<InMemoryRepositoryStrategy<TItem>, TItem>;
