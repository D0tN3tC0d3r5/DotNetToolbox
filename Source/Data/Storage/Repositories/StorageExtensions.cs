namespace DotNetToolbox.Data.Repositories;

public static class StorageExtensions {
    // ReSharper disable UnusedMember.Global
    public static IItemSet<TEntity, TStrategy> AsRepository<TEntity, TStrategy>(this IEnumerable<TEntity> source)
        where TStrategy : IQueryStrategy<TStrategy> {
        IsNotNull(source);
        return source as IItemSet<TEntity, TStrategy>
            ?? throw new NotSupportedException("This collection does not support the repository implementation.");
    }

    public static IItemSet<TEntity, TStrategy> ToReadOnlyRepository<TEntity, TStrategy>(this IEnumerable<TEntity> source, TStrategy? strategy = null)
        where TEntity : class, IEntity
        where TStrategy : class, IRepositoryStrategy<TStrategy>
        => new ReadOnlyRepository<TEntity, TStrategy>(source, strategy);

    public static IItemSet<TEntity, TStrategy> ToReadOnlyAsyncRepository<TEntity, TStrategy>(this IEnumerable<TEntity> source, TStrategy? strategy = null)
        where TEntity : class, IEntity
        where TStrategy : class, IAsyncRepositoryStrategy<TStrategy>
        => new ReadOnlyAsyncRepository<TEntity, TStrategy>(source, strategy);

