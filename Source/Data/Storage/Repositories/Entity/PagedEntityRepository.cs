using DotNetToolbox.Data.Strategies.Key;

namespace DotNetToolbox.Data.Repositories.Entity;

public class PagedEntityRepository<TItem, TKey, TKeyHandler>
    : PagedEntityRepository<IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>, TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    public PagedEntityRepository(IEnumerable<TItem>? data = null)
        : base(new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) {
    }
    public PagedEntityRepository(IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    public PagedEntityRepository(string name, IEnumerable<TItem>? data = null)
        : base(name, new InMemoryEntityRepositoryStrategy<TItem, TKey, TKeyHandler>(), data) {
    }
    public PagedEntityRepository(string name, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler> strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
    }
}

public abstract class PagedEntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    : EntityRepository<TStrategy, TItem, TKey, TKeyHandler>
    , IPagedQueryableRepository<TItem>
    where TStrategy : class, IEntityRepositoryStrategy<TItem, TKey, TKeyHandler>
    where TItem : IEntity<TKey>
    where TKeyHandler : class, IKeyHandler<TKey>, IHasDefault<TKeyHandler>
    where TKey : notnull {
    protected PagedEntityRepository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data) {
    }
    protected PagedEntityRepository(string name, TStrategy strategy, IEnumerable<TItem>? data = null)
        : base(name, strategy, data) {
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
