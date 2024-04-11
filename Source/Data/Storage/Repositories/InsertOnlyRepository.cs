namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepository<TEntity>
    : ReadOnlyRepository<TEntity>,
      IInsertOnlyRepository<TEntity> {
    public InsertOnlyRepository(IRepositoryStrategy? strategy = null)
        : base(strategy) {
    }
    public InsertOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public InsertOnlyRepository(IEnumerable<TEntity> source, IRepositoryStrategy? strategy = null)
        : base(source, strategy) {
    }
    public virtual Task Add(TEntity input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Add", input, ct);
    public virtual Task AddOrUpdate(TEntity input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("AddOrUpdate", input, ct);
    public virtual Task<TEntity> Create(Action<TEntity> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync<Action<TEntity>, TEntity>("Create", setModel, ct);
    public Task<object> GenerateKey(CancellationToken ct = default)
        => Strategy.ExecuteAsync<object>("GenerateKey", ct);
}
