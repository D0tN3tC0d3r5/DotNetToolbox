namespace DotNetToolbox.Data.Repositories;

public class ItemSet {
    public static IItemSet Create(Type entityType, Expression expression, IRepositoryStrategy? strategy = null) {
        var setType = typeof(ItemSet<>).MakeGenericType(entityType);
        return (IItemSet)Activator.CreateInstance(setType, expression, strategy)!;
    }
    public static IItemSet Create(Type entityType, IEnumerable source, IRepositoryStrategy? strategy = null) {
        var setType = typeof(ItemSet<>).MakeGenericType(entityType);
        return (IItemSet)Activator.CreateInstance(setType, source, strategy)!;
    }
}

public class ItemSet<TEntity>
    : IItemSet<TEntity> {
    private readonly List<TEntity> _internal;
    private readonly Expression _expression;

    public ItemSet(IRepositoryStrategy? strategy = null) {
        _internal = [];
        _expression = Expression.Constant(this);
        Strategy = strategy ?? new InMemoryRepositoryStrategy<TEntity>(this);
    }
    public ItemSet(Expression expression, IRepositoryStrategy? strategy = null)
        : this(strategy) {
        if (_expression.Type.GetElementType() != typeof(TEntity))
            throw new InvalidCastException("Expression is of the wrong type.");
        _expression = expression;
    }
    public ItemSet(IEnumerable<TEntity> source, IRepositoryStrategy? strategy = null)
        : this(strategy) {
        _internal = [..source];
    }

    public IRepositoryStrategy Strategy { get; }
    public Type ElementType { get; } = typeof(TEntity);
    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => Strategy
            .ExecuteAsync<IAsyncEnumerable<TEntity>>(_internal.AsQueryable().Expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<TEntity> GetEnumerator()
        => _internal.GetEnumerator();

    Expression IQueryable.Expression => _expression;
    IQueryProvider IQueryable.Provider => _internal.AsQueryable().Provider;
}
