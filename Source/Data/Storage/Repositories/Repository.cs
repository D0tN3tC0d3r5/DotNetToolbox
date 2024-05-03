namespace DotNetToolbox.Data.Repositories;

public abstract class Repository<TStrategy, TItem>
    : IRepository<TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {

    protected Repository(TStrategy strategy, IEnumerable<TItem>? data = null)
        : this($"|>{nameof(Repository<TStrategy, TItem>)}_{Guid.NewGuid():N}<|", strategy, data) {
    }

    protected Repository(string name, TStrategy strategy, IEnumerable<TItem>? data = null) {
        Name = name;
        Strategy = IsNotNull(strategy);
        if (data is null)
            return;
        var list = data as List<TItem> ?? data.ToList();
        if (list.Count == 0)
            return;
        Strategy.Seed(list);
    }

    public string Name { get; }

    protected TStrategy Strategy { get; init; }
    public Type ElementType => Strategy.ElementType;
    public Expression Expression => Strategy.Expression;

    #region Blocking

    IEnumerator IEnumerable.GetEnumerator() => Strategy.GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Strategy.GetEnumerator();
    public IQueryProvider Provider => Strategy.Provider;

    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);
    public void Load()
        => Strategy.Load();

    #endregion

    #region Async

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => Strategy.GetAsyncEnumerator(ct);
    public IAsyncQueryProvider AsyncProvider
        => Strategy.AsyncProvider;

    public Task SeedAsync(IEnumerable<TItem> seed, CancellationToken ct = default)
        => Strategy.SeedAsync(seed, ct);
    public Task LoadAsync(CancellationToken ct = default)
        => Strategy.LoadAsync(ct);

    #endregion
}
