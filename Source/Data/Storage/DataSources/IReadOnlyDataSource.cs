namespace DotNetToolbox.Data.DataSources;

public interface IReadOnlyDataSource;

public interface IReadOnlyDataSource<TItem, in TKey >
    : IReadOnlyDataSource
    where TItem : IEntity<TKey>
    where TKey : notnull {
    Result Load();

    TItem[] GetAll(Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);

    Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);
    Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, Expression<Func<TItem, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null);

    TItem? Find(Expression<Func<TItem, bool>> predicate);

    TItem? FindByKey(TKey key);
}
