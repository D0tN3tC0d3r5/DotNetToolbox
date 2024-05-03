using static DotNetToolbox.Pagination.PaginationSettings;

namespace DotNetToolbox.Data.Strategies.ValueObject;

public abstract class ValueObjectRepositoryStrategy<TItem>
    : RepositoryStrategy<TItem>,
    IValueObjectRepositoryStrategy<TItem> {

    #region Blocking

    public virtual TItem[] GetAll() => throw new NotImplementedException();
    public virtual Page<TItem> GetPage(uint pageIndex = 0, uint pageSize = DefaultPageSize) => throw new NotImplementedException();
    public virtual Block<TItem> GetBlock(Expression<Func<TItem, bool>> isNotStart, uint blockSize = DefaultBlockSize) => throw new NotImplementedException();

    public virtual TItem? Find(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    public virtual TItem Create(Action<TItem> setItem) => throw new NotImplementedException();

    public virtual void Add(TItem newItem) => throw new NotImplementedException();
    public virtual void Patch(Expression<Func<TItem, bool>> predicate, Action<TItem> setItem) => throw new NotImplementedException();
    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) => throw new NotImplementedException();
    public virtual void Remove(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    #endregion

    #region Async

    public virtual ValueTask<TItem[]> GetAllAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public virtual ValueTask<Page<TItem>> GetPageAsync(uint pageIndex = 0, uint pageSize = DefaultPageSize, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual ValueTask<Block<TItem>> GetBlockAsync(Expression<Func<TItem, bool>> findStart, uint blockSize = 20U, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual ValueTask<TItem?> FindAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task<TItem> CreateAsync(Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task PatchAsync(Expression<Func<TItem, bool>> predicate, Func<TItem, CancellationToken, Task> setItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
