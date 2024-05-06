namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository;

public interface IReadOnlyRepository<TItem>
    : IReadOnlyRepository {

    #region Blocking

    void Load();

    TItem[] GetAll();

    Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize);
    Chunk<TItem> GetChunk(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize);

    TItem? Find(Expression<Func<TItem, bool>> predicate);

    #endregion

    #region Async

    Task LoadAsync(CancellationToken ct = default);

    ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default);
    ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default);
    ValueTask<Chunk<TItem>> GetChunkAsync(Expression<Func<TItem, bool>>? isChunkStart = null, uint blockSize = DefaultBlockSize, CancellationToken ct = default);

    ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    #endregion
}

public interface IReadOnlyRepository<TItem, in TKey>
    : IReadOnlyRepository<TItem>
    where TItem : IEntity<TKey>
    where TKey : notnull {

    #region Blocking

    TItem? FindByKey(TKey key);

    #endregion

    #region Async

    ValueTask<TItem?> FindByKeyAsync(TKey key, CancellationToken ct = default);

    #endregion
}
