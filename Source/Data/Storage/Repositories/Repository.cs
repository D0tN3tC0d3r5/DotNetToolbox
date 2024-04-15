namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem>
    : IRepository<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data;

    public Repository(IStrategyProvider? provider = null)
        : this([], provider) {
    }

    internal Repository(IEnumerable<TItem> source, IQueryStrategy provider) {
        var list = source.ToList();
        _data = IsNotNull(list).AsQueryable();
        Provider = IsOfType<IRepositoryStrategy<TItem>>(provider);
    }

    public Repository(IEnumerable<TItem> source, IStrategyProvider? provider = null) {
        var list = source.ToList();
        _data = IsNotNull(list).AsQueryable();
        Provider = (IRepositoryStrategy<TItem>?)provider?.GetStrategy<TItem>()
                ?? new InMemoryRepositoryStrategy<TItem>(list);
    }

    public Type ElementType => _data.ElementType;
    public Expression Expression => _data.Expression;
    IQueryProvider IQueryable.Provider => Provider;
    IRepositoryStrategy IRepository.Provider => Provider;
    public IRepositoryStrategy<TItem> Provider { get; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TItem> GetEnumerator() => _data.GetEnumerator();

    public bool HaveAny() => Provider.HaveAny();
    public int Count() => Provider.Count();
    public TItem[] ToArray() => Provider.ToArray();
    public TItem? GetFirst() => Provider.GetFirst();

    public void Add(TItem newItem) => Provider.Add(newItem);
    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Provider.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Provider.Remove(predicate);
}
