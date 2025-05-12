namespace DotNetToolbox.Data.DataSources;

internal interface IDataSourceFactory {
    TDataSource Create<TDataSource, TStorage, TItem, TKey>(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource Create<TDataSource, TStorage, TItem, TKey>(IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource Create<TDataSource, TStorage, TItem, TKey>(string id, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource Create<TDataSource, TStorage, TItem, TKey>(IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource Create<TDataSource, TStorage, TItem, TKey>(TStorage storage)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource Create<TDataSource, TStorage, TItem>(string id, IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource Create<TDataSource, TStorage, TItem>(IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource Create<TDataSource, TStorage, TItem>(string id, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource Create<TDataSource, TStorage, TItem>(IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource Create<TDataSource, TStorage, TItem>(TStorage storage)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();

    TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(string id, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(TStorage storage)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull;
    TDataSource CreateAsync<TDataSource, TStorage, TItem>(string id, IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource CreateAsync<TDataSource, TStorage, TItem>(IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource CreateAsync<TDataSource, TStorage, TItem>(string id, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource CreateAsync<TDataSource, TStorage, TItem>(IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
    TDataSource CreateAsync<TDataSource, TStorage, TItem>(TStorage storage)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new();
}
