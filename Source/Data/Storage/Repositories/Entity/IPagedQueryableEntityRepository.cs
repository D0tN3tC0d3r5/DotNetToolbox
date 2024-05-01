namespace DotNetToolbox.Data.Repositories.Entity;

public interface IPagedQueryableEntityRepository<TItem, TKey>
    : IQueryableEntityRepository<TItem, TKey>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    #region Blocking

    IReadOnlyList<int> GetAllowedPageSizes();
    IPage<TItem> GetPage(uint pageSize, uint pageIndex = 0);

    #endregion

    #region Async

    Task<IPage<TItem>> GetPageAsync(uint pageSize, uint pageIndex = 0, CancellationToken ct = default);

    #endregion
}
