namespace DotNetToolbox.Data.Repositories;

public class AsyncRepository<TItem>
    : IAsyncRepository<TItem>,
      IEnumerable<TItem>,
      IAsyncEnumerable<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data;
    private readonly AsyncRepositoryStrategy<TItem> _strategy;

    protected AsyncRepository(IStrategyFactory? provider = null)
        : this([], provider) {
    }

    protected AsyncRepository(IEnumerable<TItem> source, IStrategyFactory? provider = null) {
        var list = source.ToList();
        _data = IsNotNull(list).AsQueryable();
        _strategy = provider?.GetAsyncRepositoryStrategy(_data)
            ?? new InMemoryAsyncRepositoryStrategy<AsyncRepository<TItem>, TItem>(_data);
    }

    public IEnumerator<TItem> GetEnumerator()
        => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => _data is IAsyncEnumerable<TItem> asyncData
               ? asyncData.GetAsyncEnumerator(ct)
               : throw new NotSupportedException("This collection does not support asynchronous enumeration.");

    public AsyncRepository<TResult> OfType<TResult>()
        where TResult : class
        => _strategy.OfType<TResult>();

    public AsyncRepository<TResult> Cast<TResult>()
        where TResult : class
        => _strategy.Cast<TResult>();

    public AsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                Expression<Func<TKey>> outerKeySelector,
                                                                Expression<Func<TInner, TKey>> innerKeySelector,
                                                                Expression<Func<TInner, TResult>> resultSelector,
                                                                IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                     Expression<Func<TKey>> outerKeySelector,
                                                                     Expression<Func<TInner, TKey>> innerKeySelector,
                                                                     Expression<Func<IEnumerable<TInner>, TResult>> resultSelector,
                                                                     IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> Order()
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> Order(IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderDescending()
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual IOrderedQueryableRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Take(int count)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Take(Range range)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> TakeLast(int count)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Skip(int count)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> SkipLast(int count)
        => throw new NotImplementedException();

    public AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => throw new NotImplementedException();

    public AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Distinct()
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem[]> Chunk(int size)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Concat(IEnumerable<TItem> source)
        => throw new NotImplementedException();

    public AsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public AsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => throw new NotImplementedException();

    public AsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Union(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Except(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> DefaultIfEmpty()
        => throw new NotImplementedException();

    public AsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Reverse()
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Append(TItem element)
        => throw new NotImplementedException();

    public AsyncRepository<TItem> Prepend(TItem element)
        => throw new NotImplementedException();

    public virtual Task<TItem> First(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> First(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Last(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Last(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Single(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Single(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAt(int index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAt(Index index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAtOrDefault(int index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Contains(TItem item, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Contains(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> SequenceEqual(IEnumerable<TItem> source2, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Any(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> Any(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<bool> All(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Count(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Count(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> LongCount(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> LongCount(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Min(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> Min<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Max(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> Max<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Sum(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<int?> Sum(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> Sum(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<long?> Sum(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float> Sum(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float?> Sum(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Sum(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Sum(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal> Sum(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal?> Sum(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Average(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Average(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float> Average(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<float?> Average(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Average(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Average(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double> Average(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<double?> Average(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal> Average(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<decimal?> Average(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TItem> Aggregate(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TAccumulate> Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<TResult> Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IReadOnlyList<TItem>> ToArray(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IReadOnlyList<TItem>> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IList<TItem>> ToList(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IList<TResult>> ToList<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<ISet<TItem>> ToHashSet(CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<ISet<TResult>> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task<IRepository<TResult>> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => throw new NotImplementedException();

    public virtual Task<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(Expression<Func<TItem, TKey>> selectKey, Expression<Func<TItem, TValue>> selectValue, CancellationToken ct = default)
        where TKey : notnull
        => throw new NotImplementedException();

    public virtual Task Add(TItem newItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => throw new NotImplementedException();

    public virtual Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => throw new NotImplementedException();
}
