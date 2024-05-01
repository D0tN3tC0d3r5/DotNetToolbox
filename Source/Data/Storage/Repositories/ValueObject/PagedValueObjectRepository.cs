namespace DotNetToolbox.Data.Repositories.ValueObject;

public class PagedValueObjectRepository<TItem>
    : PagedValueObjectRepository<IValueObjectRepositoryStrategy<TItem>, TItem> {
    public PagedValueObjectRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryValueObjectRepositoryStrategy<TItem>(), data) { }
    public PagedValueObjectRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IValueObjectRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public PagedValueObjectRepository(IValueObjectRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
}

public abstract class PagedValueObjectRepository<TStrategy, TItem>
    : ValueObjectRepository<TItem>
    , IPagedQueryableRepository<TItem>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected PagedValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null) {
        Strategy = IsNotNull(strategy);
        if (data is null)
            return;
        var list = data as List<TItem> ?? data.ToList();
        Strategy.Seed(list);
    }

    #region Pageing

    public IReadOnlyList<int> GetAllowedPageSizes()
        => Strategy.GetAllowedPageSizes();
    public IPage<TItem> GetPage(uint pageSize, uint pageIndex = 0)
        => Strategy.GetPage(pageSize, pageIndex);

    #endregion

    #region Async

    public Task<IPage<TItem>> GetPageAsync(uint pageSize, uint pageIndex = 0, CancellationToken ct = default)
        => Strategy.GetPageAsync(pageSize, pageIndex, ct);

    #endregion
}
