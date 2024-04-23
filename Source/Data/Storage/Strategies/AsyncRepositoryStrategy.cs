namespace DotNetToolbox.Data.Strategies;

public abstract partial class AsyncRepositoryStrategy
    : IAsyncRepositoryStrategy;

public abstract partial class AsyncRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy,
    IAsyncRepositoryStrategy<TItem> {

    protected AsyncRepositoryStrategy()
        : this([]) {
    }

    protected AsyncRepositoryStrategy(IEnumerable<TItem> data) {
        OriginalData = data.ToList();
        Query = OriginalData.AsQueryable();
    }

    IQueryable IAsyncRepositoryStrategy<TItem>.Query => Query;
    protected IQueryable<TItem> Query { get; set; }
    protected IEnumerable<TItem> OriginalData { get; set; }
    protected List<TItem> UpdatableData => IsOfType<List<TItem>>(OriginalData);

    public virtual Task SeedAsync(IEnumerable<TItem> seed)
        => throw new NotImplementedException();
}
