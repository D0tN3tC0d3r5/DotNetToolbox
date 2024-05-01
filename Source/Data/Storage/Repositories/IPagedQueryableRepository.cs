namespace DotNetToolbox.Data.Repositories;

public interface IPagedQueryableRepository<TItem>
    : IQueryableRepository<TItem> {

    #region Blocking

    IReadOnlyList<int> GetAllowedPageSizes();
    IPage<TItem> GetPage(uint pageSize, uint pageIndex = 0);

    #endregion

    #region Async

    Task<IPage<TItem>> GetPageAsync(uint pageSize, uint pageIndex = 0, CancellationToken ct = default);

    #endregion
}
