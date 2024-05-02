using static DotNetToolbox.Pagination.PaginationSettings;

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

    #region Blockiging

    public Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize)
        => Strategy.GetPage(pageIndex, pageSize);

    #endregion

    #region Async

    public ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0U, uint pageSize = DefaultPageSize, CancellationToken ct = default)
        => Strategy.GetPageAsync(pageIndex, pageSize, ct);

    #endregion
}
