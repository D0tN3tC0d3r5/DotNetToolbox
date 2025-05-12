namespace DotNetToolbox.Data.DataSources;

[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public interface IAsyncDataSource
    : IAsyncQueryableDataSource
    , IAsyncReadOnlyDataSource
    , IAsyncUpdatableDataSource
    , IAsyncDisposable;

public interface IAsyncDataSource<TItem, in TKey>
    : IAsyncDataSource
    , IAsyncQueryableDataSource<TItem, TKey>
    , IAsyncReadOnlyDataSource<TItem, TKey>
    , IAsyncUpdatableDataSource<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;
