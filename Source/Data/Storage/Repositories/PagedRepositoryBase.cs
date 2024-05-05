namespace DotNetToolbox.Data.Repositories;

public abstract class PagedRepositoryBase<TStrategy, TItem, TKey>
    : RepositoryBase<TStrategy, TItem, TKey>
    , IPagedRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem, TKey>, new()
    where TItem : IEntity<TKey>
    where TKey : notnull {
    protected PagedRepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) {
    }
    protected PagedRepositoryBase(string name, IEnumerable<TItem>? data = null)
        : base(name, data) {
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

public class PagedRepositoryBase<TStrategy, TItem>
    : RepositoryBase<TStrategy, TItem>, IPagedRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem>, new() {
    public PagedRepositoryBase(IEnumerable<TItem>? data = null)
        : base(data) { }

    public PagedRepositoryBase(string name, IEnumerable<TItem>? data = null)
        : base(name, data) { }

#region Blocking

    public Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize) => Strategy.GetPage(pageIndex, pageSize);

#endregion

#region Async

    public ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0U, uint pageSize = DefaultPageSize, CancellationToken ct = default)
        => Strategy.GetPageAsync(pageIndex, pageSize, ct);

#endregion
}