    public static IItemSet<TResult> Where<TResult>(this IItemSet<TResult> source, Expression<Func<TResult, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Where, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> Where<TResult>(this IItemSet<TResult> source, Expression<Func<TResult, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Where, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> OfType<TResult>(this IItemSet source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.OfType<TResult>, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> Cast<TResult>(this IItemSet source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Cast<TResult>, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> Select<TSource, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Select, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> Select<TSource, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, int, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Select, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> SelectMany<TSource, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.SelectMany, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> SelectMany<TSource, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.SelectMany, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> SelectMany<TSource, TCollection, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(collectionSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.SelectMany, source, collectionSelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> SelectMany<TSource, TCollection, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(collectionSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.SelectMany, source, collectionSelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> Join<TOuter, TInner, TKey, TResult>(this IItemSet<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector) {
        IsNotNull(outer);
        // ReSharper disable PossibleMultipleEnumeration
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.Join, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        var args = new[] {
            outer.Expression,
            GetSourceExpression(inner),
            Expression.Quote(outerKeySelector),
            Expression.Quote(innerKeySelector),
            Expression.Quote(resultSelector),
        };
        var expression = Expression.Call(null, method, args);
        // ReSharper enable PossibleMultipleEnumeration
        return (IItemSet<TResult>)outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> Join<TOuter, TInner, TKey, TResult>(this IItemSet<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(outer);
        // ReSharper disable PossibleMultipleEnumeration
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.Join, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        var args = new[] {
            outer.Expression,
            GetSourceExpression(inner),
            Expression.Quote(outerKeySelector),
            Expression.Quote(innerKeySelector),
            Expression.Quote(resultSelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        // ReSharper enable PossibleMultipleEnumeration
        return (IItemSet<TResult>)outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IItemSet<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector) {
        IsNotNull(outer);
        // ReSharper disable PossibleMultipleEnumeration
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupJoin, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        var args = new[] {
            outer.Expression,
            GetSourceExpression(inner),
            Expression.Quote(outerKeySelector),
            Expression.Quote(innerKeySelector),
            Expression.Quote(resultSelector),
        };
        var expression = Expression.Call(null, method, args);
        // ReSharper enable PossibleMultipleEnumeration
        return (IItemSet<TResult>)outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IItemSet<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(outer);
        // ReSharper disable PossibleMultipleEnumeration
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupJoin, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        var args = new[] {
            outer.Expression,
            GetSourceExpression(inner),
            Expression.Quote(outerKeySelector),
            Expression.Quote(innerKeySelector),
            Expression.Quote(resultSelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        // ReSharper enable PossibleMultipleEnumeration
        return (IItemSet<TResult>)outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IOrderedItemSet<TSource> OrderBy<TSource, TKey>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderBy, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> OrderBy<TSource, TKey>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderBy, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> OrderByDescending<TSource, TKey>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderByDescending, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> OrderByDescending<TSource, TKey>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderByDescending, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> ThenBy<TSource, TKey>(this IOrderedItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenBy, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> ThenBy<TSource, TKey>(this IOrderedItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenBy, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> ThenByDescending<TSource, TKey>(this IOrderedItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenByDescending, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedItemSet<TSource> ThenByDescending<TSource, TKey>(this IOrderedItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenByDescending, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Take<TSource>(this IItemSet<TSource> source, int count) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Take, source, count);
        var args = new[] { source.Expression, Expression.Constant(count) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> TakeWhile<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.TakeWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> TakeWhile<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.TakeWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Skip<TSource>(this IItemSet<TSource> source, int count) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Skip, source, count);
        var args = new[] { source.Expression, Expression.Constant(count) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> SkipWhile<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.SkipWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> SkipWhile<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.SkipWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<IGrouping<TKey, TSource>>)source.Provider.CreateQuery<IGrouping<TKey, TSource>>(expression);
    }

    public static IItemSet<IGrouping<TKey, TEntity>> GroupBy<TSource, TKey, TEntity>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TEntity>> elementSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, elementSelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<IGrouping<TKey, TEntity>>)source.Provider.CreateQuery<IGrouping<TKey, TEntity>>(expression);
    }

    public static IItemSet<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<IGrouping<TKey, TSource>>)source.Provider.CreateQuery<IGrouping<TKey, TSource>>(expression);
    }

    public static IItemSet<IGrouping<TKey, TEntity>> GroupBy<TSource, TKey, TEntity>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TEntity>> elementSelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, elementSelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Quote(elementSelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<IGrouping<TKey, TEntity>>)source.Provider.CreateQuery<IGrouping<TKey, TEntity>>(expression);
    }

    public static IItemSet<TResult> GroupBy<TSource, TKey, TEntity, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TEntity>> elementSelector, Expression<Func<TKey, IEnumerable<TEntity>, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, elementSelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> GroupBy<TSource, TKey, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> GroupBy<TSource, TKey, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, resultSelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Quote(resultSelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TResult> GroupBy<TSource, TKey, TEntity, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TEntity>> elementSelector, Expression<Func<TKey, IEnumerable<TEntity>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, elementSelector, resultSelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Quote(elementSelector),
            Expression.Quote(resultSelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)source.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TSource> Distinct<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Distinct, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Distinct<TSource>(this IItemSet<TSource> source, IEqualityComparer<TSource> comparer) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Distinct, source, comparer);
        var args = new[] {
            source.Expression,
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Concat<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Concat, target, source);
        var args = new[] { target.Expression, GetSourceExpression(source) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TResult> Zip<TFirst, TSecond, TResult>(this IItemSet<TFirst> target, IEnumerable<TSecond> source, Expression<Func<TFirst, TSecond, TResult>> resultSelector) {
        IsNotNull(target);
        IsNotNull(source);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.Zip, target, source, resultSelector);
        var args = new[] { target.Expression, GetSourceExpression(source) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TResult>)target.Provider.CreateQuery<TResult>(expression);
    }

    public static IItemSet<TSource> Union<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Union, target, source);
        var args = new[] { target.Expression, GetSourceExpression(source) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Union<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source, IEqualityComparer<TSource> comparer) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Union, target, source, comparer);
        var args = new[] {
            target.Expression,
            GetSourceExpression(source),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Intersect<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Intersect, target, source);
        var args = new[] { target.Expression, GetSourceExpression(source) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Intersect<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source, IEqualityComparer<TSource> comparer) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Union, target, source, comparer);
        var args = new[] {
            target.Expression,
            GetSourceExpression(source),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Except<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Except, target, source);
        var args = new[] { target.Expression, GetSourceExpression(source) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> Except<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source, IEqualityComparer<TSource> comparer) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Except, target, source, comparer);
        var args = new[] {
            target.Expression,
            GetSourceExpression(source),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)target.Provider.CreateQuery<TSource>(expression);
    }

    public static TSource First<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.First, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource First<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.First, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource FirstOrDefault<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.FirstOrDefault, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource FirstOrDefault<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.FirstOrDefault, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource Last<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Last, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource Last<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Last, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource LastOrDefault<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.LastOrDefault, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource LastOrDefault<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.LastOrDefault, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource Single<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Single, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource Single<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Single, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource SingleOrDefault<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.SingleOrDefault, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource SingleOrDefault<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.SingleOrDefault, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource ElementAt<TSource>(this IItemSet<TSource> source, int index) {
        IsNotNull(source);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        var method = GetMethodInfo(Queryable.ElementAt, source, index);
        var args = new[] { source.Expression, Expression.Constant(index) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TSource ElementAtOrDefault<TSource>(this IItemSet<TSource> source, int index) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.ElementAtOrDefault, source, index);
        var args = new[] { source.Expression, Expression.Constant(index) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static IItemSet<TSource> DefaultIfEmpty<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.DefaultIfEmpty, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IItemSet<TSource> DefaultIfEmpty<TSource>(this IItemSet<TSource> source, TSource defaultValue) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.DefaultIfEmpty, source, defaultValue);
        var args = new[] { source.Expression, Expression.Constant(defaultValue, typeof(TSource)) };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static bool Contains<TSource>(this IItemSet<TSource> source, TSource item) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Contains, source, item);
        var args = new[] { source.Expression, Expression.Constant(item, typeof(TSource)) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static bool Contains<TSource>(this IItemSet<TSource> source, TSource item, IEqualityComparer<TSource> comparer) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Contains, source, item, comparer);
        var args = new[] {
            source.Expression,
            Expression.Constant(item, typeof(TSource)),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static IItemSet<TSource> Reverse<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Reverse, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return (IItemSet<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static bool SequenceEqual<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.SequenceEqual, target, source);
        var args = new[] { target.Expression, GetSourceExpression(source) };
        var expression = Expression.Call(null, method, args);
        return target.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static bool SequenceEqual<TSource>(this IItemSet<TSource> target, IEnumerable<TSource> source, IEqualityComparer<TSource> comparer) {
        IsNotNull(target);
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.SequenceEqual, target, source, comparer);
        var args = new[] {
            target.Expression,
            GetSourceExpression(source),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return target.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static bool Any<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Any, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static bool Any<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Any, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static bool All<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.All, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<bool>(GetCurrentMethodName(), expression);
    }

    public static int Count<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Count, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<int>(GetCurrentMethodName(), expression);
    }

    public static int Count<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Any, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<int>(GetCurrentMethodName(), expression);
    }

    public static long LongCount<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.LongCount, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<long>(GetCurrentMethodName(), expression);
    }

    public static long LongCount<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.LongCount, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<long>(GetCurrentMethodName(), expression);
    }

    public static TSource Min<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Min, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TResult Min<TSource, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Min, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TResult>(GetCurrentMethodName(), expression);
    }

