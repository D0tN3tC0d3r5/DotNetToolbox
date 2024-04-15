namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryStrategy<TItem>(IEnumerable<TItem> remote)
    : QueryableStrategy<TItem, TItem>(remote, s => s, s => s)
    where TItem : class {
}

public abstract class AsyncRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
                                                               Expression<Func<TModel, TEntity>> projectToEntity,
                                                               Expression<Func<TEntity, TModel>> projectToModel)
    : QueryableStrategy<TModel, TEntity>(remote, projectToEntity, projectToModel),
      IAsyncRepositoryStrategy<TModel>
    where TModel : class
    where TEntity : class {
    public virtual Task<bool> HaveAny(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(HaveAny));
    public virtual Task<int> Count(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(HaveAny));
    public virtual Task<TModel[]> ToArray(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(ToArray));
    public virtual Task<TModel?> GetFirst(CancellationToken ct = default)
        => throw new NotImplementedException(nameof(GetFirst));
    public virtual Task Add(TModel newItem, CancellationToken ct = default)
        => throw new NotImplementedException(nameof(Add));
    public virtual Task Update(Expression<Func<TModel, bool>> predicate, TModel updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException(nameof(Update));
    public virtual Task Remove(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException(nameof(Remove));
}
