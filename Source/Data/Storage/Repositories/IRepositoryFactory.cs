namespace DotNetToolbox.Data.Repositories;

internal interface IRepositoryFactory {
    InMemoryRepository<TItem, TKey> CreateInMemory<TItem, TKey>(IEnumerable<TItem>? data = null)
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull;
    Repository<TStrategy, TItem, TKey> CreateFromStrategy<TStrategy, TItem, TKey>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem, TKey>
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull;
    TRepository Create<TRepository, TStrategy, TItem, TKey>(IEnumerable<TItem>? data = null)
        where TRepository : Repository<TStrategy, TItem, TKey>
        where TStrategy : class, IRepositoryStrategy<TItem, TKey>
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull;
    InMemoryRepository<TItem> CreateInMemory<TItem>(IEnumerable<TItem>? data = null);
    Repository<TStrategy, TItem> CreateFromStrategy<TStrategy, TItem>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>;
    TRepository Create<TRepository, TStrategy, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : Repository<TStrategy, TItem>
        where TStrategy : class, IRepositoryStrategy<TItem>;
}
