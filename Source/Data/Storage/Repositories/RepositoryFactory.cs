namespace DotNetToolbox.Data.Repositories;

internal class RepositoryFactory
    : IRepositoryFactory {
    public InMemoryRepository<TItem, TKey> CreateInMemory<TItem, TKey>(IEnumerable<TItem>? data = null)
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull
        => new(data);

    public Repository<TStrategy, TItem, TKey> CreateFromStrategy<TStrategy, TItem, TKey>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem, TKey>
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull
        => InstanceFactory.Create<Repository<TStrategy, TItem, TKey>>(data ?? []);
    public TRepository Create<TRepository, TStrategy, TItem, TKey>(IEnumerable<TItem>? data = null)
        where TRepository : Repository<TStrategy, TItem, TKey>
        where TStrategy : class, IRepositoryStrategy<TItem, TKey>
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull
        => InstanceFactory.Create<TRepository>(data ?? []);

    public InMemoryRepository<TItem> CreateInMemory<TItem>(IEnumerable<TItem>? data = null)
        => new(data);

    public Repository<TStrategy, TItem> CreateFromStrategy<TStrategy, TItem>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IRepositoryStrategy<TItem>
        => InstanceFactory.Create<Repository<TStrategy, TItem>>(data ?? []);
    public TRepository Create<TRepository, TStrategy, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : Repository<TStrategy, TItem>
        where TStrategy : class, IRepositoryStrategy<TItem>
        => InstanceFactory.Create<TRepository>(data ?? []);
}
