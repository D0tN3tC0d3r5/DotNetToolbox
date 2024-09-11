namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepository<TItem, TKey>(IEnumerable<TItem>? data = null)
    : InMemoryRepositoryBase<InMemoryRepository<TItem, TKey>, TItem, TKey>(data)
    where TItem : class, IEntity<TKey>, new()
    where TKey : notnull;

public class InMemoryRepository<TItem>(IEnumerable<TItem>? data = null)
    : InMemoryRepositoryBase<InMemoryRepository<TItem>, TItem>(data);
