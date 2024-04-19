//// ReSharper disable UnusedMember.Global
//using System.Diagnostics.CodeAnalysis;

//namespace DotNetToolbox.Data.Repositories;

//public static class RepositoryImplementation {
//    public static IQueryableRepository<TItem> AsRepository<TItem>(this IEnumerable<TItem> source)
//        where TItem : class {
//        IsNotNull(source);
//        return source as IQueryableRepository<TItem>
//            ?? throw new NotSupportedException("This collection does not support the repository implementation.");
//    }

//    public static IQueryableRepository<TItem> ToRepository<TItem>(this IEnumerable<TItem> source, IStrategyFactory? provider = null)
//        where TItem : class
//        => new Repository<TItem>(source, provider);

//    public static IQueryableRepository<TSource> Where<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);
//        var method = new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(Queryable.Where).Method;
//        var args = new[] { source.Expression, Expression.Quote(predicate) };
//        var expression = Expression.Call(null, method, args);
//        return source.Provider.CreateRepository<TSource>(expression);
//    }

//    public static IQueryableRepository<TSource> Where<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(Queryable.Where).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static IQueryableRepository<TResult> OfType<TResult>(this IQueryableRepository source)
//        where TResult : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable, IQueryable<TResult>>(Queryable.OfType<TResult>).Method,
//                source.Expression));
//    }

//    public static IQueryableRepository<TResult> Cast<TResult>(this IQueryableRepository source)
//        where TResult : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable, IQueryable<TResult>>(Queryable.Cast<TResult>).Method,
//                source.Expression));
//    }

//    public static IQueryableRepository<TResult> Select<TSource, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TResult>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TResult>>, IQueryable<TResult>>(Queryable.Select).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static IQueryableRepository<TResult> Select<TSource, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int, TResult>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int, TResult>>, IQueryable<TResult>>(Queryable.Select).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static IQueryableRepository<TResult> SelectMany<TSource, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<TResult>>>, IQueryable<TResult>>(Queryable.SelectMany).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static IQueryableRepository<TResult> SelectMany<TSource, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int, IEnumerable<TResult>>>, IQueryable<TResult>>(Queryable.SelectMany).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static IQueryableRepository<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(collectionSelector);
//        IsNotNull(resultSelector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IQueryable<TResult>>(Queryable.SelectMany).Method,
//                source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector)));
//    }

//    public static IQueryableRepository<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(collectionSelector);
//        IsNotNull(resultSelector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IQueryable<TResult>>(Queryable.SelectMany).Method,
//                source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryableRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
//        where TOuter : class
//        where TInner : class
//        where TResult : class {
//        IsNotNull(outer);
//        IsNotNull(inner);
//        IsNotNull(outerKeySelector);
//        IsNotNull(innerKeySelector);
//        IsNotNull(resultSelector);

//        return outer.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IQueryable<TResult>>(Queryable.Join).Method,
//                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryableRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
//        where TOuter : class
//        where TInner : class
//        where TResult : class {
//        IsNotNull(outer);
//        IsNotNull(inner);
//        IsNotNull(outerKeySelector);
//        IsNotNull(innerKeySelector);
//        IsNotNull(resultSelector);

//        return outer.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.Join).Method,
//                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryableRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
//        where TOuter : class
//        where TInner : class
//        where TResult : class {
//        IsNotNull(outer);
//        IsNotNull(inner);
//        IsNotNull(outerKeySelector);
//        IsNotNull(innerKeySelector);
//        IsNotNull(resultSelector);

//        return outer.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IQueryable<TResult>>(Queryable.GroupJoin).Method,
//                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryableRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
//        where TOuter : class
//        where TInner : class
//        where TResult : class {
//        IsNotNull(outer);
//        IsNotNull(inner);
//        IsNotNull(outerKeySelector);
//        IsNotNull(innerKeySelector);
//        IsNotNull(resultSelector);

//        return outer.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.GroupJoin).Method,
//                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IOrderedRepository<T> Order<T>(this IQueryableRepository<T> source)
//        where T : class {
//        IsNotNull(source);

