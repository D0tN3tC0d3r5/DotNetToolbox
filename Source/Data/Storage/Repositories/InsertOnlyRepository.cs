namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepository<TEntity, TKey>
    : ReadOnlyRepository<TEntity, TKey>,
      IInsertOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>, new()
    where TKey : notnull {
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
    public virtual Task Create(Action<TEntity> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Create", setModel, ct);
    public Task<TKey> GenerateKey(CancellationToken ct = default)
        => Strategy.ExecuteAsync<TKey>("GenerateKey", ct);
}
