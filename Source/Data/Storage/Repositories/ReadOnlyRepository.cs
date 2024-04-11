namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepository<TEntity>
    : ItemSet<TEntity>,
      IReadOnlyRepository<TEntity> {
    public ReadOnlyRepository(IRepositoryStrategy? strategy = null)
        : base(strategy) {
    }
    public ReadOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public ReadOnlyRepository(IEnumerable<TEntity> source, IRepositoryStrategy? strategy = null)
        : base(source, strategy) {
    }
    public virtual Task<TEntity[]> GetList(CancellationToken ct = default)
        => Strategy.ExecuteAsync<TEntity[]>("GetList", ct);
    public virtual Task<int> Count(CancellationToken ct = default)
        => Count(_ => true, ct);
    public virtual Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<int>("Count", predicate, ct);
    public virtual Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(_ => true, ct);
    public virtual Task<bool> HaveAny(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<bool>("Any", predicate, ct);
    public virtual Task<TEntity?> FindFirst(CancellationToken ct = default)
        => FindFirst(_ => true, ct);
    public virtual Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<TEntity?>("FindFirst", predicate, ct);
}
