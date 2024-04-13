namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepository<TItem>
    : ItemSet<TItem> {
    public ReadOnlyRepository(IEnumerable<TItem>? data = null)
        : base(data ?? [], null) {
    }
    public ReadOnlyRepository(Expression expression)
        : base([], expression) {
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
        : base([], null, strategy) {
    }
    public ReadOnlyRepository(Expression expression, TStrategy? strategy = null)
        : base([], expression, strategy) {
    }
    public ReadOnlyRepository(IEnumerable<TItem> data, TStrategy? strategy = null)
        : base(data, null, strategy) {
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
