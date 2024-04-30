using System.Diagnostics;

namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem>
    : Repository<IRepositoryStrategy<TItem>, TItem> {
    public Repository(IEnumerable<TItem>? data = null)
        : base(new InMemoryRepositoryStrategy<TItem>(), data) { }
    public Repository(IRepositoryStrategyProvider provider, IEnumerable<TItem>? data = null)
        : base(IsNotNull(provider).GetStrategyFor<TItem>(), data) { }

    public Repository(IRepositoryStrategy<TItem> strategy, IEnumerable<TItem>? data = null)
        : base(strategy, data ?? []) {
    }
}

public abstract class Repository<TStrategy, TItem>
    : IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    protected Repository(TStrategy strategy, IEnumerable<TItem>? data = null) {
        Strategy = IsNotNull(strategy);
        if (data is null) return;
        var list = data as List<TItem> ?? data.ToList();
        Strategy.Seed(list);
    }

    protected TStrategy Strategy { get; init; }
    public Type ElementType => Strategy.ElementType;
    public Expression Expression => Strategy.Expression;

    #region Blocking

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Strategy).GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Strategy.GetEnumerator();
    public IQueryProvider Provider => Strategy.Provider;

    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public void Add(TItem newItem) {
        Debug.Assert(true);
        Strategy.Add(newItem);
        Debug.Assert(true);
    }

    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);

    #endregion

    #region Async

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => Strategy.GetAsyncEnumerator(ct);

    public IAsyncQueryProvider AsyncProvider => Strategy.AsyncProvider;

    public Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => Strategy.SeedAsync(seed, ct);

    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);

    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);
    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);

    #endregion
}