    public static TSource Max<TSource>(this IItemSet<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Max, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TResult Max<TSource, TResult>(this IItemSet<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Min, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TResult>(GetCurrentMethodName(), expression);
    }

    public static int Sum(this IItemSet<int> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<int>(GetCurrentMethodName(), expression);
    }

    public static int? Sum(this IItemSet<int?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<int?>(GetCurrentMethodName(), expression);
    }

    public static long Sum(this IItemSet<long> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<long>(GetCurrentMethodName(), expression);
    }

    public static long? Sum(this IItemSet<long?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<long?>(GetCurrentMethodName(), expression);
    }

    public static float Sum(this IItemSet<float> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float>(GetCurrentMethodName(), expression);
    }

    public static float? Sum(this IItemSet<float?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float?>(GetCurrentMethodName(), expression);
    }

    public static double Sum(this IItemSet<double> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Sum(this IItemSet<double?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static decimal Sum(this IItemSet<decimal> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal>(GetCurrentMethodName(), expression);
    }

    public static decimal? Sum(this IItemSet<decimal?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal?>(GetCurrentMethodName(), expression);
    }

    public static int Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, int>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<int>(GetCurrentMethodName(), expression);
    }

    public static int? Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, int?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<int?>(GetCurrentMethodName(), expression);
    }

    public static long Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, long>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<long>(GetCurrentMethodName(), expression);
    }

