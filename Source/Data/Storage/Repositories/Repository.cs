namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem, TKey>
    : RepositoryBase<InMemoryRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public Repository(IEnumerable<TItem>? data = null)
        : base(data) { }

    public Repository(string name, IEnumerable<TItem>? data = null)
        : base(name, data) { }
}

public class Repository<TItem>
    : RepositoryBase<InMemoryRepositoryStrategy<TItem>, TItem> {
    public Repository(IEnumerable<TItem>? data = null)
        : base(DefaultName, data) { }

    public Repository(string name, IEnumerable<TItem>? data = null)
        : base(name, data) { }
}
