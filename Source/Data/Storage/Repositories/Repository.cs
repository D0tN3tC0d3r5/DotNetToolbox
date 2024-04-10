namespace DotNetToolbox.Data.Repositories;

public class Repository<TModel>
    : InsertOnlyRepository<TModel>,
      IRepository<TModel>
    where TModel : class, IEntity, new() {
    protected Repository(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy)
        : base(expression, source, strategy) {
    }
    public Repository(IRepositoryStrategy? strategy = null)
        : this(default, default, strategy) {
    }
    public Repository(IEnumerable<TModel> source, IRepositoryStrategy? strategy = null)
        : this(default, IsNotNull(source), strategy) {
    }
    public Repository(Expression expression, IRepositoryStrategy? strategy = null)
        : this(IsNotNull(expression), default, strategy) {
    }
    public Task Update(TModel input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Update", input, Expression, ct);
    public Task AddOrUpdate(TModel input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("AddOrUpdate", input, Expression, ct);

    public Task Patch<TKey>(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ExecuteAsync("Patch", (key, setModel), Expression, ct);
    public Task CreateOrPatch<TKey>(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ExecuteAsync("CreateOrPatch", (key, setModel), Expression, ct);

    public Task Remove<TKey>(TKey key, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ExecuteAsync("Remove", key, Expression, ct);
}

public class Repository<TModel, TKey>
    : InsertOnlyRepository<TModel, TKey>,
      IRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected Repository(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy)
        : base(expression, source, strategy) {
    }
    public Repository(IRepositoryStrategy? strategy = null)
        : this(default, default, strategy) {
    }
    public Repository(IEnumerable<TModel> source, IRepositoryStrategy? strategy = null)
        : this(default, IsNotNull(source), strategy) {
    }
    public Repository(Expression expression, IRepositoryStrategy? strategy = null)
        : this(IsNotNull(expression), default, strategy) {
    }
    public Task Update(TModel input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Update", input, Expression, ct);
    public Task AddOrUpdate(TModel input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("AddOrUpdate", input, Expression, ct);

    public Task Patch(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Patch", (key, setModel), Expression, ct);
    public Task CreateOrPatch(TKey key, Action<TModel> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("CreateOrPatch", (key, setModel), Expression, ct);

    public Task Remove(TKey key, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Remove", key, Expression, ct);
}
