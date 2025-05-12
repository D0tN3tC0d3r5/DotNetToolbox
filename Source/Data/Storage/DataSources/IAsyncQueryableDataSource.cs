namespace DotNetToolbox.Data.DataSources;

[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public interface IAsyncQueryableDataSource
    : IAsyncQueryable;

public interface IAsyncQueryableDataSource<out TItem, in TKey>
    : IAsyncQueryableDataSource,
      IAsyncQueryable<TItem>,
      IAsyncDisposable
    where TItem : IEntity<TKey>
    where TKey : notnull;
