namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyAsyncRepository<TItem>
    : ReadOnlyAsyncRepository<TItem, InMemoryAsyncRepositoryStrategy<TItem>>,
      IAsyncItemSet<TItem> {
    public ReadOnlyAsyncRepository() {
    }
    public ReadOnlyAsyncRepository(Expression expression)
        : base(expression) {
    }
    public ReadOnlyAsyncRepository(IEnumerable<TItem> data)
        : base(data) {
    }
}

public class ReadOnlyAsyncRepository<TItem, TStrategy>
    : ItemSet<TItem, TStrategy>,
      IAsyncRepository<TItem, TStrategy>
    where TStrategy : class, IAsyncRepositoryStrategy<TStrategy> {
    public ReadOnlyAsyncRepository(TStrategy? strategy = null)
        : base(strategy) {
    }
    public ReadOnlyAsyncRepository(Expression expression, TStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public ReadOnlyAsyncRepository(IEnumerable<TItem> data, TStrategy? strategy = null)
        : base(data, strategy) {
    }
    public virtual Task<TItem[]> GetList(CancellationToken ct = default)
        => Strategy.ExecuteFunction<TItem[]>("GetList", default, ct);
    public virtual Task<int> Count(CancellationToken ct = default)
        => CountWhere(_ => true, ct);
    public virtual Task<int> CountWhere(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteFunction<int>("Count", predicate, ct);
    public virtual Task<bool> HaveAny(CancellationToken ct = default)
        => HaveAnyWhere(_ => true, ct);
    public virtual Task<bool> HaveAnyWhere(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteFunction<bool>("Any", predicate, ct);
    public virtual Task<TItem?> FindFirst(CancellationToken ct = default)
        => FindFirstWhere(_ => true, ct);
    public virtual Task<TItem?> FindFirstWhere(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.ExecuteFunction<TItem?>("FindFirst", predicate, ct);
}
