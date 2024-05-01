namespace DotNetToolbox.Data.Repositories.ValueObject;

public interface IPagedQueryableValueObjectRepository<TItem>
    : IQueryableValueObjectRepository<TItem> {

    #region Blocking

    IReadOnlyList<int> GetAllowedPageSizes();
    IPage<TItem> GetList(uint pageSize, uint pageIndex = 0);

    #endregion

    #region Async

    Task<IPage<TItem>> GetListAsync(uint pageSize, uint pageIndex = 0, CancellationToken ct = default);

    #endregion
}
