namespace DotNetToolbox.Data.Strategies.ValueObject;

public abstract class ValueObjectRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem>,
    IValueObjectRepositoryStrategy<TItem> {

    #region Blocking

    public IReadOnlyList<int> GetAllowedPageSizes() => throw new NotImplementedException();
    public IReadOnlyList<int> GetAllowedBlockSizes() => throw new NotImplementedException();

    public IReadOnlyList<TItem> GetAll() => throw new NotImplementedException();
    public IPage<TItem> GetPage(uint pageSize, uint pageIndex = 0) => throw new NotImplementedException();
    public IBlock<TItem, TOffsetMarker> GetBlock<TOffsetMarker>(uint blockSize, TOffsetMarker? marker = default) => throw new NotImplementedException();

    public virtual TItem Find(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    public void Create(Action<TItem> setItem) => throw new NotImplementedException();

    public virtual void Add(TItem newItem) => throw new NotImplementedException();
    public virtual void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) => throw new NotImplementedException();
    public virtual void Update(TItem updatedItem) => throw new NotImplementedException();
    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) => throw new NotImplementedException();
    public virtual void Remove(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    #endregion

    #region Async

    public Task<IReadOnlyList<TItem>> GetAllAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public Task<IPage<TItem>> GetPageAsync(uint pageSize, uint pageIndex = 0, CancellationToken ct = default) => throw new NotImplementedException();
    public Task<IBlock<TItem, TOffsetMarker>> GetBlockAsync<TOffsetMarker>(uint blockSize, TOffsetMarker? marker = default, CancellationToken ct = default) => throw new NotImplementedException();

    public Task<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
