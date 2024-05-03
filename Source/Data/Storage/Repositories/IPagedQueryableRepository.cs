namespace DotNetToolbox.Data.Repositories;

public interface IPagedQueryableRepository<TItem>
    : IQueryableRepository<TItem> {

    #region Blocking

    Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize);

    #endregion

    #region Async

    ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default);

    #endregion
}