//        return (IOrderedRepository<T>)source.Provider.CreateRepository<T>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<T>, IOrderedQueryable<T>>(Queryable.Order).Method,
//                source.Expression));
//    }

//    public static IOrderedRepository<T> Order<T>(this IQueryableRepository<T> source, IComparer<T> comparer)
//        where T : class {
//        IsNotNull(source);

//        return (IOrderedRepository<T>)source.Provider.CreateRepository<T>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<T>, IComparer<T>, IOrderedQueryable<T>>(Queryable.Order).Method,
//                source.Expression, Expression.Constant(comparer, typeof(IComparer<T>))));
//    }

//    public static IOrderedRepository<TSource> OrderBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.OrderBy).Method,
//                source.Expression, Expression.Quote(keySelector)));
//    }

//    public static IOrderedRepository<TSource> OrderBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.OrderBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
//    }

//    public static IOrderedRepository<T> OrderDescending<T>(this IQueryableRepository<T> source)
//        where T : class {
//        IsNotNull(source);

//        return (IOrderedRepository<T>)source.Provider.CreateRepository<T>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<T>, IOrderedQueryable<T>>(Queryable.OrderDescending).Method,
//                source.Expression));
//    }

//    public static IOrderedRepository<T> OrderDescending<T>(this IQueryableRepository<T> source, IComparer<T> comparer)
//        where T : class {
//        IsNotNull(source);

//        return (IOrderedRepository<T>)source.Provider.CreateRepository<T>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<T>, IComparer<T>, IOrderedQueryable<T>>(Queryable.OrderDescending).Method,
//                source.Expression, Expression.Constant(comparer, typeof(IComparer<T>))));
//    }

//    public static IOrderedRepository<TSource> OrderByDescending<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.OrderByDescending).Method,
//                source.Expression, Expression.Quote(keySelector)));
//    }

//    public static IOrderedRepository<TSource> OrderByDescending<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.OrderByDescending).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
//    }

//    public static IOrderedRepository<TSource> ThenBy<TSource, TKey>(this IOrderedRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IOrderedRepository<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.ThenBy).Method,
//                source.Expression, Expression.Quote(keySelector)));
//    }

//    public static IOrderedRepository<TSource> ThenBy<TSource, TKey>(this IOrderedRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IOrderedRepository<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.ThenBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
//    }

//    public static IOrderedRepository<TSource> ThenByDescending<TSource, TKey>(this IOrderedRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IOrderedRepository<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(Queryable.ThenByDescending).Method,
//                source.Expression, Expression.Quote(keySelector)));
//    }

//    public static IOrderedRepository<TSource> ThenByDescending<TSource, TKey>(this IOrderedRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return (IOrderedRepository<TSource>)source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IOrderedRepository<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(Queryable.ThenByDescending).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
//    }

//    public static IQueryableRepository<TSource> Take<TSource>(this IQueryableRepository<TSource> source, int count)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, IQueryable<TSource>>(Queryable.Take).Method,
//                source.Expression, Expression.Constant(count)));
//    }

//    public static IQueryableRepository<TSource> Take<TSource>(this IQueryableRepository<TSource> source, Range range)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Range, IQueryable<TSource>>(Queryable.Take).Method,
//                source.Expression, Expression.Constant(range)));
//    }

//    public static IQueryableRepository<TSource> TakeWhile<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(Queryable.TakeWhile).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static IQueryableRepository<TSource> TakeWhile<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(Queryable.TakeWhile).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static IQueryableRepository<TSource> TakeLast<TSource>(this IQueryableRepository<TSource> source, int count)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, IQueryable<TSource>>(Queryable.TakeLast).Method,
//                source.Expression, Expression.Constant(count)));
//    }

//    public static IQueryableRepository<TSource> Skip<TSource>(this IQueryableRepository<TSource> source, int count)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, IQueryable<TSource>>(Queryable.Skip).Method,
//                source.Expression, Expression.Constant(count)));
//    }

