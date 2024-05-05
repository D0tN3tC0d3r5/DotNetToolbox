namespace DotNetToolbox.Data.Repositories;

public class PagedRepository<TItem, TKey>
    : PagedRepositoryBase<InMemoryRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public PagedRepository(IEnumerable<TItem>? data = null)
        : base(data) {
    }
    public PagedRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, data) {
    }
}

public class PagedRepository<TItem>
    : PagedRepositoryBase<InMemoryRepositoryStrategy<TItem>, TItem> {
    public PagedRepository(IEnumerable<TItem>? data = null)
        : base(data) {
    }
    public PagedRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, data) {
    }
}
