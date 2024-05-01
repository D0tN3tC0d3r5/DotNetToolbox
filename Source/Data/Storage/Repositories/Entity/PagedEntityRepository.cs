namespace DotNetToolbox.Data.Repositories.Entity;

public class PagedEntityRepository<TItem, TKey>
    : PagedEntityRepository<IEntityRepositoryStrategy<TItem, TKey>, TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    public PagedEntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey>(), data) { }
    public PagedEntityRepository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base((IEntityRepositoryStrategy<TItem, TKey>)IsNotNull(provider).GetStrategy<TItem>(), data) { }
    public PagedEntityRepository(IEntityRepositoryStrategy<TItem, TKey> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
}

public abstract class PagedEntityRepository<TStrategy, TItem, TKey>
    : EntityRepository<TStrategy, TItem, TKey>
    , IPagedQueryableRepository<TItem>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected PagedEntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
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
