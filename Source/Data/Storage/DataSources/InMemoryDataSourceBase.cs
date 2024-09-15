namespace DotNetToolbox.Data.DataSources;

public abstract class InMemoryDataSourceBase<TRepository, TItem, TKey>
    : DataSource<InMemoryStorage<TItem, TKey>, TItem, TKey>
    where TRepository : InMemoryDataSourceBase<TRepository, TItem, TKey>
    where TItem : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected InMemoryDataSourceBase(IEnumerable<TItem>? records = null)
        : base(records) {
        Strategy = new(Records);
    }
}

public abstract class InMemoryDataSourceBase<TRepository, TItem>
    : DataSource<InMemoryStorage<TItem>, TItem>
    where TRepository : InMemoryDataSourceBase<TRepository, TItem> {
    protected InMemoryDataSourceBase(IEnumerable<TItem>? records = null)
        : base(records) {
        Strategy = new(Records);
    }
}
