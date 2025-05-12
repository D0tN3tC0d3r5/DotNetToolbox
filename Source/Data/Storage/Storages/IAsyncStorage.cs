namespace DotNetToolbox.Data.Storages;

public interface IAsyncStorage
    : IReadOnlyDataSource,
      IUpdatableDataSource,
      IAsyncDisposable;

public interface IAsyncStorage<TItem, TKey>
    : IAsyncStorage,
      IAsyncReadOnlyDataSource<TItem, TKey>,
      IAsyncUpdatableDataSource<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    string Id { get; init; }
    IAsyncKeyGenerator<TKey> KeyGenerator { get; init; }
    List<TItem> Data { get; init; }
}
