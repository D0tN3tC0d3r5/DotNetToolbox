namespace DotNetToolbox.Data.Repositories;

public static class ItemSet {
    public static IItemSet Create(Type itemType, Expression expression) {
        var setType = typeof(ItemSet<>).MakeGenericType(itemType);
        return (IItemSet)Activator.CreateInstance(setType, Array.CreateInstance(itemType, 0), expression)!;
    }
    public static IItemSet Create(Type itemType, IEnumerable source) {
        var setType = typeof(ItemSet<>).MakeGenericType(itemType);
        return (IItemSet)Activator.CreateInstance(setType, source, null)!;
    }
    // ReSharper disable once SuspiciousTypeConversion.Global
    public static IItemSet<TItem> Create<TItem>(Expression expression)
        => (IItemSet<TItem>)Create(typeof(TItem), expression);
    // ReSharper disable once SuspiciousTypeConversion.Global
    public static IItemSet<TItem> Create<TItem>(IEnumerable<TItem> source)
        => (IItemSet<TItem>)Create(typeof(TItem), source);

    public static IItemSet Create<TStrategy>(Type itemType, Expression expression, TStrategy? strategy = null)
        where TStrategy : class, IRepositoryStrategy<TStrategy> {
        var setType = typeof(ItemSet<,>).MakeGenericType(itemType, typeof(TStrategy));
        return (IItemSet)Activator.CreateInstance(setType, expression, strategy)!;
    }
    public static IItemSet Create<TStrategy>(Type itemType, IEnumerable source, TStrategy? strategy = null)
        where TStrategy : class, IRepositoryStrategy<TStrategy> {
        var setType = typeof(ItemSet<,>).MakeGenericType(itemType, typeof(TStrategy));
        return (IItemSet)Activator.CreateInstance(setType, source, strategy)!;
    }
    // ReSharper disable once SuspiciousTypeConversion.Global
    public static IItemSet<TItem> Create<TItem, TStrategy>(Expression expression, TStrategy? strategy = null)
        where TStrategy : class, IRepositoryStrategy<TStrategy>
        => (IItemSet<TItem>)Create<TStrategy>(typeof(TItem), expression, strategy);
    // ReSharper disable once SuspiciousTypeConversion.Global
    public static IItemSet<TItem> Create<TItem, TStrategy>(IEnumerable<TItem> source, TStrategy? strategy = null)
        where TStrategy : class, IRepositoryStrategy<TStrategy>
        => (IItemSet<TItem>)Create<TStrategy>(typeof(TItem), source, strategy);
}

public class ItemSet<TItem>(IEnumerable<TItem> data, Expression? expression)
    : ItemSet<TItem, InMemoryRepositoryStrategy<TItem>>(IsNotNull(data), expression),
      IItemSet<TItem>;

public class ItemSet<TItem, TStrategy>(IEnumerable<TItem> data,
                                                Expression? expression,
                                                TStrategy? strategy = null)
    : Queryable<TItem>(IsNotNull(data), expression),
      IItemSet<TItem, TStrategy>
    where TStrategy : class, IQueryStrategy {
    public TStrategy Strategy
        => strategy ??= InstanceFactory.Create<TStrategy>(this);
}
