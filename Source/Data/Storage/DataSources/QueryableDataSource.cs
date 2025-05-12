using System.Diagnostics;

namespace DotNetToolbox.Data.DataSources;

[DebuggerDisplay("Data[{typeof(TItem).Name}]; Count = {Storage.Data.Count} ")]
public abstract class QueryableDataSource<TStorage, TItem, TKey>
    : IQueryableDataSource<TItem, TKey>
    where TStorage : class, IStorage<TItem, TKey>, new()
    where TItem : IEntity<TKey>, new()
    where TKey : notnull {
    private bool _disposed;
    private readonly bool _disposeStorage;
    internal string Id { get; } = $"|>Data[{typeof(TItem).Name}]_{Guid.CreateVersion7():N}<|";

    protected QueryableDataSource(string id, IKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null) {
        Storage = new() {
            Id = id,
            KeyGenerator = keyGenerator,
            Data = [..seed ?? []],
        };
        _disposeStorage = true;
    }

    protected QueryableDataSource(TStorage storage) {
        Storage = IsNotNull(storage);
        Storage.Load();
    }

    public void Dispose() {
        if (_disposed) return;
        Dispose(true);
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing) {
        if (!isDisposing) return;
        if (_disposeStorage) Storage.Dispose();
    }

    protected TStorage Storage { get; init; }

    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;

    internal IQueryable<TItem> Query => Storage.Data.AsQueryable();
    public IQueryProvider Provider => Query.Provider;
    IEnumerator IEnumerable.GetEnumerator()
        => Query.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator()
        => Query.GetEnumerator();
}
