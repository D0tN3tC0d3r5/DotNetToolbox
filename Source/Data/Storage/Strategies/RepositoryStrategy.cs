namespace DotNetToolbox.Data.Strategies;

public abstract class RepositoryStrategy
    : IRepositoryStrategy;

public abstract class RepositoryStrategy<TItem>
    : RepositoryStrategy,
    IRepositoryStrategy<TItem> {

    protected RepositoryStrategy()
        : this([]) {
    }

    protected RepositoryStrategy(IEnumerable<TItem> data) {
        OriginalData = data.ToList();
        Query = new EnumerableQuery<TItem>(OriginalData);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => Query.GetEnumerator();

    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;
    public IQueryProvider Provider => Query.Provider;

    protected IQueryable<TItem> Query { get; set; }
    protected IEnumerable<TItem> OriginalData { get; set; }
    protected List<TItem> UpdatableData => IsOfType<List<TItem>>(OriginalData);

    public virtual void Seed(IEnumerable<TItem> seed)
        => throw new NotImplementedException();

    public virtual void Add(TItem newItem)
        => throw new NotImplementedException();

    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => throw new NotImplementedException();

    public virtual void Remove(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();
}
