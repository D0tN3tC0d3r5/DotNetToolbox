namespace DotNetToolbox.Data.Repositories;

public class AddOnlyRepository<TItem>
    : ReadOnlyRepository<TItem>,
      ICanAddItem<TItem> {
    public AddOnlyRepository() {
    }
    public AddOnlyRepository(Expression expression)
        : base(expression) {
    }
    public AddOnlyRepository(IEnumerable<TItem> data)
        : base(data) {
    }
    public virtual void Add(TItem item)
        => Strategy.ExecuteAction("Add", item);
}

public class ReadOnlyRepository<TItem>
    : ItemSet<TItem> {
    public ReadOnlyRepository(){
    }
    public ReadOnlyRepository(Expression expression)
        : base(expression) {
    }
    public ReadOnlyRepository(IEnumerable<TItem> data)
        : base(data) {
    }
    public virtual TItem[] GetList()
        => Strategy.ExecuteFunction<TItem[]>("GetList");
    public virtual int Count()
        => CountWhere(_ => true);
    public virtual int CountWhere(Expression<Func<TItem, bool>> predicate)
        => Strategy.ExecuteFunction<int>("Count", predicate);
    public virtual bool HaveAny()
        => HaveAnyWhere(_ => true);
    public virtual bool HaveAnyWhere(Expression<Func<TItem, bool>> predicate)
        => Strategy.ExecuteFunction<bool>("Any", predicate);
    public virtual TItem? FindFirst()
        => FindFirstWhere(_ => true);
    public virtual TItem? FindFirstWhere(Expression<Func<TItem, bool>> predicate)
        => Strategy.ExecuteFunction<TItem?>("FindFirst", predicate);
}

public class ReadOnlyRepository<TItem, TStrategy>
    : ItemSet<TItem, TStrategy>,
      IRepository<TItem, TStrategy>
    where TStrategy : class, IRepositoryStrategy<TStrategy> {
    public ReadOnlyRepository(TStrategy? strategy = null)
        : base(strategy) {
    }
    public ReadOnlyRepository(Expression expression, TStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public ReadOnlyRepository(IEnumerable<TItem> data, TStrategy? strategy = null)
        : base(data, strategy) {
    }
    public virtual TItem[] GetList()
        => Strategy.ExecuteFunction<TItem[]>("GetList");
    public virtual int Count()
        => CountWhere(_ => true);
    public virtual int CountWhere(Expression<Func<TItem, bool>> predicate)
        => Strategy.ExecuteFunction<int>("Count", predicate);
    public virtual bool HaveAny()
        => HaveAnyWhere(_ => true);
    public virtual bool HaveAnyWhere(Expression<Func<TItem, bool>> predicate)
        => Strategy.ExecuteFunction<bool>("Any", predicate);
    public virtual TItem? FindFirst()
        => FindFirstWhere(_ => true);
    public virtual TItem? FindFirstWhere(Expression<Func<TItem, bool>> predicate)
        => Strategy.ExecuteFunction<TItem?>("FindFirst", predicate);
}
