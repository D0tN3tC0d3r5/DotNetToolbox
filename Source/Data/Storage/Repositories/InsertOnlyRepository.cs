namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepository<TModel, TKey>
    : ReadOnlyRepository<TModel, TKey>,
      IInsertOnlyRepository<TModel, TKey>
    where TModel : class, IEntity<TKey>, new()
    where TKey : notnull {
    protected InsertOnlyRepository(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy)
        : base(expression, source, strategy) {
    }
    public InsertOnlyRepository(IRepositoryStrategy? strategy = null)
        : this(default, default, strategy) {
    }
    public InsertOnlyRepository(IEnumerable<TModel> source, IRepositoryStrategy? strategy = null)
        : this(default, IsNotNull(source), strategy) {
    }
    public InsertOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : this(IsNotNull(expression), default, strategy) {
    }
    public Task Add(TModel input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Add", input, Expression, ct);
    public Task Create(Action<TModel> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Create", setModel, Expression, ct);
}

public class InsertOnlyRepository<TModel>
    : ReadOnlyRepository<TModel>,
      IInsertOnlyRepository<TModel>
    where TModel : class, IEntity, new() {
    protected InsertOnlyRepository(Expression? expression, IEnumerable<TModel>? source, IRepositoryStrategy? strategy)
        : base(expression, source, strategy) {
    }
    public InsertOnlyRepository(IRepositoryStrategy? strategy = null)
        : this(default, default, strategy) {
    }
    public InsertOnlyRepository(IEnumerable<TModel> source, IRepositoryStrategy? strategy = null)
        : this(default, IsNotNull(source), strategy) {
    }
    public InsertOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : this(IsNotNull(expression), default, strategy) {
    }
    public Task Add(TModel input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Add", input, Expression, ct);
    public Task Create(Action<TModel> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Create", setModel, Expression, ct);
}
