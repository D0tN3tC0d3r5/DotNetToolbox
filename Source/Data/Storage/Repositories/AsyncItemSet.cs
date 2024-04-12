namespace DotNetToolbox.Data.Repositories;

public class AsyncItemSet {
    public static IItemSet Create(Type entityType, Expression expression) {
        var strategyType = typeof(InMemoryAsyncRepositoryStrategy<>).MakeGenericType(entityType);
        var setType = typeof(ItemSet<,>).MakeGenericType(entityType, strategyType);
        return (IItemSet)Activator.CreateInstance(setType, expression)!;
    }
    public static IItemSet Create<TStrategy>(Type entityType, Expression expression, TStrategy? strategy = null)
        where TStrategy : class, IAsyncRepositoryStrategy<TStrategy> {
        var setType = typeof(ItemSet<,>).MakeGenericType(entityType, typeof(TStrategy));
        return (IItemSet)Activator.CreateInstance(setType, expression, strategy)!;
    }
    public static IItemSet Create(Type entityType, IEnumerable source) {
        var strategyType = typeof(InMemoryAsyncRepositoryStrategy<>).MakeGenericType(entityType);
        var setType = typeof(ItemSet<,>).MakeGenericType(entityType, strategyType);
        return (IItemSet)Activator.CreateInstance(setType, source)!;
    }
    public static IItemSet Create<TStrategy>(Type entityType, IEnumerable source, TStrategy? strategy = null)
        where TStrategy : class, IAsyncRepositoryStrategy<TStrategy> {
        var setType = typeof(ItemSet<,>).MakeGenericType(entityType, typeof(TStrategy));
        return (IItemSet)Activator.CreateInstance(setType, source, strategy)!;
    }
}

public class AsyncItemSet<TItem>
    : AsyncItemSet<TItem, InMemoryAsyncRepositoryStrategy<TItem>>,
      IAsyncItemSet<TItem> {
    public AsyncItemSet() {
    }
    public AsyncItemSet(Expression expression)
        : base(expression) {
    }
    public AsyncItemSet(IEnumerable<TItem> data)
        : base(data) {
    }
}

public class AsyncItemSet<TItem, TStrategy>
    : ItemSet<TItem, TStrategy>,
      IAsyncItemSet<TItem, TStrategy>
    where TStrategy : class, IAsyncQueryStrategy<TStrategy> {
    public AsyncItemSet(TStrategy? strategy = null)
        : base(strategy) {
    }
    public AsyncItemSet(Expression expression, TStrategy? strategy = null)
        : base(expression, strategy) {
    }
    public AsyncItemSet(IEnumerable<TItem> data, TStrategy? strategy = null)
        : base(data, strategy) {
    }

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => Strategy
            .ExecuteQuery<IAsyncEnumerable<TItem>>(Data.AsQueryable().Expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
}

