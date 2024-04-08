namespace DotNetToolbox.Data.Repositories;

public class CompositeKeyRepository<TModel>(ModelAsyncQueryProvider queryProvider, Expression expression)
    : QueryableRepository<TModel>(queryProvider, expression),
      ICompositeKeyRepository<TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new() {
    public virtual Task<int> CountAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(default!, ct);
    public virtual Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<TModel?> FindByKey(object key, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<TModel?> FindByKeys(object?[] keys, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => FindFirst(default!, ct);
    public virtual Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual ValueTask Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Update(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(object key, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(object?[]? keys, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Remove(object key, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Remove(object?[]? keys, CancellationToken ct = default)
        => throw new NotImplementedException();
}
