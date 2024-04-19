namespace DotNetToolbox.Data.Repositories;

public abstract class AsyncRepositoryQueryStrategy
    : IRepositoryStrategy,
      IAsyncQueryableRepository {
    public virtual AsyncRepository<TResult> OfType<TResult>()
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> Cast<TResult>()
        where TResult : class
        => throw new NotImplementedException();
}

public abstract class AsyncRepositoryQueryStrategy<TItem>
    : AsyncRepositoryQueryStrategy,
      IRepositoryStrategy<TItem>,
      IAsyncQueryableRepository<TItem>
    where TItem : class {

    public virtual AsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate) => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                             Expression<Func<TKey>> outerKeySelector,
                                                                             Expression<Func<TInner, TKey>> innerKeySelector,
                                                                             Expression<Func<TInner, TResult>> resultSelector,
                                                                             IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                                  Expression<Func<TKey>> outerKeySelector,
                                                                                  Expression<Func<TInner, TKey>> innerKeySelector,
                                                                                  Expression<Func<IEnumerable<TInner>, TResult>> resultSelector,
                                                                                  IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> Order() => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> Order(IComparer<TItem> comparer) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderDescending() => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderDescending(IComparer<TItem> comparer) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Take(int count) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Take(Range range) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> TakeLast(int count) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Skip(int count) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> SkipLast(int count) => throw new NotImplementedException();

    public virtual AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector) => throw new NotImplementedException();

    public virtual AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Distinct() => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem[]> Chunk(int size) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Concat(IEnumerable<TItem> source) => throw new NotImplementedException();

    public virtual AsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public virtual AsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source) => throw new NotImplementedException();

    public virtual AsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Union(IEnumerable<TItem> source2) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Except(IEnumerable<TItem> source2) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> DefaultIfEmpty() => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Reverse() => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Append(TItem element) => throw new NotImplementedException();

    public virtual AsyncRepository<TItem> Prepend(TItem element) => throw new NotImplementedException();
}
