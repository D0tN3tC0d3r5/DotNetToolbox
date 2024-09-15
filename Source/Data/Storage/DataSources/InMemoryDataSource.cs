namespace DotNetToolbox.Data.DataSources;

public class InMemoryDataSource<TItem, TKey>(IEnumerable<TItem>? records = null)
    : InMemoryDataSourceBase<InMemoryDataSource<TItem, TKey>, TItem, TKey>(records)
    where TItem : class, IEntity<TKey>, new()
    where TKey : notnull;

public class InMemoryDataSource<TItem>(IEnumerable<TItem>? records = null)
    : InMemoryDataSourceBase<InMemoryDataSource<TItem>, TItem>(records);