//    public static IQueryableRepository<TSource> SkipWhile<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, IQueryable<TSource>>(Queryable.SkipWhile).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static IQueryableRepository<TSource> SkipWhile<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int, bool>>, IQueryable<TSource>>(Queryable.SkipWhile).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static IQueryableRepository<TSource> SkipLast<TSource>(this IQueryableRepository<TSource> source, int count)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, IQueryable<TSource>>(Queryable.SkipLast).Method,
//                source.Expression, Expression.Constant(count)
//                ));
//    }

//    public static IQueryableRepository<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.CreateRepository<IGrouping<TKey, TSource>>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IQueryable<IGrouping<TKey, TSource>>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector)));
//    }

//    public static IQueryableRepository<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);
//        IsNotNull(elementSelector);

//        return source.Provider.CreateRepository<IGrouping<TKey, TElement>>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IQueryable<IGrouping<TKey, TElement>>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector)));
//    }

//    public static IQueryableRepository<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.CreateRepository<IGrouping<TKey, TSource>>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<IGrouping<TKey, TSource>>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IQueryableRepository<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);
//        IsNotNull(elementSelector);

//        return source.Provider.CreateRepository<IGrouping<TKey, TElement>>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IEqualityComparer<TKey>, IQueryable<IGrouping<TKey, TElement>>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IQueryableRepository<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);
//        IsNotNull(elementSelector);
//        IsNotNull(resultSelector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IQueryable<TResult>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector)));
//    }

//    public static IQueryableRepository<TResult> GroupBy<TSource, TKey, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);
//        IsNotNull(resultSelector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IQueryable<TResult>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Quote(resultSelector)));
//    }

//    public static IQueryableRepository<TResult> GroupBy<TSource, TKey, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);
//        IsNotNull(resultSelector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IQueryableRepository<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);
//        IsNotNull(elementSelector);
//        IsNotNull(resultSelector);

//        return source.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IEqualityComparer<TKey>, IQueryable<TResult>>(Queryable.GroupBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IQueryableRepository<TSource> Distinct<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IQueryable<TSource>>(Queryable.Distinct).Method,
//                source.Expression));
//    }

//    public static IQueryableRepository<TSource> Distinct<TSource>(this IQueryableRepository<TSource> source, IEqualityComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Distinct).Method,
//                source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
//    }

//    public static IQueryableRepository<TSource> DistinctBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IQueryable<TSource>>(Queryable.DistinctBy).Method,
//                source.Expression, Expression.Quote(keySelector)));
//    }

//    public static IQueryableRepository<TSource> DistinctBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<TSource>>(Queryable.DistinctBy).Method,
//                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IQueryableRepository<TSource[]> Chunk<TSource>(this IQueryableRepository<TSource> source, int size)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource[]>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, IQueryable<TSource[]>>(Queryable.Chunk).Method,
//                source.Expression, Expression.Constant(size)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Concat<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Concat).Method,
//                source1.Expression, GetSourceExpression(source2)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<IPack<TFirst, TSecond>> Zip<TFirst, TSecond>(this IQueryableRepository<TFirst> source1, IEnumerable<TSecond> source2)
//        where TFirst : class
//        where TSecond : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<IPack<TFirst, TSecond>>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TFirst>, IEnumerable<TSecond>, IQueryable<(TFirst First, TSecond Second)>>(Queryable.Zip).Method,
//                source1.Expression, GetSourceExpression(source2)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TResult> Zip<TFirst, TSecond, TResult>(this IQueryableRepository<TFirst> source1, IEnumerable<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector)
//        where TFirst : class
//        where TSecond : class
//        where TResult : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(resultSelector);

//        return source1.Provider.CreateRepository<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TFirst>, IEnumerable<TSecond>, Expression<Func<TFirst, TSecond, TResult>>, IQueryable<TResult>>(Queryable.Zip).Method,
//                source1.Expression, GetSourceExpression(source2), Expression.Quote(resultSelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<IPack<TFirst, TSecond, TThird>> Zip<TFirst, TSecond, TThird>(this IQueryableRepository<TFirst> source1, IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
//        where TFirst : class
//        where TSecond : class
//        where TThird : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(source3);

