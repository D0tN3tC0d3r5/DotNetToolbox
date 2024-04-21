namespace DotNetToolbox.Data.Strategies;

public abstract partial class AsyncRepositoryStrategy {
    public virtual IAsyncRepository<TResult> OfType<TResult>()
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> Cast<TResult>()
        where TResult : class
        => throw new NotImplementedException();
}

public abstract partial class AsyncRepositoryStrategy<TItem> {
    public virtual IAsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                         Expression<Func<TItem, TKey>> outerKeySelector,
                                                                         Expression<Func<TInner, TKey>> innerKeySelector,
                                                                         Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                         Expression<Func<TItem, TKey>> outerKeySelector,
                                                                         Expression<Func<TInner, TKey>> innerKeySelector,
                                                                         Expression<Func<TItem, TInner, TResult>> resultSelector,
                                                                         IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                              Expression<Func<TItem, TKey>> outerKeySelector,
                                                                              Expression<Func<TInner, TKey>> innerKeySelector,
                                                                              Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                              Expression<Func<TItem, TKey>> outerKeySelector,
                                                                              Expression<Func<TInner, TKey>> innerKeySelector,
                                                                              Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector,
                                                                              IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> Order()
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> OrderDescending()
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Take(int count)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Take(Range range)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> TakeLast(int count)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Skip(int count)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> SkipLast(int count)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Distinct()
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem[]> Chunk(int size)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Concat(IEnumerable<TItem> source)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IAsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Union(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Except(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    #pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    public virtual IAsyncRepository<TItem?> DefaultIfEmpty()
        => throw new NotImplementedException();
    #pragma warning restore CS8634

    public virtual IAsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Reverse()
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Append(TItem element)
        => throw new NotImplementedException();

    public virtual IAsyncRepository<TItem> Prepend(TItem element)
        => throw new NotImplementedException();
}
