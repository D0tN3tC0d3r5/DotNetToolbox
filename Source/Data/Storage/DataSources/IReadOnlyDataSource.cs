namespace DotNetToolbox.Data.DataSources;

public interface IReadOnlyDataSource;

public interface IReadOnlyDataSource<TItem>
    : IReadOnlyDataSource {
    #region Blocking

    Result Load();

    TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);

    Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);
    Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);

    TItem? Find(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    Task<Result> LoadAsync(CancellationToken ct = default);

    ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);
    ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);
    ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);

    ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}

public interface IReadOnlyDataSource<TItem, in TKey>
    : IReadOnlyDataSource<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {
    #region Blocking

    TItem? FindByKey(TKey key);

    #endregion

    #region Async

    ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default);

    #endregion
}