//        return source1.Provider.CreateRepository<IPack<TFirst, TSecond, TThird>>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IQueryable<(TFirst First, TSecond Second, TThird Third)>>(Queryable.Zip).Method,
//                source1.Expression, GetSourceExpression(source2), GetSourceExpression(source3)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Union<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Union).Method,
//                source1.Expression, GetSourceExpression(source2)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Union<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Union).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> UnionBy<TSource, TKey>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(keySelector);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, Expression<Func<TSource, TKey>>, IQueryable<TSource>>(Queryable.UnionBy).Method,
//                source1.Expression, GetSourceExpression(source2), Expression.Quote(keySelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> UnionBy<TSource, TKey>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(keySelector);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<TSource>>(Queryable.UnionBy).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Quote(keySelector),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Intersect<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Intersect).Method,
//                source1.Expression, GetSourceExpression(source2)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Intersect<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Intersect).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> IntersectBy<TSource, TKey>(this IQueryableRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(keySelector);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IQueryable<TSource>>(Queryable.IntersectBy).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Quote(keySelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> IntersectBy<TSource, TKey>(this IQueryableRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(keySelector);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<TSource>>(Queryable.IntersectBy).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Quote(keySelector),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Except<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IQueryable<TSource>>(Queryable.Except).Method,
//                source1.Expression, GetSourceExpression(source2)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> Except<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IQueryable<TSource>>(Queryable.Except).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> ExceptBy<TSource, TKey>(this IQueryableRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(keySelector);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IQueryable<TSource>>(Queryable.ExceptBy).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Quote(keySelector)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static IQueryableRepository<TSource> ExceptBy<TSource, TKey>(this IQueryableRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);
//        IsNotNull(keySelector);

//        return source1.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IQueryable<TSource>>(Queryable.ExceptBy).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Quote(keySelector),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
//    }

//    public static IQueryableRepository<TSource> DefaultIfEmpty<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IQueryable<TSource?>>(Queryable.DefaultIfEmpty).Method,
//                source.Expression));
//    }

//    public static IQueryableRepository<TSource> DefaultIfEmpty<TSource>(this IQueryableRepository<TSource> source, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, IQueryable<TSource>>(Queryable.DefaultIfEmpty).Method,
//                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
//    }

//    public static IQueryableRepository<TSource> Reverse<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IQueryable<TSource>>(Queryable.Reverse).Method,
//                source.Expression));
//    }
//    public static IQueryableRepository<TSource> Append<TSource>(this IQueryableRepository<TSource> source, TSource element)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, IQueryable<TSource>>(Queryable.Append).Method,
//                source.Expression, Expression.Constant(element)));
//    }

//    public static IQueryableRepository<TSource> Prepend<TSource>(this IQueryableRepository<TSource> source, TSource element)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.CreateRepository<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, IQueryable<TSource>>(Queryable.Prepend).Method,
//                source.Expression, Expression.Constant(element)));
//    }

//    public static TSource First<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource>(Queryable.First).Method,
//                source.Expression));
//    }

//    public static TSource First<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.First).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource FirstOrDefault<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource?>(Queryable.FirstOrDefault).Method,
//                source.Expression));
//    }

//    public static TSource FirstOrDefault<TSource>(this IQueryableRepository<TSource> source, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, TSource>(Queryable.FirstOrDefault).Method,
//                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
//    }

//    public static TSource FirstOrDefault<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource?>(Queryable.FirstOrDefault).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource FirstOrDefault<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource, TSource>(Queryable.FirstOrDefault).Method,
//                source.Expression, Expression.Quote(predicate), Expression.Constant(defaultValue, typeof(TSource))));
//    }

//    public static TSource Last<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource>(Queryable.Last).Method,
//                source.Expression));
//    }

//    public static TSource Last<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.Last).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource LastOrDefault<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource?>(Queryable.LastOrDefault).Method,
//                source.Expression));
//    }

