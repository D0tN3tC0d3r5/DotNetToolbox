namespace DotNetToolbox.Data.Repositories;

public interface IAsyncQueryableRepository {
    AsyncRepository<TResult> OfType<TResult>()
        where TResult : class;
    AsyncRepository<TResult> Cast<TResult>()
        where TResult : class;
}

public interface IAsyncQueryableRepository<TItem>
    : IAsyncQueryableRepository
    where TItem : class {
    AsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate);

    AsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate);

    AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class;

    AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class;

    AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class;

    AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class;

    AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                              Expression<Func<TKey>> outerKeySelector,
                                                              Expression<Func<TInner, TKey>> innerKeySelector,
                                                              Expression<Func<TInner, TResult>> resultSelector,
                                                              IEqualityComparer<TKey>? comparer)
        where TResult : class;

    AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                   Expression<Func<TKey>> outerKeySelector,
                                                                   Expression<Func<TInner, TKey>> innerKeySelector,
                                                                   Expression<Func<IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                   Expression<Func<TKey>> outerKeySelector,
                                                                   Expression<Func<TInner, TKey>> innerKeySelector,
                                                                   Expression<Func<IEnumerable<TInner>, TResult>> resultSelector,
                                                                   IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IOrderedQueryableRepository<TItem> Order();

    IOrderedQueryableRepository<TItem> Order(IComparer<TItem> comparer);

    IOrderedQueryableRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedQueryableRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IOrderedQueryableRepository<TItem> OrderDescending();

    IOrderedQueryableRepository<TItem> OrderDescending(IComparer<TItem> comparer);

    IOrderedQueryableRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedQueryableRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IOrderedQueryableRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedQueryableRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IOrderedQueryableRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedQueryableRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    AsyncRepository<TItem> Take(int count);

    AsyncRepository<TItem> Take(Range range);

    AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate);

    AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate);

    AsyncRepository<TItem> TakeLast(int count);

    AsyncRepository<TItem> Skip(int count);

    AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate);

    AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate);

    AsyncRepository<TItem> SkipLast(int count);

    AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector);

    AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer);

    AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector,
                                                                   Expression<Func<TItem, TElement>> elementSelector,
                                                                   Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector,
                                                                   IEqualityComparer<TKey>? comparer)
        where TResult : class;

    AsyncRepository<TItem> Distinct();

    AsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer);

    AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    AsyncRepository<TItem[]> Chunk(int size);

    AsyncRepository<TItem> Concat(IEnumerable<TItem> source);

    AsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class;

    AsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source);

    AsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3);

    AsyncRepository<TItem> Union(IEnumerable<TItem> source2);

    AsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector);

    AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2);

    AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    AsyncRepository<TItem> Except(IEnumerable<TItem> source2);

    AsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    AsyncRepository<TItem> DefaultIfEmpty();

    AsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue);

    AsyncRepository<TItem> Reverse();

    AsyncRepository<TItem> Append(TItem element);

    AsyncRepository<TItem> Prepend(TItem element);
}
