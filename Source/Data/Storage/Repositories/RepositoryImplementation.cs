namespace DotNetToolbox.Data.Repositories;

public static class RepositoryImplementation {
    public static IRepository<TItem> AsRepository<TItem>(this IEnumerable<TItem> source)
        where TItem : class {
        IsNotNull(source);
        return source as IRepository<TItem>
            ?? throw new NotSupportedException("This collection does not support the repository implementation.");
    }
    public static IRepository<TItem> ToRepository<TItem>(this IEnumerable<TItem> source, IStrategyProvider? provider = null)
        where TItem : class
        => new Repository<TItem>(source, provider);

    public static IRepository<TSource> Where<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate)
        where TSource : class {
        IsNotNull(source);
        IsNotNull(predicate);
        //var method = GetMethodInfo(Queryable.Where, source, predicate);
        //var args = new[] { source.Expression, Expression.Quote(predicate) };
        //var expression = Expression.Call(null, method, args);
        //var result = source.Provider.CreateRepository(expression);

        var method = new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, IRepository<TSource>>(Where).Method;
        var args = new[] { source.Expression, Expression.Quote(predicate) };
        var expression = Expression.Call(null, method, args);
        return source.Provider.CreateRepository<TSource>(expression);
    }

    public static IRepository<TSource> Where<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int, bool>>, IRepository<TSource>>(Where).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static IRepository<TResult> OfType<TResult>(this IRepository source) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository, IRepository<TResult>>(OfType<TResult>).Method,
                source.Expression));
    }

    public static IRepository<TResult> Cast<TResult>(this IRepository source) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository, IRepository<TResult>>(Cast<TResult>).Method,
                source.Expression));
    }

    public static IRepository<TResult> Select<TSource, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TResult>>, IRepository<TResult>>(Select).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static IRepository<TResult> Select<TSource, TResult>(this IRepository<TSource> source, Expression<Func<TSource, int, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int, TResult>>, IRepository<TResult>>(Select).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static IRepository<TResult> SelectMany<TSource, TResult>(this IRepository<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, IEnumerable<TResult>>>, IRepository<TResult>>(SelectMany).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static IRepository<TResult> SelectMany<TSource, TResult>(this IRepository<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int, IEnumerable<TResult>>>, IRepository<TResult>>(SelectMany).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static IRepository<TResult> SelectMany<TSource, TCollection, TResult>(this IRepository<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(collectionSelector);
        IsNotNull(resultSelector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IRepository<TResult>>(SelectMany).Method,
                source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector)));
    }

    public static IRepository<TResult> SelectMany<TSource, TCollection, TResult>(this IRepository<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(collectionSelector);
        IsNotNull(resultSelector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, IEnumerable<TCollection>>>, Expression<Func<TSource, TCollection, TResult>>, IRepository<TResult>>(SelectMany).Method,
                source.Expression, Expression.Quote(collectionSelector), Expression.Quote(resultSelector)));
    }

    private static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source) {
        IRepository<TSource>? q = source as IRepository<TSource>;
        return q != null ? q.Expression : Expression.Constant(source, typeof(IEnumerable<TSource>));
    }

    public static IRepository<TResult> Join<TOuter, TInner, TKey, TResult>(this IRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector) {
        IsNotNull(outer);
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);

        return outer.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IRepository<TResult>>(Join).Method,
                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector)));
    }

    public static IRepository<TResult> Join<TOuter, TInner, TKey, TResult>(this IRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(outer);
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);

        return outer.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, TInner, TResult>>, IEqualityComparer<TKey>, IRepository<TResult>>(Join).Method,
                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector) {
        IsNotNull(outer);
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);

        return outer.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IRepository<TResult>>(GroupJoin).Method,
                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector)));
    }

    public static IRepository<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IRepository<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(outer);
        IsNotNull(inner);
        IsNotNull(outerKeySelector);
        IsNotNull(innerKeySelector);
        IsNotNull(resultSelector);

        return outer.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TOuter>, IEnumerable<TInner>, Expression<Func<TOuter, TKey>>, Expression<Func<TInner, TKey>>, Expression<Func<TOuter, IEnumerable<TInner>, TResult>>, IEqualityComparer<TKey>, IRepository<TResult>>(GroupJoin).Method,
                outer.Expression, GetSourceExpression(inner), Expression.Quote(outerKeySelector), Expression.Quote(innerKeySelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    /// <summary>
    /// Sorts the elements of a sequence in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method has at least one parameter of type <see cref="Expression{TDelegate}"/> whose type argument is one
    /// of the <see cref="Func{T,TResult}"/> types.
    /// For these parameters, you can pass in a lambda expression and it will be compiled to an <see cref="Expression{TDelegate}"/>.
    ///
    /// The <see cref="Order{T}(IRepository{T})"/> method generates a <see cref="MethodCallExpression"/> that represents
    /// calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/> itself as a constructed generic method.
    /// It then passes the <see cref="MethodCallExpression"/> to the <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> method
    /// of the <see cref="IQueryProvider"/> represented by the <see cref="IRepository.Provider"/> property of the <paramref name="source"/>
    /// parameter. The result of calling <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> is cast to
    /// type <see cref="IOrderedQueryable{T}"/> and returned.
    ///
    /// The query behavior that occurs as a result of executing an expression tree
    /// that represents calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/>
    /// depends on the implementation of the <paramref name="source"/> parameter.
    /// The expected behavior is that it sorts the elements of <paramref name="source"/> by itself.
    /// </remarks>
    public static IOrderedQueryable<T> Order<T>(this IRepository<T> source) {
        IsNotNull(source);

        return (IOrderedQueryable<T>)source.Provider.CreateRepository<T>(
            Expression.Call(
                null,
                new Func<IRepository<T>, IOrderedQueryable<T>>(Order).Method,
                source.Expression));
    }

    /// <summary>
    /// Sorts the elements of a sequence in ascending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method has at least one parameter of type <see cref="Expression{TDelegate}"/> whose type argument is one
    /// of the <see cref="Func{T,TResult}"/> types.
    /// For these parameters, you can pass in a lambda expression and it will be compiled to an <see cref="Expression{TDelegate}"/>.
    ///
    /// The <see cref="Order{T}(IRepository{T})"/> method generates a <see cref="MethodCallExpression"/> that represents
    /// calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/> itself as a constructed generic method.
    /// It then passes the <see cref="MethodCallExpression"/> to the <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> method
    /// of the <see cref="IQueryProvider"/> represented by the <see cref="IRepository.Provider"/> property of the <paramref name="source"/>
    /// parameter. The result of calling <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> is cast to
    /// type <see cref="IOrderedQueryable{T}"/> and returned.
    ///
    /// The query behavior that occurs as a result of executing an expression tree
    /// that represents calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/>
    /// depends on the implementation of the <paramref name="source"/> parameter.
    /// The expected behavior is that it sorts the elements of <paramref name="source"/> by itself.
    /// </remarks>
    public static IOrderedQueryable<T> Order<T>(this IRepository<T> source, IComparer<T> comparer) {
        IsNotNull(source);

        return (IOrderedQueryable<T>)source.Provider.CreateRepository<T>(
            Expression.Call(
                null,
                new Func<IRepository<T>, IComparer<T>, IOrderedQueryable<T>>(Order).Method,
                source.Expression, Expression.Constant(comparer, typeof(IComparer<T>))));
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(OrderBy).Method,
                source.Expression, Expression.Quote(keySelector)));
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(OrderBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
    }

    /// <summary>
    /// Sorts the elements of a sequence in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method has at least one parameter of type <see cref="Expression{TDelegate}"/> whose type argument is one
    /// of the <see cref="Func{T,TResult}"/> types.
    /// For these parameters, you can pass in a lambda expression and it will be compiled to an <see cref="Expression{TDelegate}"/>.
    ///
    /// The <see cref="Order{T}(IRepository{T})"/> method generates a <see cref="MethodCallExpression"/> that represents
    /// calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/> itself as a constructed generic method.
    /// It then passes the <see cref="MethodCallExpression"/> to the <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> method
    /// of the <see cref="IQueryProvider"/> represented by the <see cref="IRepository.Provider"/> property of the <paramref name="source"/>
    /// parameter. The result of calling <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> is cast to
    /// type <see cref="IOrderedQueryable{T}"/> and returned.
    ///
    /// The query behavior that occurs as a result of executing an expression tree
    /// that represents calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/>
    /// depends on the implementation of the <paramref name="source"/> parameter.
    /// The expected behavior is that it sorts the elements of <paramref name="source"/> by itself.
    /// </remarks>
    public static IOrderedQueryable<T> OrderDescending<T>(this IRepository<T> source) {
        IsNotNull(source);

        return (IOrderedQueryable<T>)source.Provider.CreateRepository<T>(
            Expression.Call(
                null,
                new Func<IRepository<T>, IOrderedQueryable<T>>(OrderDescending).Method,
                source.Expression));
    }

    /// <summary>
    /// Sorts the elements of a sequence in descending order.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
    /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// This method has at least one parameter of type <see cref="Expression{TDelegate}"/> whose type argument is one
    /// of the <see cref="Func{T,TResult}"/> types.
    /// For these parameters, you can pass in a lambda expression and it will be compiled to an <see cref="Expression{TDelegate}"/>.
    ///
    /// The <see cref="Order{T}(IRepository{T})"/> method generates a <see cref="MethodCallExpression"/> that represents
    /// calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/> itself as a constructed generic method.
    /// It then passes the <see cref="MethodCallExpression"/> to the <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> method
    /// of the <see cref="IQueryProvider"/> represented by the <see cref="IRepository.Provider"/> property of the <paramref name="source"/>
    /// parameter. The result of calling <see cref="IQueryProvider.CreateRepository{TElement}(Expression)"/> is cast to
    /// type <see cref="IOrderedQueryable{T}"/> and returned.
    ///
    /// The query behavior that occurs as a result of executing an expression tree
    /// that represents calling <see cref="Enumerable.Order{T}(IEnumerable{T})"/>
    /// depends on the implementation of the <paramref name="source"/> parameter.
    /// The expected behavior is that it sorts the elements of <paramref name="source"/> by itself.
    /// </remarks>
    public static IOrderedQueryable<T> OrderDescending<T>(this IRepository<T> source, IComparer<T> comparer) {
        IsNotNull(source);

        return (IOrderedQueryable<T>)source.Provider.CreateRepository<T>(
            Expression.Call(
                null,
                new Func<IRepository<T>, IComparer<T>, IOrderedQueryable<T>>(OrderDescending).Method,
                source.Expression, Expression.Constant(comparer, typeof(IComparer<T>))));
    }

    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(OrderByDescending).Method,
                source.Expression, Expression.Quote(keySelector)));
    }

    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(OrderByDescending).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
    }

    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(ThenBy).Method,
                source.Expression, Expression.Quote(keySelector)));
    }

    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(ThenBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
    }

    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IOrderedQueryable<TSource>>(ThenByDescending).Method,
                source.Expression, Expression.Quote(keySelector)));
    }

    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return (IOrderedQueryable<TSource>)source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IOrderedQueryable<TSource>, Expression<Func<TSource, TKey>>, IComparer<TKey>, IOrderedQueryable<TSource>>(ThenByDescending).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IComparer<TKey>))));
    }

    public static IRepository<TSource> Take<TSource>(this IRepository<TSource> source, int count) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, IRepository<TSource>>(Take).Method,
                source.Expression, Expression.Constant(count)));
    }

    /// <summary>Returns a specified range of contiguous elements from a sequence.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="range">The range of elements to return, which has start and end indexes either from the start or the end.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="IRepository{T}" /> that contains the specified <paramref name="range" /> of elements from the <paramref name="source" /> sequence.</returns>
    public static IRepository<TSource> Take<TSource>(this IRepository<TSource> source, Range range) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Range, IRepository<TSource>>(Take).Method,
                source.Expression, Expression.Constant(range)));
    }

    public static IRepository<TSource> TakeWhile<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, IRepository<TSource>>(TakeWhile).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static IRepository<TSource> TakeWhile<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int, bool>>, IRepository<TSource>>(TakeWhile).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static IRepository<TSource> Skip<TSource>(this IRepository<TSource> source, int count) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, IRepository<TSource>>(Skip).Method,
                source.Expression, Expression.Constant(count)));
    }

    public static IRepository<TSource> SkipWhile<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, IRepository<TSource>>(SkipWhile).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static IRepository<TSource> SkipWhile<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int, bool>>, IRepository<TSource>>(SkipWhile).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static IRepository<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.CreateRepository<IGrouping<TKey, TSource>>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IRepository<IGrouping<TKey, TSource>>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector)));
    }

    public static IRepository<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);

        return source.Provider.CreateRepository<IGrouping<TKey, TElement>>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IRepository<IGrouping<TKey, TElement>>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector)));
    }

    public static IRepository<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.CreateRepository<IGrouping<TKey, TSource>>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IRepository<IGrouping<TKey, TSource>>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);

        return source.Provider.CreateRepository<IGrouping<TKey, TElement>>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, IEqualityComparer<TKey>, IRepository<IGrouping<TKey, TElement>>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        IsNotNull(resultSelector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IRepository<TResult>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector)));
    }

    public static IRepository<TResult> GroupBy<TSource, TKey, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(resultSelector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IRepository<TResult>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Quote(resultSelector)));
    }

    public static IRepository<TResult> GroupBy<TSource, TKey, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(resultSelector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TKey, IEnumerable<TSource>, TResult>>, IEqualityComparer<TKey>, IRepository<TResult>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        IsNotNull(resultSelector);

        return source.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, Expression<Func<TSource, TElement>>, Expression<Func<TKey, IEnumerable<TElement>, TResult>>, IEqualityComparer<TKey>, IRepository<TResult>>(GroupBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Quote(elementSelector), Expression.Quote(resultSelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<TSource> Distinct<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IRepository<TSource>>(Distinct).Method,
                source.Expression));
    }

    public static IRepository<TSource> Distinct<TSource>(this IRepository<TSource> source, IEqualityComparer<TSource>? comparer) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEqualityComparer<TSource>, IRepository<TSource>>(Distinct).Method,
                source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
    }

    /// <summary>Returns distinct elements from a sequence according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>An <see cref="IRepository{T}" /> that contains distinct elements from the source sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    public static IRepository<TSource> DistinctBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IRepository<TSource>>(DistinctBy).Method,
                source.Expression, Expression.Quote(keySelector)));
    }

    /// <summary>Returns distinct elements from a sequence according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
    /// <returns>An <see cref="IRepository{T}" /> that contains distinct elements from the source sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    public static IRepository<TSource> DistinctBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IRepository<TSource>>(DistinctBy).Method,
                source.Expression, Expression.Quote(keySelector), Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    /// <summary>Split the elements of a sequence into chunks of size at most <paramref name="size"/>.</summary>
    /// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
    /// <param name="size">Maximum size of each chunk.</param>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <returns>An <see cref="IRepository{T}"/> that contains the elements the input sequence split into chunks of size <paramref name="size"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is below 1.</exception>
    /// <remarks>
    /// <para>Every chunk except the last will be of size <paramref name="size"/>.</para>
    /// <para>The last chunk will contain the remaining elements and may be of a smaller size.</para>
    /// </remarks>
    public static IRepository<TSource[]> Chunk<TSource>(this IRepository<TSource> source, int size) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource[]>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, IRepository<TSource[]>>(Chunk).Method,
                source.Expression, Expression.Constant(size)));
    }

    public static IRepository<TSource> Concat<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IRepository<TSource>>(Concat).Method,
                source1.Expression, GetSourceExpression(source2)));
    }

    public static IRepository<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IRepository<TFirst> source1, IEnumerable<TSecond> source2) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<(TFirst, TSecond)>(
            Expression.Call(
                null,
                new Func<IRepository<TFirst>, IEnumerable<TSecond>, IRepository<(TFirst, TSecond)>>(Zip).Method,
                source1.Expression, GetSourceExpression(source2)));
    }

    public static IRepository<TResult> Zip<TFirst, TSecond, TResult>(this IRepository<TFirst> source1, IEnumerable<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(resultSelector);

        return source1.Provider.CreateRepository<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TFirst>, IEnumerable<TSecond>, Expression<Func<TFirst, TSecond, TResult>>, IRepository<TResult>>(Zip).Method,
                source1.Expression, GetSourceExpression(source2), Expression.Quote(resultSelector)));
    }

    /// <summary>
    /// Produces a sequence of tuples with elements from the three specified sequences.
    /// </summary>
    /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
    /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
    /// <typeparam name="TThird">The type of the elements of the third input sequence.</typeparam>
    /// <param name="source1">The first sequence to merge.</param>
    /// <param name="source2">The second sequence to merge.</param>
    /// <param name="source3">The third sequence to merge.</param>
    /// <returns>A sequence of tuples with elements taken from the first, second and third sequences, in that order.</returns>
    public static IRepository<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IRepository<TFirst> source1, IEnumerable<TSecond> source2, IEnumerable<TThird> source3) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(source3);

        return source1.Provider.CreateRepository<(TFirst, TSecond, TThird)>(
            Expression.Call(
                null,
                new Func<IRepository<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IRepository<(TFirst, TSecond, TThird)>>(Zip).Method,
                source1.Expression, GetSourceExpression(source2), GetSourceExpression(source3)));
    }

    public static IRepository<TSource> Union<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IRepository<TSource>>(Union).Method,
                source1.Expression, GetSourceExpression(source2)));
    }

    public static IRepository<TSource> Union<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IRepository<TSource>>(Union).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
    }

    /// <summary>Produces the set union of two sequences according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source1">An <see cref="IRepository{T}" /> whose distinct elements form the first set for the union.</param>
    /// <param name="source2">An <see cref="IEnumerable{T}" /> whose distinct elements form the second set for the union.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>An <see cref="IRepository{T}" /> that contains the elements from both input sequences, excluding duplicates.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source1" /> or <paramref name="source2" /> is <see langword="null" />.</exception>
    public static IRepository<TSource> UnionBy<TSource, TKey>(this IRepository<TSource> source1, IEnumerable<TSource> source2, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(keySelector);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, Expression<Func<TSource, TKey>>, IRepository<TSource>>(UnionBy).Method,
                source1.Expression, GetSourceExpression(source2), Expression.Quote(keySelector)));
    }

    /// <summary>Produces the set union of two sequences according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source1">An <see cref="IRepository{T}" /> whose distinct elements form the first set for the union.</param>
    /// <param name="source2">An <see cref="IEnumerable{T}" /> whose distinct elements form the second set for the union.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> to compare values.</param>
    /// <returns>An <see cref="IRepository{T}" /> that contains the elements from both input sequences, excluding duplicates.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source1" /> or <paramref name="source2" /> is <see langword="null" />.</exception>
    public static IRepository<TSource> UnionBy<TSource, TKey>(this IRepository<TSource> source1, IEnumerable<TSource> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(keySelector);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IRepository<TSource>>(UnionBy).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Quote(keySelector),
                Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<TSource> Intersect<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IRepository<TSource>>(Intersect).Method,
                source1.Expression, GetSourceExpression(source2)));
    }

    public static IRepository<TSource> Intersect<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IRepository<TSource>>(Intersect).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
    }

    /// <summary>Produces the set intersection of two sequences according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source1">An <see cref="IRepository{T}" /> whose distinct elements that also appear in <paramref name="source2" /> will be returned.</param>
    /// <param name="source2">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence will be returned.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source1" /> or <paramref name="source2" /> is <see langword="null" />.</exception>
    public static IRepository<TSource> IntersectBy<TSource, TKey>(this IRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(keySelector);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IRepository<TSource>>(IntersectBy).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Quote(keySelector)));
    }

    /// <summary>Produces the set intersection of two sequences according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source1">An <see cref="IRepository{T}" /> whose distinct elements that also appear in <paramref name="source2" /> will be returned.</param>
    /// <param name="source2">An <see cref="IEnumerable{T}" /> whose distinct elements that also appear in the first sequence will be returned.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
    /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source1" /> or <paramref name="source2" /> is <see langword="null" />.</exception>
    public static IRepository<TSource> IntersectBy<TSource, TKey>(this IRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(keySelector);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IRepository<TSource>>(IntersectBy).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Quote(keySelector),
                Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static IRepository<TSource> Except<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IRepository<TSource>>(Except).Method,
                source1.Expression, GetSourceExpression(source2)));
    }

    public static IRepository<TSource> Except<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, IRepository<TSource>>(Except).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
    }

    /// <summary>
    /// Produces the set difference of two sequences according to a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source1">An <see cref="IRepository{TSource}" /> whose keys that are not also in <paramref name="source2"/> will be returned.</param>
    /// <param name="source2">An <see cref="IEnumerable{TKey}" /> whose keys that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>A <see cref="IRepository{TSource}" /> that contains the set difference of the elements of two sequences.</returns>
    public static IRepository<TSource> ExceptBy<TSource, TKey>(this IRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(keySelector);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IRepository<TSource>>(ExceptBy).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Quote(keySelector)));
    }

    /// <summary>
    /// Produces the set difference of two sequences according to a specified key selector function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the input sequence.</typeparam>
    /// <typeparam name="TKey">The type of key to identify elements by.</typeparam>
    /// <param name="source1">An <see cref="IRepository{TSource}" /> whose keys that are not also in <paramref name="source2"/> will be returned.</param>
    /// <param name="source2">An <see cref="IEnumerable{TKey}" /> whose keys that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}" /> to compare keys.</param>
    /// <returns>A <see cref="IRepository{TSource}" /> that contains the set difference of the elements of two sequences.</returns>
    public static IRepository<TSource> ExceptBy<TSource, TKey>(this IRepository<TSource> source1, IEnumerable<TKey> source2, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);
        IsNotNull(keySelector);

        return source1.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TKey>, Expression<Func<TSource, TKey>>, IEqualityComparer<TKey>, IRepository<TSource>>(ExceptBy).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Quote(keySelector),
                Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
    }

    public static TSource First<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource>(First).Method,
                source.Expression));
    }

    public static TSource First<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource>(First).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static TSource? FirstOrDefault<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource?>(FirstOrDefault).Method,
                source.Expression));
    }

    /// <summary>Returns the first element of a sequence, or a default value if the sequence contains no elements.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}" /> to return the first element of.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns><paramref name="defaultValue" /> if <paramref name="source" /> is empty; otherwise, the first element in <paramref name="source" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource FirstOrDefault<TSource>(this IRepository<TSource> source, TSource defaultValue) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, TSource>(FirstOrDefault).Method,
                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
    }

    public static TSource? FirstOrDefault<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource?>(FirstOrDefault).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    /// <summary>Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns><paramref name="defaultValue" /> if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static TSource FirstOrDefault<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource, TSource>(FirstOrDefault).Method,
                source.Expression, Expression.Quote(predicate), Expression.Constant(defaultValue, typeof(TSource))));
    }

    public static TSource Last<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource>(Last).Method,
                source.Expression));
    }

    public static TSource Last<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource>(Last).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static TSource? LastOrDefault<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource?>(LastOrDefault).Method,
                source.Expression));
    }

    /// <summary>Returns the last element of a sequence, or a default value if the sequence contains no elements.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return the last element of.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns><paramref name="defaultValue" /> if the source sequence is empty; otherwise, the last element in the <see cref="IEnumerable{T}" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource LastOrDefault<TSource>(this IRepository<TSource> source, TSource defaultValue) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, TSource>(LastOrDefault).Method,
                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
    }

    public static TSource? LastOrDefault<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource?>(LastOrDefault).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    /// <summary>Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns><paramref name="defaultValue" /> if the sequence is empty or if no elements pass the test in the predicate function; otherwise, the last element that passes the test in the predicate function.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static TSource LastOrDefault<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource, TSource>(LastOrDefault).Method,
                source.Expression, Expression.Quote(predicate), Expression.Constant(defaultValue, typeof(TSource))
            ));
    }

    public static TSource Single<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource>(Single).Method,
                source.Expression));
    }

    public static TSource Single<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource>(Single).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static TSource? SingleOrDefault<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource?>(SingleOrDefault).Method,
                source.Expression));
    }

    /// <summary>Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return the single element of.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns>The single element of the input sequence, or <paramref name="defaultValue" /> if the sequence contains no elements.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">The input sequence contains more than one element.</exception>
    public static TSource SingleOrDefault<TSource>(this IRepository<TSource> source, TSource defaultValue) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, TSource>(SingleOrDefault).Method,
                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
    }

    public static TSource? SingleOrDefault<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource?>(SingleOrDefault).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    /// <summary>Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to return a single element from.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns>The single element of the input sequence that satisfies the condition, or <paramref name="defaultValue" /> if no such element is found.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">More than one element satisfies the condition in <paramref name="predicate" />.</exception>
    public static TSource SingleOrDefault<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate, TSource defaultValue) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, TSource, TSource>(SingleOrDefault).Method,
                source.Expression, Expression.Quote(predicate), Expression.Constant(defaultValue, typeof(TSource))));
    }

    public static TSource ElementAt<TSource>(this IRepository<TSource> source, int index) {
        IsNotNull(source);

        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, TSource>(ElementAt).Method,
                source.Expression, Expression.Constant(index)));
    }

    /// <summary>Returns the element at a specified index in a sequence.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IRepository{T}" /> to return an element from.</param>
    /// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence.</exception>
    /// <returns>The element at the specified position in the <paramref name="source" /> sequence.</returns>
    public static TSource ElementAt<TSource>(this IRepository<TSource> source, Index index) {
        IsNotNull(source);

        if (index.IsFromEnd && index.Value == 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Index, TSource>(ElementAt).Method,
                source.Expression, Expression.Constant(index)));
    }

    public static TSource? ElementAtOrDefault<TSource>(this IRepository<TSource> source, int index) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, TSource?>(ElementAtOrDefault).Method,
                source.Expression, Expression.Constant(index)));
    }

    /// <summary>Returns the element at a specified index in a sequence or a default value if the index is out of range.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IRepository{T}" /> to return an element from.</param>
    /// <param name="index">The index of the element to retrieve, which is either from the start or the end.</param>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <returns><see langword="default" /> if <paramref name="index" /> is outside the bounds of the <paramref name="source" /> sequence; otherwise, the element at the specified position in the <paramref name="source" /> sequence.</returns>
    public static TSource? ElementAtOrDefault<TSource>(this IRepository<TSource> source, Index index) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Index, TSource?>(ElementAtOrDefault).Method,
                source.Expression, Expression.Constant(index)));
    }

    public static IRepository<TSource?> DefaultIfEmpty<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IRepository<TSource?>>(DefaultIfEmpty).Method,
                source.Expression));
    }

    public static IRepository<TSource> DefaultIfEmpty<TSource>(this IRepository<TSource> source, TSource defaultValue) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, IRepository<TSource?>>(DefaultIfEmpty).Method,
                source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
    }

    public static bool Contains<TSource>(this IRepository<TSource> source, TSource item) {
        IsNotNull(source);

        return source.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, bool>(Contains).Method,
                source.Expression, Expression.Constant(item, typeof(TSource))));
    }

    public static bool Contains<TSource>(this IRepository<TSource> source, TSource item, IEqualityComparer<TSource>? comparer) {
        IsNotNull(source);

        return source.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, IEqualityComparer<TSource>, bool>(Contains).Method,
                source.Expression, Expression.Constant(item, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
    }

    public static IRepository<TSource> Reverse<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IRepository<TSource>>(Reverse).Method,
                source.Expression));
    }

    public static bool SequenceEqual<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, bool>(SequenceEqual).Method,
                source1.Expression, GetSourceExpression(source2)));
    }

    public static bool SequenceEqual<TSource>(this IRepository<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource>? comparer) {
        IsNotNull(source1);
        IsNotNull(source2);

        return source1.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IEnumerable<TSource>, IEqualityComparer<TSource>, bool>(SequenceEqual).Method,
                source1.Expression,
                GetSourceExpression(source2),
                Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
    }

    public static bool Any<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, bool>(Any).Method,
                source.Expression));
    }

    public static bool Any<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, bool>(Any).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static bool All<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<bool>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, bool>(All).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static int Count<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<int>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int>(Count).Method,
                source.Expression));
    }

    public static int Count<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<int>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, int>(Count).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static long LongCount<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<long>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, long>(LongCount).Method,
                source.Expression));
    }

    public static long LongCount<TSource>(this IRepository<TSource> source, Expression<Func<TSource, bool>> predicate) {
        IsNotNull(source);
        IsNotNull(predicate);

        return source.Provider.Execute<long>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, bool>>, long>(LongCount).Method,
                source.Expression, Expression.Quote(predicate)));
    }

    public static TSource? Min<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource?>(Min).Method,
                source.Expression));
    }

    /// <summary>Returns the minimum value in a generic <see cref="System.Linq.IRepository{T}" />.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="comparer">The <see cref="IComparer{T}" /> to compare values.</param>
    /// <returns>The minimum value in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">No object in <paramref name="source" /> implements the <see cref="System.IComparable" /> or <see cref="System.IComparable{T}" /> interface.</exception>
    public static TSource? Min<TSource>(this IRepository<TSource> source, IComparer<TSource>? comparer) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IComparer<TSource>, TSource?>(Min).Method,
                source.Expression,
                Expression.Constant(comparer, typeof(IComparer<TSource>))));
    }

    public static TResult? Min<TSource, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TResult>>, TResult?>(Min).Method,
                source.Expression, Expression.Quote(selector)));
    }

    /// <summary>Returns the minimum value in a generic <see cref="IRepository{T}"/> according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>The value with the minimum key in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">No key extracted from <paramref name="source" /> implements the <see cref="IComparable" /> or <see cref="IComparable{TKey}" /> interface.</exception>
    public static TSource? MinBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, TSource?>(MinBy).Method,
                source.Expression,
                Expression.Quote(keySelector)));
    }

    /// <summary>Returns the minimum value in a generic <see cref="IRepository{T}"/> according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
    /// <param name="source">A sequence of values to determine the minimum value of.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">The <see cref="IComparer{TKey}" /> to compare keys.</param>
    /// <returns>The value with the minimum key in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">No key extracted from <paramref name="source" /> implements the <see cref="IComparable" /> or <see cref="IComparable{TKey}" /> interface.</exception>
    public static TSource? MinBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TSource>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IComparer<TSource>, TSource?>(MinBy).Method,
                source.Expression,
                Expression.Quote(keySelector),
                Expression.Constant(comparer, typeof(IComparer<TSource>))));
    }

    public static TSource? Max<TSource>(this IRepository<TSource> source) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource?>(Max).Method,
                source.Expression));
    }

    /// <summary>Returns the maximum value in a generic <see cref="System.Linq.IRepository{T}" />.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="comparer">The <see cref="IComparer{T}" /> to compare values.</param>
    /// <returns>The maximum value in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    public static TSource? Max<TSource>(this IRepository<TSource> source, IComparer<TSource>? comparer) {
        IsNotNull(source);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, IComparer<TSource>, TSource?>(Max).Method,
                source.Expression,
                Expression.Constant(comparer, typeof(IComparer<TSource>))));
    }

    public static TResult? Max<TSource, TResult>(this IRepository<TSource> source, Expression<Func<TSource, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TResult>>, TResult?>(Max).Method,
                source.Expression, Expression.Quote(selector)));
    }

    /// <summary>Returns the maximum value in a generic <see cref="IRepository{T}"/> according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>The value with the maximum key in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">No key extracted from <paramref name="source" /> implements the <see cref="IComparable" /> or <see cref="IComparable{TKey}" /> interface.</exception>
    public static TSource? MaxBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, TSource?>(MaxBy).Method,
                source.Expression,
                Expression.Quote(keySelector)));
    }

    /// <summary>Returns the maximum value in a generic <see cref="IRepository{T}"/> according to a specified key selector function.</summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of key to compare elements by.</typeparam>
    /// <param name="source">A sequence of values to determine the maximum value of.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">The <see cref="IComparer{TKey}" /> to compare keys.</param>
    /// <returns>The value with the maximum key in the sequence.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">No key extracted from <paramref name="source" /> implements the <see cref="IComparable" /> or <see cref="IComparable{TKey}" /> interface.</exception>
    public static TSource? MaxBy<TSource, TKey>(this IRepository<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TSource>? comparer) {
        IsNotNull(source);
        IsNotNull(keySelector);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TKey>>, IComparer<TSource>, TSource?>(MaxBy).Method,
                source.Expression,
                Expression.Quote(keySelector),
                Expression.Constant(comparer, typeof(IComparer<TSource>))));
    }

    public static int Sum(this IRepository<int> source) {
        IsNotNull(source);

        return source.Provider.Execute<int>(
            Expression.Call(
                null,
                new Func<IRepository<int>, int>(Sum).Method,
                source.Expression));
    }

    public static int? Sum(this IRepository<int?> source) {
        IsNotNull(source);

        return source.Provider.Execute<int?>(
            Expression.Call(
                null,
                new Func<IRepository<int?>, int?>(Sum).Method,
                source.Expression));
    }

    public static long Sum(this IRepository<long> source) {
        IsNotNull(source);

        return source.Provider.Execute<long>(
            Expression.Call(
                null,
                new Func<IRepository<long>, long>(Sum).Method,
                source.Expression));
    }

    public static long? Sum(this IRepository<long?> source) {
        IsNotNull(source);

        return source.Provider.Execute<long?>(
            Expression.Call(
                null,
                new Func<IRepository<long?>, long?>(Sum).Method,
                source.Expression));
    }

    public static float Sum(this IRepository<float> source) {
        IsNotNull(source);

        return source.Provider.Execute<float>(
            Expression.Call(
                null,
                new Func<IRepository<float>, float>(Sum).Method,
                source.Expression));
    }

    public static float? Sum(this IRepository<float?> source) {
        IsNotNull(source);

        return source.Provider.Execute<float?>(
            Expression.Call(
                null,
                new Func<IRepository<float?>, float?>(Sum).Method,
                source.Expression));
    }

    public static double Sum(this IRepository<double> source) {
        IsNotNull(source);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<double>, double>(Sum).Method,
                source.Expression));
    }

    public static double? Sum(this IRepository<double?> source) {
        IsNotNull(source);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<double?>, double?>(Sum).Method,
                source.Expression));
    }

    public static decimal Sum(this IRepository<decimal> source) {
        IsNotNull(source);

        return source.Provider.Execute<decimal>(
            Expression.Call(
                null,
                new Func<IRepository<decimal>, decimal>(Sum).Method,
                source.Expression));
    }

    public static decimal? Sum(this IRepository<decimal?> source) {
        IsNotNull(source);

        return source.Provider.Execute<decimal?>(
            Expression.Call(
                null,
                new Func<IRepository<decimal?>, decimal?>(Sum).Method,
                source.Expression));
    }

    public static int Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<int>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int>>, int>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static int? Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<int?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int?>>, int?>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static long Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, long>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<long>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, long>>, long>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static long? Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, long?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<long?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, long?>>, long?>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static float Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, float>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<float>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, float>>, float>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static float? Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, float?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<float?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, float?>>, float?>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, double>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, double>>, double>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double? Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, double?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, double?>>, double?>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static decimal Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, decimal>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<decimal>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, decimal>>, decimal>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static decimal? Sum<TSource>(this IRepository<TSource> source, Expression<Func<TSource, decimal?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<decimal?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, decimal?>>, decimal?>(Sum).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double Average(this IRepository<int> source) {
        IsNotNull(source);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<int>, double>(Average).Method,
                source.Expression));
    }

    public static double? Average(this IRepository<int?> source) {
        IsNotNull(source);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<int?>, double?>(Average).Method,
                source.Expression));
    }

    public static double Average(this IRepository<long> source) {
        IsNotNull(source);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<long>, double>(Average).Method,
                source.Expression));
    }

    public static double? Average(this IRepository<long?> source) {
        IsNotNull(source);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<long?>, double?>(Average).Method,
                source.Expression));
    }

    public static float Average(this IRepository<float> source) {
        IsNotNull(source);

        return source.Provider.Execute<float>(
            Expression.Call(
                null,
                new Func<IRepository<float>, float>(Average).Method,
                source.Expression));
    }

    public static float? Average(this IRepository<float?> source) {
        IsNotNull(source);

        return source.Provider.Execute<float?>(
            Expression.Call(
                null,
                new Func<IRepository<float?>, float?>(Average).Method,
                source.Expression));
    }

    public static double Average(this IRepository<double> source) {
        IsNotNull(source);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<double>, double>(Average).Method,
                source.Expression));
    }

    public static double? Average(this IRepository<double?> source) {
        IsNotNull(source);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<double?>, double?>(Average).Method,
                source.Expression));
    }

    public static decimal Average(this IRepository<decimal> source) {
        IsNotNull(source);

        return source.Provider.Execute<decimal>(
            Expression.Call(
                null,
                new Func<IRepository<decimal>, decimal>(Average).Method,
                source.Expression));
    }

    public static decimal? Average(this IRepository<decimal?> source) {
        IsNotNull(source);

        return source.Provider.Execute<decimal?>(
            Expression.Call(
                null,
                new Func<IRepository<decimal?>, decimal?>(Average).Method,
                source.Expression));
    }

    public static double Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int>>, double>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double? Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, int?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, int?>>, double?>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static float Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, float>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<float>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, float>>, float>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static float? Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, float?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<float?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, float?>>, float?>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, long>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, long>>, double>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double? Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, long?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, long?>>, double?>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, double>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, double>>, double>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static double? Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, double?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<double?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, double?>>, double?>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static decimal Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, decimal>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<decimal>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, decimal>>, decimal>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static decimal? Average<TSource>(this IRepository<TSource> source, Expression<Func<TSource, decimal?>> selector) {
        IsNotNull(source);
        IsNotNull(selector);

        return source.Provider.Execute<decimal?>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, decimal?>>, decimal?>(Average).Method,
                source.Expression, Expression.Quote(selector)));
    }

    public static TSource Aggregate<TSource>(this IRepository<TSource> source, Expression<Func<TSource, TSource, TSource>> func) {
        IsNotNull(source);
        IsNotNull(func);

        return source.Provider.Execute<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, Expression<Func<TSource, TSource, TSource>>, TSource>(Aggregate).Method,
                source.Expression, Expression.Quote(func)));
    }

    public static TAccumulate Aggregate<TSource, TAccumulate>(this IRepository<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func) {
        IsNotNull(source);
        IsNotNull(func);

        return source.Provider.Execute<TAccumulate>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, TAccumulate>(Aggregate).Method,
                source.Expression, Expression.Constant(seed), Expression.Quote(func)));
    }

    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IRepository<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector) {
        IsNotNull(source);
        IsNotNull(func);
        IsNotNull(selector);

        return source.Provider.Execute<TResult>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TAccumulate, Expression<Func<TAccumulate, TSource, TAccumulate>>, Expression<Func<TAccumulate, TResult>>, TResult>(Aggregate).Method,
                source.Expression, Expression.Constant(seed), Expression.Quote(func), Expression.Quote(selector)));
    }

    public static IRepository<TSource> SkipLast<TSource>(this IRepository<TSource> source, int count) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, IRepository<TSource>>(SkipLast).Method,
                source.Expression, Expression.Constant(count)
                ));
    }

    public static IRepository<TSource> TakeLast<TSource>(this IRepository<TSource> source, int count) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, int, IRepository<TSource>>(TakeLast).Method,
                source.Expression, Expression.Constant(count)));
    }

    public static IRepository<TSource> Append<TSource>(this IRepository<TSource> source, TSource element) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, IRepository<TSource>>(Append).Method,
                source.Expression, Expression.Constant(element)));
    }

    public static IRepository<TSource> Prepend<TSource>(this IRepository<TSource> source, TSource element) {
        IsNotNull(source);

        return source.Provider.CreateRepository<TSource>(
            Expression.Call(
                null,
                new Func<IRepository<TSource>, TSource, IRepository<TSource>>(Prepend).Method,
                source.Expression, Expression.Constant(element)));
    }
}

