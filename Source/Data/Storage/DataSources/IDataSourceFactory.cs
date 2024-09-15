namespace DotNetToolbox.Data.DataSources;

internal interface IDataSourceFactory {
    InMemoryDataSource<TItem, TKey> CreateInMemory<TItem, TKey>(IEnumerable<TItem>? data = null)
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull;
    DataSource<TStrategy, TItem, TKey> CreateFromStorage<TStrategy, TItem, TKey>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IStorage<TItem, TKey>
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull;
    TRepository Create<TRepository, TStrategy, TItem, TKey>(IEnumerable<TItem>? data = null)
        where TRepository : DataSource<TStrategy, TItem, TKey>
        where TStrategy : class, IStorage<TItem, TKey>
        where TItem : class, IEntity<TKey>, new()
        where TKey : notnull;
    InMemoryDataSource<TItem> CreateInMemory<TItem>(IEnumerable<TItem>? data = null);
    DataSource<TStrategy, TItem> CreateFromStorage<TStrategy, TItem>(IEnumerable<TItem>? data = null)
        where TStrategy : class, IStorage<TItem>;
    TRepository Create<TRepository, TStrategy, TItem>(IEnumerable<TItem>? data = null)
        where TRepository : DataSource<TStrategy, TItem>
        where TStrategy : class, IStorage<TItem>;
}
