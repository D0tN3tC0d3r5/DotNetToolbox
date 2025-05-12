namespace DotNetToolbox.Data.DataSources;

public class InMemoryDataSource<TItem>
    : DataSource<InMemoryStorage<TItem>, TItem>
    where TItem : IEntity<uint>, new() {
    public InMemoryDataSource(InMemoryStorage<TItem> storage)
        : base(storage) { }
    public InMemoryDataSource(string id, IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) { }
    public InMemoryDataSource(IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryDataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryKeyGenerator<uint>.Instance, seed) { }
    public InMemoryDataSource(IEnumerable<TItem> seed)
        : this(InMemoryKeyGenerator<uint>.Instance, seed) { }
    public InMemoryDataSource()
        : this([]) { }
}

public class InMemoryDataSource<TItem, TKey>
    : DataSource<InMemoryStorage<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>, new()
    where TKey : notnull {
    public InMemoryDataSource(InMemoryStorage<TItem, TKey> storage)
        : base(storage) { }
    public InMemoryDataSource(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) { }
    public InMemoryDataSource(IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryDataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryDataSource(IEnumerable<TItem> seed)
        : this(InMemoryKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryDataSource()
        : this([]) { }
}