//    public static TSource LastOrDefault<TSource>(this IQueryableRepository<TSource> source, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, TSource>(Queryable.LastOrDefault).Method,
//                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
//    }

//    public static TSource LastOrDefault<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource?>(Queryable.LastOrDefault).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource LastOrDefault<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource, TSource>(Queryable.LastOrDefault).Method,
//                source.Expression, Expression.Quote(predicate), Expression.Constant(defaultValue, typeof(TSource))
//            ));
//    }

//    public static TSource Single<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource>(Queryable.Single).Method,
//                source.Expression));
//    }

//    public static TSource Single<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource>(Queryable.Single).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource SingleOrDefault<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource?>(Queryable.SingleOrDefault).Method,
//                source.Expression));
//    }

//    public static TSource SingleOrDefault<TSource>(this IQueryableRepository<TSource> source, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, TSource>(Queryable.SingleOrDefault).Method,
//                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
//    }

//    public static TSource SingleOrDefault<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource?>(Queryable.SingleOrDefault).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource SingleOrDefault<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, TSource, TSource>(Queryable.SingleOrDefault).Method,
//                source.Expression, Expression.Quote(predicate), Expression.Constant(defaultValue, typeof(TSource))));
//    }

//    public static TSource ElementAt<TSource>(this IQueryableRepository<TSource> source, int index)
//        where TSource : class {
//        IsNotNull(source);

//        return index < 0
//            ? throw new ArgumentOutOfRangeException(nameof(index))
//            : source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, TSource>(Queryable.ElementAt).Method,
//                source.Expression, Expression.Constant(index)));
//    }

//    public static TSource ElementAt<TSource>(this IQueryableRepository<TSource> source, Index index)
//        where TSource : class {
//        IsNotNull(source);

//        return index is { IsFromEnd: true, Value: 0 }
//            ? throw new ArgumentOutOfRangeException(nameof(index))
//            : source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Index, TSource>(Queryable.ElementAt).Method,
//                source.Expression, Expression.Constant(index)));
//    }

//    public static TSource ElementAtOrDefault<TSource>(this IQueryableRepository<TSource> source, int index)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int, TSource?>(Queryable.ElementAtOrDefault).Method,
//                source.Expression, Expression.Constant(index)));
//    }

//    public static TSource ElementAtOrDefault<TSource>(this IQueryableRepository<TSource> source, Index index)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Index, TSource?>(Queryable.ElementAtOrDefault).Method,
//                source.Expression, Expression.Constant(index)));
//    }
//    public static bool Contains<TSource>(this IQueryableRepository<TSource> source, TSource item)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, bool>(Queryable.Contains).Method,
//                source.Expression, Expression.Constant(item, typeof(TSource))));
//    }

//    public static bool Contains<TSource>(this IQueryableRepository<TSource> source, TSource item, IEqualityComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource, IEqualityComparer<TSource>, bool>(Queryable.Contains).Method,
//                source.Expression, Expression.Constant(item, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static bool SequenceEqual<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, bool>(Queryable.SequenceEqual).Method,
//                source1.Expression, GetSourceExpression(source2)));
//    }

//    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//    public static bool SequenceEqual<TSource>(this IQueryableRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source1);
//        IsNotNull(source2);

//        return source1.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, bool>(Queryable.SequenceEqual).Method,
//                source1.Expression,
//                GetSourceExpression(source2),
//                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
//    }

//    public static bool Any<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, bool>(Queryable.Any).Method,
//                source.Expression));
//    }

//    public static bool Any<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, bool>(Queryable.Any).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static bool All<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<bool>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, bool>(Queryable.All).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static int Count<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<int>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, int>(Queryable.Count).Method,
//                source.Expression));
//    }

//    public static int Count<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<int>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, int>(Queryable.Count).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static long LongCount<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<long>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, long>(Queryable.LongCount).Method,
//                source.Expression));
//    }

//    public static long LongCount<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(predicate);

//        return source.Provider.Execute<long>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, bool>>, long>(Queryable.LongCount).Method,
//                source.Expression, Expression.Quote(predicate)));
//    }