    public static long? Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, long?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<long?>(GetCurrentMethodName(), expression);
    }

    public static float Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, float>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float>(GetCurrentMethodName(), expression);
    }

    public static float? Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, float?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float?>(GetCurrentMethodName(), expression);
    }

    public static double Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, double>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, double?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static decimal Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, decimal>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal>(GetCurrentMethodName(), expression);
    }

    public static decimal? Sum<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, decimal?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal?>(GetCurrentMethodName(), expression);
    }

    public static double Average(this IItemSet<int> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Average(this IItemSet<int?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static double Average(this IItemSet<long> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Average(this IItemSet<long?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static float Average(this IItemSet<float> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float>(GetCurrentMethodName(), expression);
    }

    public static float? Average(this IItemSet<float?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float?>(GetCurrentMethodName(), expression);
    }

    public static double Average(this IItemSet<double> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Average(this IItemSet<double?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static decimal Average(this IItemSet<decimal> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal>(GetCurrentMethodName(), expression);
    }

    public static decimal? Average(this IItemSet<decimal?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal?>(GetCurrentMethodName(), expression);
    }

    public static double Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, int>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, int?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static float Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, float>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float>(GetCurrentMethodName(), expression);
    }

    public static float? Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, float?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<float?>(GetCurrentMethodName(), expression);
    }

    public static double Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, long>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, long?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static double Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, double>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double>(GetCurrentMethodName(), expression);
    }

    public static double? Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, double?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<double?>(GetCurrentMethodName(), expression);
    }

    public static decimal Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, decimal>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal>(GetCurrentMethodName(), expression);
    }

    public static decimal? Average<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, decimal?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<decimal?>(GetCurrentMethodName(), expression);
    }

    public static TSource Aggregate<TSource>(this IItemSet<TSource> source, Expression<Func<TSource, TSource, TSource>> func) {
        IsNotNull(source);
        IsNotNull(func);
        var method = GetMethodInfo(Queryable.Aggregate, source, func);
        var args = new[] { source.Expression, Expression.Quote(func) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TSource>(GetCurrentMethodName(), expression);
    }

    public static TAccumulate Aggregate<TSource, TAccumulate>(this IItemSet<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func) {
        IsNotNull(source);
        IsNotNull(func);
        var method = GetMethodInfo(Queryable.Aggregate, source, seed, func);
        var args = new[] { source.Expression, Expression.Constant(seed), Expression.Quote(func) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TAccumulate>(GetCurrentMethodName(), expression);
    }

    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IItemSet<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(func);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Aggregate, source, seed, func, selector);
        var args = new[] { source.Expression, Expression.Constant(seed), Expression.Quote(func), Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Strategy.ExecuteFunction<TResult>(GetCurrentMethodName(), expression);
    }
    // ReSharper enable UnusedMember.Global

    private static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
        => source is IItemSet<TSource> storage
        ? storage.Expression
        : Expression.Constant(source, typeof(IEnumerable<TSource>));

    #region Helper methods to obtain MethodInfo in a safe way

    // ReSharper disable UnusedParameter.Local
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCurrentMethodName() {
        var st = new StackTrace(new StackFrame(1));
        return st.GetFrame(0)!.GetMethod()!.Name;
    }

    private static MethodInfo GetMethodInfo<T1, T2>(Func<T1, T2> f, T1 _)
        => f.Method;

    private static MethodInfo GetMethodInfo<T1, T2, T3>(Func<T1, T2, T3> f, T1 _, T2 __)
        => f.Method;

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Func<T1, T2, T3, T4> f, T1 _, T2 __, T3 ___)
        => f.Method;

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> f, T1 _, T2 __, T3 ___, T4 ____)
        => f.Method;

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6> f, T1 _, T2 __, T3 ___, T4 ____, T5 ______)
        => f.Method;

    private static MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> f, T1 _, T2 __, T3 ___, T4 ____, T5 ______, T6 _______)
        => f.Method;
    // ReSharper enable UnusedParameter.Local

    #endregion
}
