namespace DotNetToolbox.Data.DataSources;

public class InMemoryAsyncDataSource<TItem>
    : AsyncDataSource<InMemoryAsyncStorage<TItem>, TItem>
    where TItem : IEntity<uint>, new() {
    public InMemoryAsyncDataSource(InMemoryAsyncStorage<TItem> storage)
        : base(storage) { }
    public InMemoryAsyncDataSource(string id, IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) { }
    public InMemoryAsyncDataSource(IAsyncKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryAsyncDataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryAsyncKeyGenerator<uint>.Instance, seed) { }
    public InMemoryAsyncDataSource(IEnumerable<TItem> seed)
        : this(InMemoryAsyncKeyGenerator<uint>.Instance, seed) { }
    public InMemoryAsyncDataSource()
        : this([]) { }
}

public class InMemoryAsyncDataSource<TItem, TKey>
    : AsyncDataSource<InMemoryAsyncStorage<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>, new()
    where TKey : notnull {
    public InMemoryAsyncDataSource(InMemoryAsyncStorage<TItem, TKey> storage)
        : base(storage) { }
    public InMemoryAsyncDataSource(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) { }
    public InMemoryAsyncDataSource(IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) { }
    public InMemoryAsyncDataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryAsyncKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryAsyncDataSource(IEnumerable<TItem> seed)
        : this(InMemoryAsyncKeyGenerator<TKey>.Instance, seed) { }
    public InMemoryAsyncDataSource()
        : this([]) { }
}
