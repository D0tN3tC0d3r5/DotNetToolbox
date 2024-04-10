namespace DotNetToolbox.Data.Repositories;

public class EntitySet {
    public static IEntitySet Create(Type entityType, Expression expression, IRepositoryStrategy? strategy = null) {
        var setType = typeof(EntitySet<>).MakeGenericType(entityType);
        return (IEntitySet)Activator.CreateInstance(setType, expression, strategy)!;
    }
    public static IEntitySet Create(Type entityType, IEnumerable source, IRepositoryStrategy? strategy = null)
        {
        var setType = typeof(EntitySet<>).MakeGenericType(entityType);
        return (IEntitySet)Activator.CreateInstance(setType, source, strategy)!;
    }
}

public class EntitySet<TEntity>
    : IEntitySet<TEntity> {
    private readonly EnumerableQuery<TEntity> _proxy;

    public EntitySet(IRepositoryStrategy? strategy = null) {
        _proxy = new(Expression.Constant(this));
        Strategy = strategy ?? new InMemoryRepositoryStrategy();
    }
    public EntitySet(Expression expression, IRepositoryStrategy? strategy = null)
        : this(strategy) {
        _proxy = new(expression);
    }
    public EntitySet(IEnumerable<TEntity> source, IRepositoryStrategy? strategy = null)
        : this(strategy) {
        _proxy = new(source);
    }

    public IRepositoryStrategy Strategy { get; }
    public Type ElementType { get; } = typeof(TEntity);
    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => Strategy
            .ExecuteAsync<IAsyncEnumerable<TEntity>>(_proxy.AsQueryable().Expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TEntity> GetEnumerator()
        => Strategy.ExecuteAsync<IEnumerable<TEntity>>(_proxy.AsQueryable().Expression, default).GetEnumerator();
    Expression IQueryable.Expression => _proxy.AsQueryable().Expression;
    IQueryProvider IQueryable.Provider => _proxy.AsQueryable().Provider;
}
