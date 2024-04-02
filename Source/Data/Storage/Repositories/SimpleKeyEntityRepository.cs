namespace DotNetToolbox.Data.Repositories;

public abstract class SimpleKeyEntityRepository<TRepository, TModel, TKey>
    : QueryableRepository<TRepository, TModel>,
      ISimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TRepository : SimpleKeyEntityRepository<TRepository, TModel, TKey>
    where TModel : class, ISimpleKeyEntity<TModel, TKey>, new()
    where TKey : notnull {
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
