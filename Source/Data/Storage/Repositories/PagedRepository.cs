namespace DotNetToolbox.Data.Repositories;

public class PagedRepository<TItem>
    : PagedRepository<IRepositoryStrategy<TItem>, TItem> {
    public PagedRepository(IEnumerable<TItem>? data = null)
        : base(DefaultName, new InMemoryRepositoryStrategy<TItem>(), data) {
    }
    public PagedRepository(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public PagedRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryRepositoryStrategy<TItem>(), data) {
    }
    public PagedRepository(string name, IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public class PagedRepository<TStrategy, TItem>
    : Repository<TStrategy, TItem>
    , IPagedQueryableRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    public PagedRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : this(DefaultName, strategy, data) {
    }
    public PagedRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(name, IsNotNull(strategy), data) {
    }

    #region Blocking

    public Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize)
        => Strategy.GetPage(pageIndex, pageSize);

    #endregion

    #region Async

    public ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0U, uint pageSize = DefaultPageSize, CancellationToken ct = default)
        => Strategy.GetPageAsync(pageIndex, pageSize, ct);

    #endregion
}
