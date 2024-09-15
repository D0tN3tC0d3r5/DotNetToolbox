namespace DotNetToolbox.Data.Storages;

public interface IStorage
    : IReadOnlyDataSource,
      IUpdatableDataSource,
      IAsyncDisposable;

public interface IStorage<TItem>
    : IStorage,
      IReadOnlyDataSource<TItem>,
      IUpdatableDataSource<TItem> {
    List<TItem> Data { get; }
}

public interface IStorage<TItem, in TKey>
    : IStorage<TItem>,
      IReadOnlyDataSource<TItem, TKey>,
      IUpdatableDataSource<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;
