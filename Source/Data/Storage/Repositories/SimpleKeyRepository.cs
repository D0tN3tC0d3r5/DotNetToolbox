namespace DotNetToolbox.Data.Repositories;

public class SimpleKeyRepository<TModel, TKey>
    : QueryableRepository<TModel>,
      ISimpleKeyRepository<TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull {
    public SimpleKeyRepository(ModelAsyncQueryProvider queryProvider, Expression expression)
        : base(queryProvider, expression) {
    }

    public virtual Task<int> CountAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> HaveAny(CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TModel?> FindByKey(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TModel?> FindFirst(CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual ValueTask Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual ValueTask Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Update(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Remove(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();
}