//    public static TSource Min<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource?>(Queryable.Min).Method,
//                source.Expression));
//    }

//    public static TResult Min<TSource, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TResult>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TResult>>, TResult?>(Queryable.Min).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static TSource MinBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, TSource?>(Queryable.MinBy).Method,
//                source.Expression,
//                Expression.Quote(keySelector)));
//    }

//    public static TSource MinBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TSource>, TSource?>(Queryable.MinBy).Method,
//                source.Expression,
//                Expression.Quote(keySelector),
//                Expression.Constant(comparer, typeof(IComparer<TSource>))));
//    }

//    public static TSource Max<TSource>(this IQueryableRepository<TSource> source)
//        where TSource : class {
//        IsNotNull(source);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TSource?>(Queryable.Max).Method,
//                source.Expression));
//    }

//    public static TResult Max<TSource, TResult>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TResult>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TResult>>, TResult?>(Queryable.Max).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static TSource MaxBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, TSource?>(Queryable.MaxBy).Method,
//                source.Expression,
//                Expression.Quote(keySelector)));
//    }

//    public static TSource MaxBy<TSource, TKey>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TSource>? comparer)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(keySelector);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TSource>, TSource?>(Queryable.MaxBy).Method,
//                source.Expression,
//                Expression.Quote(keySelector),
//                Expression.Constant(comparer, typeof(IComparer<TSource>))));
//    }

//    public static int Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<int>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int>>, int>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static int? Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<int?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int?>>, int?>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static long Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, long>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<long>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, long>>, long>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static long? Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, long?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<long?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, long?>>, long?>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static float Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, float>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<float>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, float>>, float>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static float? Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, float?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<float?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, float?>>, float?>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, double>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, double>>, double>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double? Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, double?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, double?>>, double?>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static decimal Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, decimal>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<decimal>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, decimal>>, decimal>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static decimal? Sum<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, decimal?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<decimal?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, decimal?>>, decimal?>(Queryable.Sum).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int>>, double>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double? Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, int?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, int?>>, double?>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static float Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, float>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<float>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, float>>, float>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static float? Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, float?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<float?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, float?>>, float?>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, long>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, long>>, double>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double? Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, long?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, long?>>, double?>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, double>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, double>>, double>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static double? Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, double?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<double?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, double?>>, double?>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static decimal Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, decimal>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<decimal>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, decimal>>, decimal>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static decimal? Average<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, decimal?>> selector)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(selector);

//        return source.Provider.Execute<decimal?>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, decimal?>>, decimal?>(Queryable.Average).Method,
//                source.Expression, Expression.Quote(selector)));
//    }

//    public static TSource Aggregate<TSource>(this IQueryableRepository<TSource> source, Expression<Func<TSource, TSource, TSource>> func)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(func);

//        return source.Provider.Execute<TSource>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, Expression<Func<TSource, TSource, TSource>>, TSource>(Queryable.Aggregate).Method,
//                source.Expression, Expression.Quote(func)));
//    }

//    public static TAccumulate Aggregate<TSource, TAccumulate>(this IQueryableRepository<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func)
//        where TSource : class {
//        IsNotNull(source);
//        IsNotNull(func);

//        return source.Provider.Execute<TAccumulate>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, TAccumulate>(Queryable.Aggregate).Method,
//                source.Expression, Expression.Constant(seed), Expression.Quote(func)));
//    }

//    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IQueryableRepository<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
//        where TSource : class
//        where TResult : class {
//        IsNotNull(source);
//        IsNotNull(func);
//        IsNotNull(selector);

//        return source.Provider.Execute<TResult>(
//            Expression.Call(
//                null,
//                new Func<IQueryable<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, Expression<Func<TAccumulate, TResult>>, TResult>(Queryable.Aggregate).Method,
//                source.Expression, Expression.Constant(seed), Expression.Quote(func), Expression.Quote(selector)));
//    }

//    private static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
//        => source is IQueryable<TSource> query
//               ? query.Expression
//               : Expression.Constant(source, typeof(IEnumerable<TSource>));
//}
