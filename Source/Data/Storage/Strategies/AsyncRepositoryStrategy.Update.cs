namespace DotNetToolbox.Data.Strategies;

public abstract partial class AsyncRepositoryStrategy<TItem> {
    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();
}
