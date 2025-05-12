namespace DotNetToolbox.Data.Storages;

public interface IStorage
    : IReadOnlyDataSource,
      IUpdatableDataSource,
      IDisposable;

public interface IStorage<TItem, TKey>
    : IStorage,
      IReadOnlyDataSource<TItem, TKey>,
      IUpdatableDataSource<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    string Id { get; init; }
    IKeyGenerator<TKey> KeyGenerator { get; init; }
    List<TItem> Data { get; init; }
}
