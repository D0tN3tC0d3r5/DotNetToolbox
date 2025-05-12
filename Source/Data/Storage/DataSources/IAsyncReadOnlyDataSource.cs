namespace DotNetToolbox.Data.DataSources;

public interface IAsyncReadOnlyDataSource;

public interface IAsyncReadOnlyDataSource<TItem, in TKey>
    : IAsyncReadOnlyDataSource
    where TItem : IEntity<TKey>
    where TKey : notnull {
    Task<Result> LoadAsync(CancellationToken ct = default);

    ValueTask<TItem[]> GetAllAsync(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default);

    ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0,
                                        uint pageSize = DefaultPageSize,
                                        Expression<Func<TItem, bool>>? filterBy = null,
                                        HashSet<SortClause>? orderBy = null,
                                        CancellationToken ct = default);

    ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null,
                                          uint blockSize = DefaultBlockSize,
                                          Expression<Func<TItem, bool>>? filterBy = null,
                                          HashSet<SortClause>? orderBy = null,
                                          CancellationToken ct = default);

    ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default);
}
