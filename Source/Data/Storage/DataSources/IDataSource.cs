namespace DotNetToolbox.Data.DataSources;

[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public interface IDataSource
    : IQueryableDataSource
    , IReadOnlyDataSource
    , IUpdatableDataSource
    , IDisposable;

public interface IDataSource<TItem, in TKey>
    : IDataSource
    , IQueryableDataSource<TItem, TKey>
    , IReadOnlyDataSource<TItem, TKey>
    , IUpdatableDataSource<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull;
