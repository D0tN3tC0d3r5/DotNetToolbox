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
    public PagedValueObjectRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryValueObjectRepositoryStrategy<TItem>(), data) { }
    public PagedValueObjectRepository(string name, IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base(name, (IValueObjectRepositoryStrategy<TItem>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public PagedValueObjectRepository(string name, IValueObjectRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class PagedValueObjectRepository<TStrategy, TItem>
    : ValueObjectRepository<TItem>
    , IPagedQueryableRepository<TItem>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected PagedValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    protected PagedValueObjectRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
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
