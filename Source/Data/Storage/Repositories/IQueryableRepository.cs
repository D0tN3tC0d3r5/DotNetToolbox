namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository
    : IRepository {
    IRepository<TResult> OfType<TResult>()
        where TResult : class;
    IRepository<TResult> Cast<TResult>()
        where TResult : class;
}

public interface IQueryableRepository<TItem>
    : IQueryableRepository
    where TItem : class {

    IRepository<TItem> Where(Expression<Func<TItem, bool>> predicate);

    IRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate);

    IRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class;

    IRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class;

    IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class;

    IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class;

    IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class;

    IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class;

    IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IOrderedRepository<TItem> Order();

    IOrderedRepository<TItem> Order(IComparer<TItem> comparer);

    IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IOrderedRepository<TItem> OrderDescending();

    IOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer);

    IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer);

    IRepository<TItem> Take(int count);

    IRepository<TItem> Take(Range range);

    IRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate);

    IRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate);

    IRepository<TItem> TakeLast(int count);

    IRepository<TItem> Skip(int count);

    IRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate);

    IRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate);

    IRepository<TItem> SkipLast(int count);

    IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector);

    IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer);

    IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class;

    IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class;

    IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    IRepository<TItem> Distinct();

    IRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer);

    IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IRepository<TItem[]> Chunk(int size);

    IRepository<TItem> Concat(IEnumerable<TItem> source);

    IRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class;

    IRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source);

    IRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3);

    IRepository<TItem> Union(IEnumerable<TItem> source2);

    IRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector);

    IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IRepository<TItem> Intersect(IEnumerable<TItem> source2);

    IRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    IRepository<TItem> Except(IEnumerable<TItem> source2);

    IRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    #pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    IRepository<TItem?> DefaultIfEmpty();
    #pragma warning restore CS8634

    IRepository<TItem> DefaultIfEmpty(TItem defaultValue);

    IRepository<TItem> Reverse();

    IRepository<TItem> Append(TItem element);

    IRepository<TItem> Prepend(TItem element);
}
