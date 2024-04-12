namespace DotNetToolbox.Data.Repositories;

public class ItemSet<TItem>
    : ItemSet<TItem, InMemoryRepositoryStrategy<TItem>>,
      IItemSet<TItem> {
    public ItemSet() { }

    public ItemSet(Expression expression)
        : base(expression) {
    }
    public ItemSet(IEnumerable<TItem> data)
        : base(data) {
    }
}

public class ItemSet<TItem, TStrategy>
    : Queryable<TItem>,
      IItemSet<TItem, TStrategy>
    where TStrategy : class, IQueryStrategy {
    private TStrategy? _strategy;

    public ItemSet(TStrategy? strategy = null)
        : base(null, null) {
        _strategy = strategy;
    }
    public ItemSet(Expression expression, TStrategy? strategy = null)
        : base(null, expression) {
        _strategy = strategy;
    }
    public ItemSet(IEnumerable<TItem> data, TStrategy? strategy = null)
        : base(data, null) {
        _strategy = strategy;
    }

    public TStrategy Strategy => _strategy ??= InstanceFactory.Create<TStrategy>(this);
}
