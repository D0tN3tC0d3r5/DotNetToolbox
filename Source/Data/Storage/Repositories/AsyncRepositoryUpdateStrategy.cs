namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryUpdateStrategy<TItem>
    : AsyncRepositoryReadStrategy<TItem>,
      IAsyncRepository<TItem>
    where TItem : class {
    public virtual Task Add(TItem newItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();

    public virtual Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();
}
