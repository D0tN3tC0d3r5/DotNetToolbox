using System.Diagnostics;

namespace DotNetToolbox.Data.DataSources;

[DebuggerDisplay("Data[{typeof(TItem).Name}]; Count = {Storage.Data.Count} ")]
public abstract class AsyncQueryableDataSource<TStorage, TItem, TKey>
    : IAsyncQueryableDataSource<TItem, TKey>
    where TStorage : class, IAsyncStorage<TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {
    private bool _disposed;
    private readonly bool _disposeStorage;
    private readonly IAsyncQueryable<TItem> _query;
    internal string Id { get; } = $"|>Data[{typeof(TItem).Name}]_{Guid.CreateVersion7():N}<|";

    protected AsyncQueryableDataSource(string id, IAsyncKeyGenerator<TKey> keyGenerator, IEnumerable<TItem>? seed = null) {
        Storage = new() {
            Id = id,
            KeyGenerator = keyGenerator,
            Data = seed as List<TItem> ?? [..seed ?? []],
        };
        _disposeStorage = true;
        _query = new AsyncQueryable<TItem>(Storage.Data);
    }

    protected AsyncQueryableDataSource(TStorage storage) {
        Storage = IsNotNull(storage);
        Storage.LoadAsync().GetAwaiter().GetResult().EnsureIsSuccess();
        _query = new AsyncQueryable<TItem>(Storage.Data);
    }

    public async ValueTask DisposeAsync() {
        if (_disposed) return;
        await DisposeAsyncCore();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    protected virtual ValueTask DisposeAsyncCore()
        => _disposeStorage
        ? Storage.DisposeAsync()
        : ValueTask.CompletedTask;

    protected TStorage Storage { get; init; }
    public Type ElementType => _query.ElementType;
    public Expression Expression => _query.Expression;

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => _query.GetAsyncEnumerator(cancellationToken);
    public IAsyncQueryProvider AsyncProvider => _query.AsyncProvider;

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => _query.GetEnumerator();
    IQueryProvider IQueryable.Provider => _query.Provider;
    IEnumerator IEnumerable.GetEnumerator() => _query.GetEnumerator();
}
