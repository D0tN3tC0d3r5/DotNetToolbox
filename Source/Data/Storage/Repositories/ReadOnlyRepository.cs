namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepository<TModel, TKey>
    : EntitySet<TModel>,
      IReadOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected ReadOnlyRepository(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy)
        : base(expression, source, strategy) {
    }
    public ReadOnlyRepository(IRepositoryStrategy? strategy = null)
        : this(default, default, strategy) {
    }
    public ReadOnlyRepository(IEnumerable<TModel> source, IRepositoryStrategy? strategy = null)
        : this(default, IsNotNull(source), strategy) {
    }
    public ReadOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : this(IsNotNull(expression), default, strategy) {
    }

    public Task<int> CountAsync(CancellationToken ct = default)
        => Strategy.ExecuteAsync<int>("Count", Expression, ct);
    public Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => Strategy.ExecuteAsync<IReadOnlyList<TModel>>("ToArray", Expression, ct);
    public Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(default!, ct);
    public Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<bool>("Any", Expression, ct);
    public Task<TModel?> FindByKey(TKey key, CancellationToken ct = default)
        => Strategy.ExecuteAsync<TKey, TModel?>("FindByKey", key, Expression, ct);
    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => FindFirst(default!, ct);
    public Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<TModel?>("FindFirst", Expression, ct);
}

public class ReadOnlyRepository<TModel>
    : EntitySet<TModel>,
      IReadOnlyRepository<TModel>
    where TModel : class, IEntity, new() {
    protected ReadOnlyRepository(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy)
        : base(expression, source, strategy) {
    }
    public ReadOnlyRepository(IRepositoryStrategy? strategy = null)
        : this(default, default, strategy) {
    }
    public ReadOnlyRepository(IEnumerable<TModel> source, IRepositoryStrategy? strategy = null)
        : this(default, IsNotNull(source), strategy) {
    }
    public ReadOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : this(IsNotNull(expression), default, strategy) {
    }

    public Task<int> CountAsync(CancellationToken ct = default)
        => Strategy.ExecuteAsync<int>("Count", Expression, ct);
    public Task<IReadOnlyList<TModel>> ToArrayAsync(CancellationToken ct = default)
        => Strategy.ExecuteAsync<IReadOnlyList<TModel>>("ToArray", Expression, ct);
    public Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(default!, ct);
    public Task<bool> HaveAny(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<bool>("Any", Expression, ct);
    public Task<TModel?> FindByKey<TKey>(TKey key, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ExecuteAsync<TKey, TModel?>("FindByKey", key, Expression, ct);
    public Task<TModel?> FindFirst(CancellationToken ct = default)
        => FindFirst(default!, ct);
    public Task<TModel?> FindFirst(Expression<Func<TModel, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<TModel?>("FindFirst", Expression, ct);
}
