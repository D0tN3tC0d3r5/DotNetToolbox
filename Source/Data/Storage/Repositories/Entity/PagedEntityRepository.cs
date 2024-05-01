namespace DotNetToolbox.Data.Repositories.Entity;

public abstract class PagedValueObjectRepository<TStrategy, TItem>
    : ValueObjectRepository<TItem>
    , IPagedQueryableValueObjectRepository<TItem>
    where TStrategy : class, IValueObjectRepositoryStrategy<TItem> {
    protected PagedValueObjectRepository(TStrategy strategy, IEnumerable<TItem>? data = null) {
        Strategy = IsNotNull(strategy);
        if (data is null)
            return;
        var list = data as List<TItem> ?? data.ToList();
        Strategy.Seed(list);
    }

    #region Blocking

    public IReadOnlyList<int> GetAllowedPageSizes() => throw new NotImplementedException();
    public IPage<TItem> GetList(uint pageSize, uint pageIndex = 0) => throw new NotImplementedException();

    #endregion

    #region Async

    public Task<IPage<TItem>> GetListAsync(uint pageSize, uint pageIndex = 0, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
