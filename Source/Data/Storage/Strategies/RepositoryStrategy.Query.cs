namespace DotNetToolbox.Data.Strategies;

public abstract partial class RepositoryStrategy {
    public virtual IRepository<TResult> OfType<TResult>()
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> Cast<TResult>()
        where TResult : class
        => throw new NotImplementedException();
}

public abstract partial class RepositoryStrategy<TItem> {

    public virtual IRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                   Expression<Func<TItem, TKey>> outerKeySelector,
                                                                   Expression<Func<TInner, TKey>> innerKeySelector,
                                                                   Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                   Expression<Func<TItem, TKey>> outerKeySelector,
                                                                   Expression<Func<TInner, TKey>> innerKeySelector,
                                                                   Expression<Func<TItem, TInner, TResult>> resultSelector,
                                                                   IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                        Expression<Func<TItem, TKey>> outerKeySelector,
                                                                        Expression<Func<TInner, TKey>> innerKeySelector,
                                                                        Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                        Expression<Func<TItem, TKey>> outerKeySelector,
                                                                        Expression<Func<TInner, TKey>> innerKeySelector,
                                                                        Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector,
                                                                        IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> Order()
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> OrderDescending()
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Take(int count)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Take(Range range)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> TakeLast(int count)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Skip(int count)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> SkipLast(int count)
        => throw new NotImplementedException();

    public virtual IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => throw new NotImplementedException();

    public virtual IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Distinct()
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem[]> Chunk(int size)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Concat(IEnumerable<TItem> source)
        => throw new NotImplementedException();

    public virtual IRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => throw new NotImplementedException();

    public virtual IRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Union(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Except(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    #pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    public virtual IRepository<TItem?> DefaultIfEmpty()
        => throw new NotImplementedException();
    #pragma warning restore CS8634

    public virtual IRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Reverse()
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Append(TItem element)
        => throw new NotImplementedException();

    public virtual IRepository<TItem> Prepend(TItem element)
        => throw new NotImplementedException();
}
