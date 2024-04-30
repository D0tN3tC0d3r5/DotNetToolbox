namespace DotNetToolbox.Data.Strategies;

public abstract class RepositoryStrategy
    : IRepositoryStrategy;

public abstract class RepositoryStrategy<TItem>
    : RepositoryStrategy,
    IRepositoryStrategy<TItem> {

    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;
    protected List<TItem> OriginalData { get; private set; } = [];

    #region Blocking

    protected IQueryable<TItem> Query => OriginalData.AsQueryable();
    public IQueryProvider Provider => Query.Provider;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public virtual void Seed(IEnumerable<TItem> seed)
        => OriginalData = seed as List<TItem> ?? IsNotNull(seed).ToList();

    public virtual void Add(TItem newItem) => throw new NotImplementedException();
    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) => throw new NotImplementedException();
    public virtual void Remove(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    #endregion

    #region Async

    protected IAsyncQueryable<TItem> AsyncQuery => OriginalData.AsAsyncQueryable();
    public IAsyncQueryProvider AsyncProvider => AsyncQuery.AsyncProvider;

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default) => AsyncQuery.GetAsyncEnumerator(ct);

    public virtual Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default) {
        Seed(seed);
        return Task.CompletedTask;
    }

    public virtual Task AddAsync(TItem newItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) => throw new NotImplementedException();
    public virtual Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();

    #endregion
}
