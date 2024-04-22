namespace DotNetToolbox.Data.Strategies;

public abstract partial class RepositoryStrategy
    : IRepositoryStrategy;

public abstract partial class RepositoryStrategy<TItem>
    : RepositoryStrategy,
    IRepositoryStrategy<TItem> {

    internal RepositoryStrategy(IEnumerable data, IQueryable? query = null) {
        OriginalData = data;
        Query = query ?? OriginalData.AsQueryable();
    }

    protected RepositoryStrategy() {
        OriginalData = new List<TItem>();
        Query = OriginalData.Cast<TItem>().AsQueryable();
    }

    protected IEnumerable OriginalData { get; set; }
    protected IQueryable Query { get; set; }
    public virtual void Seed(IEnumerable<TItem> seed)
        => throw new NotImplementedException();
}
