namespace DotNetToolbox.Data.DataSources;

public class DataSource<TStorage, TItem>
    : DataSource<TStorage, TItem, uint>
    where TStorage : class, IStorage<TItem, uint>, new()
    where TItem : IEntity<uint>, new() {
    public DataSource(string id, IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) {
    }
    public DataSource(IKeyGenerator<uint> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) {
    }
    public DataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryKeyGenerator<uint>.Instance, seed) {
    }
    public DataSource(IEnumerable<TItem> seed)
        : this(InMemoryKeyGenerator<uint>.Instance, seed) {
    }
    public DataSource()
        : this([]) { }
    public DataSource(TStorage storage)
        : base(storage) { }
}

public class DataSource<TStorage, TItem, TKey>
    : QueryableDataSource<TStorage, TItem, TKey>
    , IDataSource<TItem, TKey>
    where TStorage : class, IStorage<TItem, TKey>, new()
    where TItem : IEntity<TKey>, new()
    where TKey : notnull {
    public DataSource(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : base(id, keyGenerator, seed) {
    }

    public DataSource(IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null)
        : this(typeof(TItem).Name, keyGenerator, seed) {
    }

    public DataSource(string id, IEnumerable<TItem>? seed = null)
        : this(id, InMemoryKeyGenerator<TKey>.Instance, seed) { }

    public DataSource(IEnumerable<TItem> seed)
        : this(InMemoryKeyGenerator<TKey>.Instance, seed) { }

    public DataSource()
        : this([]) { }

    public DataSource(TStorage storage)
        : base(storage) {
    }

    public TItem? FindByKey(TKey key)
        => Storage.FindByKey(key);

    public Result Update(TItem updatedItem, IMap? validationContext = null)
        => Storage.Update(updatedItem, validationContext);

    public Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Storage.UpdateMany(updatedItems, validationContext);

    public Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null)
        => Storage.AddOrUpdate(updatedItem, validationContext);

    public Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Storage.AddOrUpdateMany(updatedItems, validationContext);

    public Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null)
        => Storage.Patch(key, setItem, validationContext);

    public Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null)
        => Storage.PatchMany(keys, setItem, validationContext);

    public Result Remove(TKey key)
        => Storage.Remove(key);

    public Result RemoveMany(IEnumerable<TKey> keys)
        => Storage.RemoveMany(keys);

    public virtual Result Load()
        => Storage.Load();
    public virtual Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null)
        => Storage.Seed(seed, preserveContent, validationContext);

    public virtual TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Storage.GetAll(filterBy, orderBy);

    public virtual Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Storage.GetPage(pageIndex, pageSize, filterBy, orderBy);

    public virtual Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null)
        => Storage.GetChunk(isChunkStart, blockSize, filterBy, orderBy);

    public virtual TItem? Find(Expression<Func<TItem, bool>> predicate)
        => Storage.Find(predicate);

    public virtual Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null)
        => Storage.Create(setItem, validationContext);
    public virtual Result Add(TItem newItem, IMap? validationContext = null)
        => Storage.Add(newItem, validationContext);

    public virtual Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null)
        => Storage.AddMany(newItems, validationContext);

    public virtual Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null)
        => Storage.Update(predicate, updatedItem, validationContext);
    public virtual Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null)
        => Storage.UpdateMany(predicate, updatedItems, validationContext);

    public virtual Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null)
        => Storage.AddOrUpdate(predicate, updatedItem, validationContext);
    public virtual Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null)
        => Storage.AddOrUpdateMany(predicate, items, validationContext);

    public virtual Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null)
        => Storage.Patch(predicate, setItem, validationContext);
    public virtual Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null)
        => Storage.PatchMany(predicate, setItem, validationContext);

    public virtual Result Remove(Expression<Func<TItem, bool>> predicate)
        => Storage.Remove(predicate);
    public virtual Result RemoveMany(Expression<Func<TItem, bool>> predicate)
        => Storage.RemoveMany(predicate);

    public virtual Result Clear()
        => Storage.Clear();
}
