namespace DotNetToolbox.Data.Repositories;

public class Repository<TEntity>
    : InsertOnlyRepository<TEntity>,
      IRepository<TEntity> {
    public Repository(IRepositoryStrategy? strategy = null)
        : base(strategy) {
    }
    public Repository(Expression expression, IRepositoryStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public Repository(IEnumerable<TEntity> source, IRepositoryStrategy? strategy = null)
        : base(source, strategy) {
    }
    public virtual Task Update(TEntity input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Update", input, ct);

    public virtual Task<TEntity?> Patch(Expression<Func<TEntity, bool>> predicate, Action<TEntity> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync<(Expression<Func<TEntity, bool>>, Action<TEntity>), TEntity?>("Patch", (predicate, setModel), ct);
    public virtual Task<TEntity> PatchOrCreate(Expression<Func<TEntity, bool>> predicate, Action<TEntity> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync<(Expression<Func<TEntity, bool>>, Action<TEntity>), TEntity>("PatchOrCreate", (predicate, setModel), ct);

    public virtual Task Remove(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Remove", predicate, ct);
}
