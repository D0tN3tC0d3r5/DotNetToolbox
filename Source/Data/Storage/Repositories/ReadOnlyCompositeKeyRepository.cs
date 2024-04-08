namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyCompositeKeyRepository<TModel>
    : QueryableRepository<TModel>,
      IReadOnlyCompositeKeyRepository<TModel>
    where TModel : class, ICompositeKeyEntity<TModel>, new() {

    public ReadOnlyCompositeKeyRepository(ModelAsyncQueryProvider queryProvider, Expression expression)
        : base(queryProvider, expression) {
    }

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
}
