namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepository<TEntity, TKey>
    : EntitySet<TEntity>,
      IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>, new()
    where TKey : notnull {
    public ReadOnlyRepository(IRepositoryStrategy? strategy = null)
        : base(strategy) {
    }
    public ReadOnlyRepository(Expression expression, IRepositoryStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public ReadOnlyRepository(IEnumerable<TEntity> source, IRepositoryStrategy? strategy = null)
        : base(source, strategy) {
    }
    public virtual Task<IReadOnlyList<TEntity>> GetListAsync(CancellationToken ct = default)
        => Strategy.ExecuteAsync<IReadOnlyList<TEntity>>("GetList", ct);
    public virtual Task<int> CountAsync(CancellationToken ct = default)
        => CountAsync(_ => true, ct);
    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<int>("Count", predicate, ct);
    public virtual Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAny(_ => true, ct);
    public virtual Task<bool> HaveAny(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<bool>("Any", predicate, ct);
    public virtual Task<TEntity?> FindByKey(TKey key, CancellationToken ct = default)
        => FindFirst(m => m.Id.Equals(key), ct);
    public virtual Task<TEntity?> FindFirst(CancellationToken ct = default)
        => FindFirst(_ => true, ct);
    public virtual Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteAsync<TEntity?>("FindFirst", predicate, ct);
}
