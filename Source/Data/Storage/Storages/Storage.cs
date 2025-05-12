namespace DotNetToolbox.Data.Storages;

public abstract class Storage<TStorage, TItem, TKey>(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? data = null)
    : IStorage<TItem, TKey>
    where TStorage : Storage<TStorage, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    private bool _disposed;

    public void Dispose() {
        if (_disposed) return;
        Dispose(true);
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
    }

    public string Id { get; init; } = id;
    public IKeyGenerator<TKey> KeyGenerator { get; init; } = keyGenerator;
    public List<TItem> Data { get; init; } = data as List<TItem> ?? [..data ?? []];
    protected TKey GetNextKey() => KeyGenerator.GenerateNextKey(Id);
    public virtual Result Load() => KeyGenerator.Initialize(Id);

    public abstract TItem? FindByKey(TKey key);
    public abstract Result Update(TItem updatedItem, IMap? validationContext = null);
    public abstract Result UpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null);
    public abstract Result AddOrUpdate(TItem updatedItem, IMap? validationContext = null);
    public abstract Result AddOrUpdateMany(IEnumerable<TItem> updatedItems, IMap? validationContext = null);
    public abstract Result Patch(TKey key, Action<TItem> setItem, IMap? validationContext = null);
    public abstract Result PatchMany(IEnumerable<TKey> keys, Action<TItem> setItem, IMap? validationContext = null);
    public abstract Result Remove(TKey key);
    public abstract Result RemoveMany(IEnumerable<TKey> keys);
    public abstract Result Seed(IEnumerable<TItem> seed, bool preserveContent = false, IMap? validationContext = null);
    public abstract TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);
    public abstract Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);
    public abstract Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);
    public abstract TItem? Find(Expression<Func<TItem, bool>> predicate);
    public abstract Result<TItem> Create(Action<TItem>? setItem = null, IMap? validationContext = null);
    public abstract Result Add(TItem newItem, IMap? validationContext = null);
    public abstract Result Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null);
    public abstract Result AddMany(IEnumerable<TItem> newItems, IMap? validationContext = null);
    public abstract Result UpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> updatedItems, IMap? validationContext = null);
    public abstract Result AddOrUpdate(Expression<Func<TItem, bool>> predicate, TItem updatedItem, IMap? validationContext = null);
    public abstract Result AddOrUpdateMany(Expression<Func<TItem, bool>> predicate, IEnumerable<TItem> items, IMap? validationContext = null);
    public abstract Result Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null);
    public abstract Result PatchMany(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem, IMap? validationContext = null);
    public abstract Result Remove(Expression<Func<TItem, bool>> predicate);
    public abstract Result RemoveMany(Expression<Func<TItem, bool>> predicate);
    public abstract Result Clear();
}
