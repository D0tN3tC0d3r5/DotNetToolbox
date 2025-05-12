namespace DotNetToolbox.Data.DataSources;

internal class DataSourceFactory
    : IDataSourceFactory {
    public TDataSource Create<TDataSource, TStorage, TItem, TKey>(TStorage storage)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => InstanceFactory.Create<TDataSource>(storage);
    public TDataSource Create<TDataSource, TStorage, TItem, TKey>(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => Create<TDataSource, TStorage, TItem, TKey>(new TStorage {
            Id = id,
            KeyGenerator = keyGenerator,
            Data = seed as List<TItem> ?? [.. seed ?? []],
        });
    public TDataSource Create<TDataSource, TStorage, TItem, TKey>(string id, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => Create<TDataSource, TStorage, TItem, TKey>(id, InMemoryKeyGenerator<TKey>.Instance, seed);
    public TDataSource Create<TDataSource, TStorage, TItem, TKey>(IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => Create<TDataSource, TStorage, TItem, TKey>(typeof(TItem).Name, keyGenerator, seed);
    public TDataSource Create<TDataSource, TStorage, TItem, TKey>(IEnumerable<TItem>? seed)
        where TDataSource : DataSource<TStorage, TItem, TKey>
        where TStorage : class, IStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => Create<TDataSource, TStorage, TItem, TKey>(typeof(TItem).Name, InMemoryKeyGenerator<TKey>.Instance, seed);
    public TDataSource Create<TDataSource, TStorage, TItem>(TStorage storage)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => Create<TDataSource, TStorage, TItem, uint>(storage);
    public TDataSource Create<TDataSource, TStorage, TItem>(string id, IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => Create<TDataSource, TStorage, TItem, uint>(id, keyGenerator, seed);
    public TDataSource Create<TDataSource, TStorage, TItem>(IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => Create<TDataSource, TStorage, TItem, uint>(keyGenerator, seed);
    public TDataSource Create<TDataSource, TStorage, TItem>(string id, IEnumerable<TItem>? seed)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => Create<TDataSource, TStorage, TItem, uint>(id, seed);
    public TDataSource Create<TDataSource, TStorage, TItem>(IEnumerable<TItem>? seed)
        where TDataSource : DataSource<TStorage, TItem>
        where TStorage : class, IStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => Create<TDataSource, TStorage, TItem, uint>(seed);

    public TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(TStorage storage)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => InstanceFactory.Create<TDataSource>(storage);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => CreateAsync<TDataSource, TStorage, TItem, TKey>(new TStorage {
            Id = id,
            KeyGenerator = keyGenerator,
            Data = seed as List<TItem> ?? [.. seed ?? []],
        });
    public TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(string id, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => CreateAsync<TDataSource, TStorage, TItem, TKey>(id, InMemoryAsyncKeyGenerator<TKey>.Instance, seed);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => CreateAsync<TDataSource, TStorage, TItem, TKey>(typeof(TItem).Name, keyGenerator, seed);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem, TKey>(IEnumerable<TItem>? seed)
        where TDataSource : AsyncDataSource<TStorage, TItem, TKey>
        where TStorage : class, IAsyncStorage<TItem, TKey>, new()
        where TItem : IEntity<TKey>, new()
        where TKey : notnull
        => CreateAsync<TDataSource, TStorage, TItem, TKey>(typeof(TItem).Name, InMemoryAsyncKeyGenerator<TKey>.Instance, seed);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem>(TStorage storage)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => CreateAsync<TDataSource, TStorage, TItem, uint>(storage);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem>(string id, IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => CreateAsync<TDataSource, TStorage, TItem, uint>(id, keyGenerator, seed);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem>(IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => CreateAsync<TDataSource, TStorage, TItem, uint>(keyGenerator, seed);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem>(string id, IEnumerable<TItem>? seed)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => CreateAsync<TDataSource, TStorage, TItem, uint>(id, seed);
    public TDataSource CreateAsync<TDataSource, TStorage, TItem>(IEnumerable<TItem>? seed)
        where TDataSource : AsyncDataSource<TStorage, TItem>
        where TStorage : class, IAsyncStorage<TItem, uint>, new()
        where TItem : IEntity<uint>, new()
        => CreateAsync<TDataSource, TStorage, TItem, uint>(seed);
}
