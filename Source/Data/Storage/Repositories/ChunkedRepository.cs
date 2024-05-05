namespace DotNetToolbox.Data.Repositories;

public class ChunkedRepository<TItem, TKey>
    : ChunkedRepositoryBase<InMemoryRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public ChunkedRepository(IEnumerable<TItem>? data = null)
        : base(data) { }

    public ChunkedRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, data) { }
}

public class ChunkedRepository<TItem>
    : ChunkedRepositoryBase<InMemoryRepositoryStrategy<TItem>, TItem> {
    public ChunkedRepository(IEnumerable<TItem>? data = null)
        : base(data) { }

    public ChunkedRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, data) { }
}
