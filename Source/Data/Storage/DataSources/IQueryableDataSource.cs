namespace DotNetToolbox.Data.DataSources;

[SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "Implemented below")]
[SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
public interface IQueryableDataSource
    : IQueryable;

public interface IQueryableDataSource<out TItem, in TKey>
    : IQueryableDataSource,
      IQueryable<TItem>,
      IDisposable
    where TItem : IEntity<TKey>
    where TKey : notnull;
