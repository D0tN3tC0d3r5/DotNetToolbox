namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyStorage<TModel, TKey>
    : QueryableSet<TModel>,
      IReadOnlyStorage<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    public ReadOnlyStorage(IEnumerable<TModel>? source = null)
        : base(default, source, default) {
    }

    public ReadOnlyStorage(Expression expression)
        : base(default, default, IsNotNull(expression)) {
    }

    public ReadOnlyStorage(IQueryStrategy strategy, IEnumerable<TModel>? source = null)
        : base(IsNotNull(strategy), source, default) {
    }

    public ReadOnlyStorage(IQueryStrategy strategy, Expression expression)
        : base(IsNotNull(strategy), default, IsNotNull(expression)) {
    }

    public virtual Task<int> CountAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
    public Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(default!, ct);
    public virtual Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<TModel?> FindByKey(TKey key, CancellationToken ct = default)
        => throw new NotImplementedException();
    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => FindFirst(default!, ct);
    public virtual Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
}

public class ReadOnlyStorage<TModel>
    : QueryableSet<TModel>,
      IReadOnlyStorage<TModel>
    where TModel : class, IEntity, new() {
    public ReadOnlyStorage(IEnumerable<TModel>? source = null)
        : base(default, source, default) {
    }

    public ReadOnlyStorage(Expression expression)
        : base(default, default, IsNotNull(expression)) {
    }

    public ReadOnlyStorage(IQueryStrategy strategy, IEnumerable<TModel>? source = null)
        : base(IsNotNull(strategy), source, default) {
    }

    public ReadOnlyStorage(IQueryStrategy strategy, Expression expression)
        : base(IsNotNull(strategy), default, IsNotNull(expression)) {
    }

    public virtual Task<int> CountAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
    public Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(default!, ct);
    public virtual Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
    public virtual Task<TModel?> FindByKey<TKey>(TKey key, CancellationToken ct = default)
        where TKey : notnull
        => throw new NotImplementedException();
    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => FindFirst(default!, ct);
    public virtual Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
}
