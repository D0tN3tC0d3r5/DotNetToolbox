namespace DotNetToolbox.Data.Repositories;

public interface IAsyncQueryableRepository {
    IAsyncRepository<TResult> OfType<TResult>()
        where TResult : class;
    IAsyncRepository<TResult> Cast<TResult>()
        where TResult : class;
}

public interface IAsyncQueryableRepository<TItem>
    : IAsyncQueryableRepository
    where TItem : class {
    IAsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate);

    IAsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate);

    IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class;

    IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class;

    IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class;

    IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class;

    IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                          Expression<Func<TItem, TKey>> outerKeySelector,
                                                          Expression<Func<TInner, TKey>> innerKeySelector,
                                                          Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                              Expression<Func<TItem, TKey>> outerKeySelector,
                                                              Expression<Func<TInner, TKey>> innerKeySelector,
                                                              Expression<Func<TItem, TInner, TResult>> resultSelector,
                                                              IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                   Expression<Func<TItem, TKey>> outerKeySelector,
                                                                   Expression<Func<TInner, TKey>> innerKeySelector,
                                                                   Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                   Expression<Func<TItem, TKey>> outerKeySelector,
                                                                   Expression<Func<TInner, TKey>> innerKeySelector,
                                                                   Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector,
                                                                   IEqualityComparer<TKey>? comparer)
        where TResult : class;

   IAsyncOrderedRepository<TItem> Order();

   IAsyncOrderedRepository<TItem> Order(IComparer<TItem> comparer);

   IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

   IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

   IAsyncOrderedRepository<TItem> OrderDescending();

   IAsyncOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer);

   IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

   IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IAsyncRepository<TItem> Take(int count);

    IAsyncRepository<TItem> Take(Range range);

    IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate);

    IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate);

    IAsyncRepository<TItem> TakeLast(int count);

    IAsyncRepository<TItem> Skip(int count);

    IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate);

    IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate);

    IAsyncRepository<TItem> SkipLast(int count);

    IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector);

    IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer);

    IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector,
                                                                   Expression<Func<TItem, TElement>> elementSelector,
                                                                   Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector,
                                                                   IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IAsyncRepository<TItem> Distinct();

    IAsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer);

    IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IAsyncRepository<TItem[]> Chunk(int size);

    IAsyncRepository<TItem> Concat(IEnumerable<TItem> source);

    IAsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class;

    IAsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source);

    IAsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3);

    IAsyncRepository<TItem> Union(IEnumerable<TItem> source2);

    IAsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector);

    IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2);

    IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IAsyncRepository<TItem> Except(IEnumerable<TItem> source2);

    IAsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    #pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    IAsyncRepository<TItem?> DefaultIfEmpty();
    #pragma warning restore CS8634

    IAsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue);

    IAsyncRepository<TItem> Reverse();

    IAsyncRepository<TItem> Append(TItem element);

    IAsyncRepository<TItem> Prepend(TItem element);
}
