namespace Sophia.Data;

public abstract class Repository<TModel, TKey>(DataContext dataContext)
    : IRepository<TModel, TKey>
    where TModel : class, new()
    where TKey : notnull {

    public DataContext Context { get; } = dataContext;

#region Query
    public Task<IReadOnlyList<TModel>> GetList(CancellationToken ct = default)
        => GetList(default!, default!, default!, ct);
    public Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default)
        => GetList(default!, default!, include, ct);
    public Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, List<SortClause>>> sortBy, CancellationToken ct = default)
        => GetList(default!, sortBy, default!, ct);
    public Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, CancellationToken ct = default)
        => GetList(filterBy, default!, default!, ct);
    public Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, List<SortClause>>> sortBy, Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default)
        => GetList(default!, sortBy, include, ct);
    public Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default)
        => GetList(filterBy, default!, include, ct);
    public Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, Expression<Func<TModel, List<SortClause>>> sortBy, CancellationToken ct = default)
        => GetList(filterBy, sortBy, default!, ct);
    public virtual Task<IReadOnlyList<TModel>> GetList(Expression<Func<TModel, bool>> filterBy, Expression<Func<TModel, List<SortClause>>> sortBy, Expression<Func<TModel, List<IncludeClause>>> include, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<bool> HasAny(CancellationToken ct = default)
        => HasAny(default!, ct);
    public virtual Task<bool> HasAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => FindFirst(default!, ct);
    public virtual Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
#endregion

#region Command
    public virtual Task<TModel> CreateDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Add(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Add(Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(TModel input, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Update(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task Remove(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();
#endregion
}
