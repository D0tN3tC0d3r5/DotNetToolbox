namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryStrategy<TModel>(IEnumerable<TModel> remote)
    : QueryableStrategy<TModel, TModel>(remote, s => s, s => s) {
}

public abstract class AsyncRepositoryStrategy<TModel, TEntity>(IEnumerable<TEntity> remote,
                                                               Expression<Func<TModel, TEntity>> projectToEntity,
                                                               Expression<Func<TEntity, TModel>> projectToModel)
    : QueryableStrategy<TModel, TEntity>(remote, projectToEntity, projectToModel),
      IAsyncRepositoryStrategy<TModel> {
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
