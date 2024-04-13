namespace DotNetToolbox.Data.Repositories;

public static class AsyncItemSet {
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

public abstract class AsyncItemSet<TItem>(IEnumerable<TItem> data, Expression? expression)
    : AsyncItemSet<TItem, InMemoryAsyncRepositoryStrategy<TItem>>(data, expression),
      IAsyncItemSet<TItem>;

public abstract class AsyncItemSet<TItem, TStrategy>(IEnumerable<TItem> data, Expression? expression, TStrategy? strategy = null)
    : ItemSet<TItem, TStrategy>(IsNotNull(data), expression, strategy),
      IAsyncItemSet<TItem, TStrategy>
    where TStrategy : class, IAsyncQueryStrategy<TStrategy> {
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => Strategy
            .ExecuteQuery<IAsyncEnumerable<TItem>>((LambdaExpression)Query.Expression, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);
}

