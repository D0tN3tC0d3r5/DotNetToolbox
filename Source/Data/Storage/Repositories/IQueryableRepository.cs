namespace DotNetToolbox.Data.Repositories;

public interface IQueryableRepository {
    Repository<TResult> OfType<TResult>()
        where TResult : class;
    Repository<TResult> Cast<TResult>()
        where TResult : class;
}

public interface IQueryableRepository<TItem>
    : IQueryableRepository
    where TItem : class {

    Repository<TItem> Where(Expression<Func<TItem, bool>> predicate);

    Repository<TItem> Where(Expression<Func<TItem, int, bool>> predicate);

    Repository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class;

    Repository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class;

    Repository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class;

    Repository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class;

    Repository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    Repository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class;

    Repository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector)
        where TResult : class;

    Repository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    Repository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class;

    Repository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
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

    Repository<TItem> Take(int count);

    Repository<TItem> Take(Range range);

    Repository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate);

    Repository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate);

    Repository<TItem> TakeLast(int count);

    Repository<TItem> Skip(int count);

    Repository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate);

    Repository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate);

    Repository<TItem> SkipLast(int count);

    Repository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    Repository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector);

    Repository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    Repository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer);

    Repository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class;

    Repository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class;

    Repository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    Repository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class;

    Repository<TItem> Distinct();

    Repository<TItem> Distinct(IEqualityComparer<TItem>? comparer);

    Repository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    Repository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    Repository<TItem[]> Chunk(int size);

    Repository<TItem> Concat(IEnumerable<TItem> source);

    Repository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class;

    Repository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source);

    Repository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3);

    Repository<TItem> Union(IEnumerable<TItem> source2);

    Repository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    Repository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector);

    Repository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    Repository<TItem> Intersect(IEnumerable<TItem> source2);

    Repository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    Repository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    Repository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    Repository<TItem> Except(IEnumerable<TItem> source2);

    Repository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    Repository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector);

    Repository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer);

    Repository<TItem> DefaultIfEmpty();

    Repository<TItem> DefaultIfEmpty(TItem defaultValue);

    Repository<TItem> Reverse();

    Repository<TItem> Append(TItem element);

    Repository<TItem> Prepend(TItem element);
}
