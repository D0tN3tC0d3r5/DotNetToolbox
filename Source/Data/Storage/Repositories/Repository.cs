namespace DotNetToolbox.Data.Repositories;

public class Repository<TEntity, TKey>
    : InsertOnlyRepository<TEntity, TKey>,
      IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>, new()
    where TKey : notnull {
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
    public virtual Task AddOrUpdate(TEntity input, CancellationToken ct = default)
        => Strategy.ExecuteAsync("AddOrUpdate", input, ct);

    public virtual Task Patch(Func<CancellationToken, Task<TEntity?>> find, Action<TEntity> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Patch", (find, setModel), ct);
    public virtual Task CreateOrPatch(Func<CancellationToken, Task<TEntity?>> find, Action<TEntity> setModel, CancellationToken ct = default)
        => Strategy.ExecuteAsync("CreateOrPatch", (find, setModel), ct);

    public virtual Task Remove(Func<CancellationToken, Task<TEntity?>> find, CancellationToken ct = default)
        => Strategy.ExecuteAsync("Remove", find, ct);
    public Task Patch(TKey key, Action<TEntity> setModel, CancellationToken ct = default)
        => Patch(t => FindByKey(key, t), setModel, ct);
    public Task CreateOrPatch(TKey key, Action<TEntity> setModel, CancellationToken ct = default)
        => CreateOrPatch(t => FindByKey(key, t), setModel, ct);
    public Task Remove(TKey key, CancellationToken ct = default)
        => Remove(t => FindByKey(key, t), ct);
}
