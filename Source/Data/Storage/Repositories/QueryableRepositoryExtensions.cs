namespace DotNetToolbox.Data.Repositories;

public static class RepositoryExtensions {
    // ReSharper disable UnusedMember.Global
    public static IStorage<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source) {
        // ReSharper disable once PossibleMultipleEnumeration
        IsNotNull(source);
        if (source is IStorage<TElement> elements)
            return elements;
        // ReSharper disable once PossibleMultipleEnumeration
        return new EnumerableQuery<TElement>(source);
    }

    public static IStorage AsQueryable(this IEnumerable source) {
        // ReSharper disable once PossibleMultipleEnumeration
        IsNotNull(source);
        if (source is IStorage queryable)
            return queryable;
        var enumType = FindGenericType(typeof(IEnumerable<>), source.GetType());
        // ReSharper disable once PossibleMultipleEnumeration
        return enumType is null
                   ? throw new ArgumentException("The type of the parameters is not a collection.", nameof(source))
                   : CreateEnumerableQuery(enumType.GetGenericArguments()[0], source);
    }

    public static IStorage<TResult> Where<TResult>(this IStorage<TResult> source, Expression<Func<TResult, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Where, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> Where<TResult>(this IStorage<TResult> source, Expression<Func<TResult, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Where, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> OfType<TResult>(this IStorage source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.OfType<TResult>, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> Cast<TResult>(this IStorage source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Cast<TResult>, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> Select<TSource, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Select, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> Select<TSource, TResult>(this IStorage<TSource> source, Expression<Func<TSource, int, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Select, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> SelectMany<TSource, TResult>(this IStorage<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.SelectMany, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> SelectMany<TSource, TResult>(this IStorage<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.SelectMany, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> SelectMany<TSource, TCollection, TResult>(this IStorage<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(collectionSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.SelectMany, source, collectionSelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> SelectMany<TSource, TCollection, TResult>(this IStorage<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(collectionSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.SelectMany, source, collectionSelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> Join<TOuter, TInner, TKey, TResult>(this IStorage<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector) {
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
        return outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> Join<TOuter, TInner, TKey, TResult>(this IStorage<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
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
        return outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IStorage<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector) {
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
        return outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IStorage<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
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
        return outer.Provider.CreateQuery<TResult>(expression);
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderBy, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderBy, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderByDescending, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.OrderByDescending, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenBy, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenBy, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenByDescending, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.ThenByDescending, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Take<TSource>(this IStorage<TSource> source, int count) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Take, source, count);
        var args = new[] { source.Expression, Expression.Constant(count) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> TakeWhile<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.TakeWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> TakeWhile<TSource>(this IStorage<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.TakeWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Skip<TSource>(this IStorage<TSource> source, int count) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Skip, source, count);
        var args = new[] { source.Expression, Expression.Constant(count) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> SkipWhile<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.SkipWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> SkipWhile<TSource>(this IStorage<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.SkipWhile, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<IGrouping<TKey, TSource>>(expression);
    }

    public static IStorage<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, elementSelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<IGrouping<TKey, TElement>>(expression);
    }

    public static IStorage<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, comparer);
        var args = new[] {
            source.Expression,
            Expression.Quote(keySelector),
            Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)),
        };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<IGrouping<TKey, TSource>>(expression);
    }

    public static IStorage<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer) {
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
        return source.Provider.CreateQuery<IGrouping<TKey, TElement>>(expression);
    }

    public static IStorage<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, elementSelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> GroupBy<TSource, TKey, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.GroupBy, source, keySelector, resultSelector);
        var args = new[] { source.Expression, Expression.Quote(keySelector), Expression.Quote(resultSelector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> GroupBy<TSource, TKey, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
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
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) {
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
        return source.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TSource> Distinct<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Distinct, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Distinct<TSource>(this IStorage<TSource> source, IEqualityComparer<TSource> comparer) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Distinct, source, comparer);
        var args = new[] {
            source.Expression,
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Concat<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Concat, source1, source2);
        var args = new[] { source1.Expression, GetSourceExpression(source2) };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TResult> Zip<TFirst, TSecond, TResult>(this IStorage<TFirst> source1, IEnumerable<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(resultSelector);
        var method = GetMethodInfo(Queryable.Zip, source1, source2, resultSelector);
        var args = new[] { source1.Expression, GetSourceExpression(source2) };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TResult>(expression);
    }

    public static IStorage<TSource> Union<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Union, source1, source2);
        var args = new[] { source1.Expression, GetSourceExpression(source2) };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Union<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Union, source1, source2, comparer);
        var args = new[] {
            source1.Expression,
            GetSourceExpression(source2),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Intersect<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Intersect, source1, source2);
        var args = new[] { source1.Expression, GetSourceExpression(source2) };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Intersect<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Union, source1, source2, comparer);
        var args = new[] {
            source1.Expression,
            GetSourceExpression(source2),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Except<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Except, source1, source2);
        var args = new[] { source1.Expression, GetSourceExpression(source2) };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> Except<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.Except, source1, source2, comparer);
        var args = new[] {
            source1.Expression,
            GetSourceExpression(source2),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.CreateQuery<TSource>(expression);
    }

    public static TSource First<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.First, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource First<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.First, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource FirstOrDefault<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.FirstOrDefault, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource FirstOrDefault<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.FirstOrDefault, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource Last<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Last, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource Last<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Last, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource LastOrDefault<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.LastOrDefault, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource LastOrDefault<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.LastOrDefault, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource Single<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Single, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource Single<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Single, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource SingleOrDefault<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.SingleOrDefault, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource SingleOrDefault<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.SingleOrDefault, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource ElementAt<TSource>(this IStorage<TSource> source, int index) {
        IsNotNull(source);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        var method = GetMethodInfo(Queryable.ElementAt, source, index);
        var args = new[] { source.Expression, Expression.Constant(index) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TSource ElementAtOrDefault<TSource>(this IStorage<TSource> source, int index) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.ElementAtOrDefault, source, index);
        var args = new[] { source.Expression, Expression.Constant(index) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static IStorage<TSource> DefaultIfEmpty<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.DefaultIfEmpty, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static IStorage<TSource> DefaultIfEmpty<TSource>(this IStorage<TSource> source, TSource defaultValue) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.DefaultIfEmpty, source, defaultValue);
        var args = new[] { source.Expression, Expression.Constant(defaultValue, typeof(TSource)) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static bool Contains<TSource>(this IStorage<TSource> source, TSource item) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Contains, source, item);
        var args = new[] { source.Expression, Expression.Constant(item, typeof(TSource)) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<bool>(expression);
    }

    public static bool Contains<TSource>(this IStorage<TSource> source, TSource item, IEqualityComparer<TSource> comparer) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Contains, source, item, comparer);
        var args = new[] {
            source.Expression,
            Expression.Constant(item, typeof(TSource)),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<bool>(expression);
    }

    public static IStorage<TSource> Reverse<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Reverse, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateQuery<TSource>(expression);
    }

    public static bool SequenceEqual<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.SequenceEqual, source1, source2);
        var args = new[] { source1.Expression, GetSourceExpression(source2) };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.Execute<bool>(expression);
    }

    public static bool SequenceEqual<TSource>(this IStorage<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        var method = GetMethodInfo(Queryable.SequenceEqual, source1, source2, comparer);
        var args = new[] {
            source1.Expression,
            GetSourceExpression(source2),
            Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)),
        };
        var expression = Expression.Call(null, method, args);
        return source1.Provider.Execute<bool>(expression);
    }

    public static bool Any<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Any, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<bool>(expression);
    }

    public static bool Any<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Any, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<bool>(expression);
    }

    public static bool All<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.All, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<bool>(expression);
    }

    public static int Count<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Count, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<int>(expression);
    }

    public static int Count<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.Any, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<int>(expression);
    }

    public static long LongCount<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.LongCount, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<long>(expression);
    }

    public static long LongCount<TSource>(this IStorage<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);
        var method = GetMethodInfo(Queryable.LongCount, source, predicate);
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<long>(expression);
    }

    public static TSource Min<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Min, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TResult Min<TSource, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Min, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TResult>(expression);
    }

    public static TSource Max<TSource>(this IStorage<TSource> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Max, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TResult Max<TSource, TResult>(this IStorage<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Min, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TResult>(expression);
    }

    public static int Sum(this IStorage<int> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<int>(expression);
    }

    public static int? Sum(this IStorage<int?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<int?>(expression);
    }

    public static long Sum(this IStorage<long> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<long>(expression);
    }

    public static long? Sum(this IStorage<long?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<long?>(expression);
    }

    public static float Sum(this IStorage<float> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float>(expression);
    }

    public static float? Sum(this IStorage<float?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float?>(expression);
    }

    public static double Sum(this IStorage<double> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Sum(this IStorage<double?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static decimal Sum(this IStorage<decimal> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal>(expression);
    }

    public static decimal? Sum(this IStorage<decimal?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Sum, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal?>(expression);
    }

    public static int Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, int>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<int>(expression);
    }

    public static int? Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, int?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<int?>(expression);
    }

    public static long Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, long>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<long>(expression);
    }

    public static long? Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, long?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<long?>(expression);
    }

    public static float Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, float>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float>(expression);
    }

    public static float? Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, float?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float?>(expression);
    }

    public static double Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, double>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, double?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static decimal Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, decimal>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal>(expression);
    }

    public static decimal? Sum<TSource>(this IStorage<TSource> source, Expression<Func<TSource, decimal?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Sum, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal?>(expression);
    }

    public static double Average(this IStorage<int> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Average(this IStorage<int?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static double Average(this IStorage<long> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Average(this IStorage<long?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static float Average(this IStorage<float> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float>(expression);
    }

    public static float? Average(this IStorage<float?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float?>(expression);
    }

    public static double Average(this IStorage<double> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Average(this IStorage<double?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static decimal Average(this IStorage<decimal> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal>(expression);
    }

    public static decimal? Average(this IStorage<decimal?> source) {
        IsNotNull(source);
        var method = GetMethodInfo(Queryable.Average, source);
        var args = new[] { source.Expression };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal?>(expression);
    }

    public static double Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, int>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, int?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static float Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, float>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float>(expression);
    }

    public static float? Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, float?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<float?>(expression);
    }

    public static double Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, long>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, long?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static double Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, double>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double>(expression);
    }

    public static double? Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, double?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<double?>(expression);
    }

    public static decimal Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, decimal>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal>(expression);
    }

    public static decimal? Average<TSource>(this IStorage<TSource> source, Expression<Func<TSource, decimal?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Average, source, selector);
        var args = new[] { source.Expression, Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<decimal?>(expression);
    }

    public static TSource Aggregate<TSource>(this IStorage<TSource> source, Expression<Func<TSource, TSource, TSource>> func) {
        IsNotNull(source);
        IsNotNull(func);
        var method = GetMethodInfo(Queryable.Aggregate, source, func);
        var args = new[] { source.Expression, Expression.Quote(func) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TSource>(expression);
    }

    public static TAccumulate Aggregate<TSource, TAccumulate>(this IStorage<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func) {
        IsNotNull(source);
        IsNotNull(func);
        var method = GetMethodInfo(Queryable.Aggregate, source, seed, func);
        var args = new[] { source.Expression, Expression.Constant(seed), Expression.Quote(func) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TAccumulate>(expression);
    }

    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IStorage<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(func);
        IsNotNull(selector);
        var method = GetMethodInfo(Queryable.Aggregate, source, seed, func, selector);
        var args = new[] { source.Expression, Expression.Constant(seed), Expression.Quote(func), Expression.Quote(selector) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.Execute<TResult>(expression);
    }
    // ReSharper enable UnusedMember.Global

    private static IStorage CreateEnumerableQuery(Type elementType, IEnumerable sequence) {
        var seqType = typeof(EnumerableQuery<>).MakeGenericType(elementType);
        return (IStorage)Activator.CreateInstance(seqType, sequence)!;
    }

    private static Type? FindGenericType(Type definition, Type? type) {
        while (true) {
            if (type == null || type == typeof(object))
                return null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
                return type;
            if (definition.IsInterface) {
                foreach (var iType in type.GetInterfaces()) {
                    var found = FindGenericType(definition, iType);
                    if (found != null)
                        return found;
                }
            }
            type = type.BaseType;
        }
    }

    private static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
        => source is IStorage<TSource> q
        ? q.Expression
        : Expression.Constant(source, typeof(IEnumerable<TSource>));

    #region Helper methods to obtain MethodInfo in a safe way

    // ReSharper disable UnusedParameter.Local
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
