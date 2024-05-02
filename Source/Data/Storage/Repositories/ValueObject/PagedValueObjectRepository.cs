using static DotNetToolbox.Pagination.PaginationSettings;

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

    #region Blocking

    public Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize)
        => Strategy.GetPage(pageIndex, pageSize);

    #endregion

    #region Async

    public ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0U, uint pageSize = DefaultPageSize, CancellationToken ct = default)
        => Strategy.GetPageAsync(pageIndex, pageSize, ct);

    #endregion
}
