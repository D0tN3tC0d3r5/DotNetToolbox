namespace DotNetToolbox.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static IAsyncQueryable<TResult> OfType<TResult>(this IAsyncQueryable query)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> Cast<TResult>(this IAsyncQueryable query)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Where<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Where<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> Select<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TResult>> selector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> Select<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int, TResult>> selector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> SelectMany<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, IEnumerable<TResult>>> selector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> SelectMany<TItem, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> SelectMany<TItem, TCollection, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> SelectMany<TItem, TCollection, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> Join<TItem, TInner, TKey, TResult>(this IAsyncQueryable<TItem> query, IEnumerable<TInner> inner,
                                                                         Expression<Func<TItem, TKey>> outerKeySelector,
                                                                         Expression<Func<TInner, TKey>> innerKeySelector,
                                                                         Expression<Func<TItem, TInner, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> Join<TItem, TInner, TKey, TResult>(this IAsyncQueryable<TItem> query, IEnumerable<TInner> inner,
                                                                         Expression<Func<TItem, TKey>> outerKeySelector,
                                                                         Expression<Func<TInner, TKey>> innerKeySelector,
                                                                         Expression<Func<TItem, TInner, TResult>> resultSelector,
                                                                         IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> GroupJoin<TItem, TInner, TKey, TResult>(this IAsyncQueryable<TItem> query, IEnumerable<TInner> inner,
                                                                              Expression<Func<TItem, TKey>> outerKeySelector,
                                                                              Expression<Func<TInner, TKey>> innerKeySelector,
                                                                              Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> GroupJoin<TItem, TInner, TKey, TResult>(this IAsyncQueryable<TItem> query, IEnumerable<TInner> inner,
                                                                              Expression<Func<TItem, TKey>> outerKeySelector,
                                                                              Expression<Func<TInner, TKey>> innerKeySelector,
                                                                              Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector,
                                                                              IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> Order<TItem>(this IAsyncQueryable<TItem> query)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> Order<TItem>(this IAsyncQueryable<TItem> query, IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> OrderBy<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> OrderBy<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> OrderDescending<TItem>(this IAsyncQueryable<TItem> query)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> OrderDescending<TItem>(this IAsyncQueryable<TItem> query, IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> OrderByDescending<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> OrderByDescending<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> ThenBy<TItem, TKey>(this IOrderedAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> ThenBy<TItem, TKey>(this IOrderedAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> ThenByDescending<TItem, TKey>(this IOrderedAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IOrderedAsyncQueryable<TItem> ThenByDescending<TItem, TKey>(this IOrderedAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Take<TItem>(this IAsyncQueryable<TItem> query, int count)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Take<TItem>(this IAsyncQueryable<TItem> query, Range range)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> TakeWhile<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> TakeWhile<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> TakeLast<TItem>(this IAsyncQueryable<TItem> query, int count)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Skip<TItem>(this IAsyncQueryable<TItem> query, int count)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> SkipWhile<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> SkipWhile<TItem>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> SkipLast<TItem>(this IAsyncQueryable<TItem> query, int count)
        => throw new NotImplementedException();

    public static IAsyncQueryable<IGrouping<TKey, TItem>> GroupBy<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<IGrouping<TKey, TElement>> GroupBy<TItem, TKey, TElement>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<IGrouping<TKey, TItem>> GroupBy<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<IGrouping<TKey, TElement>> GroupBy<TItem, TKey, TElement>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> GroupBy<TItem, TKey, TElement, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> GroupBy<TItem, TKey, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> GroupBy<TItem, TKey, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> GroupBy<TItem, TKey, TElement, TResult>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Distinct<TItem>(this IAsyncQueryable<TItem> query)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Distinct<TItem>(this IAsyncQueryable<TItem> query, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> DistinctBy<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> DistinctBy<TItem, TKey>(this IAsyncQueryable<TItem> query, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem[]> Chunk<TItem>(this IAsyncQueryable<TItem> query, int size)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Concat<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TResult> Combine<TItem, TSecond, TResult>(this IAsyncQueryable<TItem> query, IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<IPack<TItem, TSecond>> Zip<TItem, TSecond>(this IAsyncQueryable<TItem> query, IEnumerable<TSecond> source)
        => throw new NotImplementedException();

    public static IAsyncQueryable<IPack<TItem, TSecond, TThird>> Zip<TItem, TSecond, TThird>(this IAsyncQueryable<TItem> query, IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Union<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Union<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> UnionBy<TItem, TKey>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> UnionBy<TItem, TKey>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Intersect<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Intersect<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> IntersectBy<TItem, TKey>(this IAsyncQueryable<TItem> query, IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> IntersectBy<TItem, TKey>(this IAsyncQueryable<TItem> query, IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Except<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Except<TItem>(this IAsyncQueryable<TItem> query, IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> ExceptBy<TItem, TKey>(this IAsyncQueryable<TItem> query, IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> ExceptBy<TItem, TKey>(this IAsyncQueryable<TItem> query, IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem?> DefaultIfEmpty<TItem>(this IAsyncQueryable<TItem> query)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> DefaultIfEmpty<TItem>(this IAsyncQueryable<TItem> query, TItem defaultValue)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Reverse<TItem>(this IAsyncQueryable<TItem> query)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Append<TItem>(this IAsyncQueryable<TItem> query, TItem element)
        => throw new NotImplementedException();

    public static IAsyncQueryable<TItem> Prepend<TItem>(this IAsyncQueryable<TItem> query, TItem element)
        => throw new NotImplementedException();
}
