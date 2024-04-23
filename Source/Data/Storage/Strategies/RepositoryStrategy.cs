namespace DotNetToolbox.Data.Strategies;

public abstract partial class RepositoryStrategy
    : IRepositoryStrategy;

public abstract partial class RepositoryStrategy<TItem>
    : RepositoryStrategy,
    IRepositoryStrategy<TItem> {

    protected RepositoryStrategy()
        : this([]) {
    }

    protected RepositoryStrategy(IEnumerable<TItem> data) {
        OriginalData = data.ToList();
        Query = OriginalData.AsQueryable();
    }

    IQueryable IRepositoryStrategy<TItem>.Query => Query;
    protected IQueryable<TItem> Query { get; set; }
    protected IEnumerable<TItem> OriginalData { get; set; }
    protected List<TItem> UpdatableData => IsOfType<List<TItem>>(OriginalData);

    public virtual void Seed(IEnumerable<TItem> seed)
        => throw new NotImplementedException();
}
