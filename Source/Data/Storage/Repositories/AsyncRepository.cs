namespace DotNetToolbox.Data.Repositories;

public class AsyncRepository<TItem>
    : IAsyncRepository<TItem>,
      IEnumerable<TItem>,
      IAsyncEnumerable<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data;

    protected AsyncRepository(IStrategyFactory? provider = null)
        : this([], provider) {
    }

    protected AsyncRepository(IEnumerable<TItem> source, IStrategyFactory? provider = null) {
        var list = source.ToList();
        _data = IsNotNull(list).AsQueryable();
        Strategy = provider?.GetAsyncRepositoryStrategy(_data)
            ?? new InMemoryAsyncRepositoryStrategy<AsyncRepository<TItem>, TItem>(_data);
    }

    protected AsyncRepositoryStrategy<TItem> Strategy { get; }

    public IEnumerator<TItem> GetEnumerator()
        => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1725:Parameter names should match base declaration", Justification = "<Pending>")]
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => _data is IAsyncEnumerable<TItem> asyncData
               ? asyncData.GetAsyncEnumerator(ct)
               : throw new NotSupportedException("This collection does not support asynchronous enumeration.");

    public AsyncRepository<TResult> OfType<TResult>()
        where TResult : class
        => Strategy.OfType<TResult>();

    public AsyncRepository<TResult> Cast<TResult>()
        where TResult : class
        => Strategy.Cast<TResult>();

    public AsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => Strategy.Where(predicate);

    public AsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.Where(predicate);

    public AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => Strategy.Select(selector);

    public AsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => Strategy.Select(selector);

    public AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => Strategy.SelectMany(selector);

    public AsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => Strategy.SelectMany(selector);

    public AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => Strategy.SelectMany(collectionSelector, resultSelector);

    public AsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => Strategy.SelectMany(collectionSelector, resultSelector);

    public AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector)
        where TResult : class
        => Strategy.Join(inner,
                          outerKeySelector,
                          innerKeySelector,
                          resultSelector);

    public AsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                Expression<Func<TKey>> outerKeySelector,
                                                                Expression<Func<TInner, TKey>> innerKeySelector,
                                                                Expression<Func<TInner, TResult>> resultSelector,
                                                                IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.Join(inner,
                          outerKeySelector,
                          innerKeySelector,
                          resultSelector,
                          comparer);

    public AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => Strategy.GroupJoin(inner,
                               outerKeySelector,
                               innerKeySelector,
                               resultSelector);

    public AsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                     Expression<Func<TKey>> outerKeySelector,
                                                                     Expression<Func<TInner, TKey>> innerKeySelector,
                                                                     Expression<Func<IEnumerable<TInner>, TResult>> resultSelector,
                                                                     IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.GroupJoin(inner,
                               outerKeySelector,
                               innerKeySelector,
                               resultSelector,
                               comparer);

    public AsyncOrderedRepository<TItem> Order()
        => Strategy.Order();

    public AsyncOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => Strategy.Order(comparer);

    public AsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.OrderBy(keySelector);

    public AsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.OrderBy(keySelector, comparer);

    public AsyncOrderedRepository<TItem> OrderDescending()
        => Strategy.OrderDescending();

    public AsyncOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => Strategy.OrderDescending(comparer);

    public AsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.OrderByDescending(keySelector);

    public AsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.OrderByDescending(keySelector, comparer);

    public AsyncRepository<TItem> Take(int count)
        => Strategy.Take(count);

    public AsyncRepository<TItem> Take(Range range)
        => Strategy.Take(range);

    public AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => Strategy.TakeWhile(predicate);

    public AsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.TakeWhile(predicate);

    public AsyncRepository<TItem> TakeLast(int count)
        => Strategy.TakeLast(count);

    public AsyncRepository<TItem> Skip(int count)
        => Strategy.Skip(count);

    public AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => Strategy.SkipWhile(predicate);

    public AsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.SkipWhile(predicate);

    public AsyncRepository<TItem> SkipLast(int count)
        => Strategy.SkipLast(count);

    public AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.GroupBy(keySelector);

    public AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => Strategy.GroupBy(keySelector, elementSelector);

    public AsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, comparer);

    public AsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, elementSelector, comparer);

    public AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => Strategy.GroupBy(keySelector, elementSelector, resultSelector);

    public AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => Strategy.GroupBy(keySelector, resultSelector);

    public AsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.GroupBy(keySelector, resultSelector, comparer);

    public AsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.GroupBy(keySelector,
                             elementSelector,
                             resultSelector,
                             comparer);

    public AsyncRepository<TItem> Distinct()
        => Strategy.Distinct();

    public AsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => Strategy.Distinct(comparer);

    public AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.DistinctBy(keySelector);

    public AsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.DistinctBy(keySelector, comparer);

    public AsyncRepository<TItem[]> Chunk(int size)
        => Strategy.Chunk(size);

    public AsyncRepository<TItem> Concat(IEnumerable<TItem> source)
        => Strategy.Concat(source);

    public AsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => Strategy.Combine(source2, resultSelector);

    public AsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => Strategy.Zip(source);

    public AsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => Strategy.Zip(source2, source3);

    public AsyncRepository<TItem> Union(IEnumerable<TItem> source2)
        => Strategy.Union(source2);

    public AsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Union(source2, comparer);

    public AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.UnionBy(source2, keySelector);

    public AsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.UnionBy(source2, keySelector, comparer);

    public AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => Strategy.Intersect(source2);

    public AsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Intersect(source2, comparer);

    public AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.IntersectBy(source2, keySelector);

    public AsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.IntersectBy(source2, keySelector, comparer);

    public AsyncRepository<TItem> Except(IEnumerable<TItem> source2)
        => Strategy.Except(source2);

    public AsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Except(source2, comparer);

    public AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ExceptBy(source2, keySelector);

    public AsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.ExceptBy(source2, keySelector, comparer);

    public AsyncRepository<TItem> DefaultIfEmpty()
        => Strategy.DefaultIfEmpty();

    public AsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => Strategy.DefaultIfEmpty(defaultValue);

    public AsyncRepository<TItem> Reverse()
        => Strategy.Reverse();

    public AsyncRepository<TItem> Append(TItem element)
        => Strategy.Append(element);

    public AsyncRepository<TItem> Prepend(TItem element)
        => Strategy.Prepend(element);

    public Task<IReadOnlyList<TItem>> ToArray(CancellationToken ct = default)
        => Strategy.ToArray(ct);

    public Task<IReadOnlyList<TItem>> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Strategy.ToArray(mapping, ct);

    public Task<IList<TItem>> ToList(CancellationToken ct = default)
        => Strategy.ToList(ct);

    public Task<IList<TResult>> ToList<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Strategy.ToList(mapping, ct);

    public Task<ISet<TItem>> ToHashSet(CancellationToken ct = default)
        => Strategy.ToHashSet(ct);

    public Task<ISet<TResult>> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Strategy.ToHashSet(mapping, ct);

    public Task<IRepository<TResult>> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => Strategy.ToRepository(mapping, ct);

    public Task<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(Expression<Func<TItem, TKey>> selectKey, Expression<Func<TItem, TValue>> selectValue, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ToDictionary(selectKey, selectValue, ct);

    public Task<TItem> First(CancellationToken ct = default)
        => Strategy.First(ct);

    public Task<TItem> First(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.First(predicate, ct);

    public Task<TItem> FirstOrDefault(CancellationToken ct = default)
        => Strategy.FirstOrDefault(ct);

    public Task<TItem> FirstOrDefault(TItem defaultValue, CancellationToken ct = default)
        => Strategy.FirstOrDefault(defaultValue, ct);

    public Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.FirstOrDefault(predicate, ct);

    public Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Strategy.FirstOrDefault(predicate, defaultValue, ct);

    public Task<TItem> Last(CancellationToken ct = default)
        => Strategy.Last(ct);

    public Task<TItem> Last(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.Last(predicate, ct);

    public Task<TItem> LastOrDefault(CancellationToken ct = default)
        => Strategy.LastOrDefault(ct);

    public Task<TItem> LastOrDefault(TItem defaultValue, CancellationToken ct = default)
        => Strategy.LastOrDefault(defaultValue, ct);

    public Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.LastOrDefault(predicate, ct);

    public Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Strategy.LastOrDefault(predicate, defaultValue, ct);

    public Task<TItem> Single(CancellationToken ct = default)
        => Strategy.Single(ct);

    public Task<TItem> Single(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.Single(predicate, ct);

    public Task<TItem> SingleOrDefault(CancellationToken ct = default)
        => Strategy.SingleOrDefault(ct);

    public Task<TItem> SingleOrDefault(TItem defaultValue, CancellationToken ct = default)
        => Strategy.SingleOrDefault(defaultValue, ct);

    public Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.SingleOrDefault(predicate, ct);

    public Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Strategy.SingleOrDefault(predicate, defaultValue, ct);

    public Task<TItem> ElementAt(int index, CancellationToken ct = default)
        => Strategy.ElementAt(index, ct);

    public Task<TItem> ElementAt(Index index, CancellationToken ct = default)
        => Strategy.ElementAt(index, ct);

    public Task<TItem> ElementAtOrDefault(int index, CancellationToken ct = default)
        => Strategy.ElementAtOrDefault(index, ct);

    public Task<TItem> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => Strategy.ElementAtOrDefault(index, ct);

    public Task<bool> Contains(TItem item, CancellationToken ct = default)
        => Strategy.Contains(item, ct);

    public Task<bool> Contains(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.Contains(item, comparer, ct);

    public Task<bool> SequenceEqual(IEnumerable<TItem> source2, CancellationToken ct = default)
        => Strategy.SequenceEqual(source2, ct);

    public Task<bool> SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.SequenceEqual(source2, comparer, ct);

    public Task<bool> Any(CancellationToken ct = default)
        => Strategy.Any(ct);

    public Task<bool> Any(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.Any(predicate, ct);

    public Task<bool> All(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.All(predicate, ct);

    public Task<int?> Count(CancellationToken ct = default)
        => Strategy.Count(ct);

    public Task<int?> Count(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.Count(predicate, ct);

    public Task<long?> LongCount(CancellationToken ct = default)
        => Strategy.LongCount(ct);

    public Task<long?> LongCount(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.LongCount(predicate, ct);

    public Task<TItem> Min(CancellationToken ct = default)
        => Strategy.Min(ct);

    public Task<TResult> Min<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => Strategy.Min(selector, ct);

    public Task<TItem> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Strategy.MinBy(keySelector, ct);

    public Task<TItem> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.MinBy(keySelector, comparer, ct);

    public Task<TItem> Max(CancellationToken ct = default)
        => Strategy.Max(ct);

    public Task<TResult> Max<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => Strategy.Max(selector, ct);

    public Task<TItem> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Strategy.MaxBy(keySelector, ct);

    public Task<TItem> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.MaxBy(keySelector, comparer, ct);

    public Task<int?> Sum(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<int?> Sum(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<long?> Sum(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<long?> Sum(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<float> Sum(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<float?> Sum(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<double> Sum(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<double?> Sum(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<decimal> Sum(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<decimal?> Sum(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Strategy.Sum(selector, ct);

    public Task<double> Average(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<double?> Average(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<float> Average(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<float?> Average(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<double> Average(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<double?> Average(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<double> Average(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<double?> Average(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<decimal> Average(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<decimal?> Average(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Strategy.Average(selector, ct);

    public Task<TItem> Aggregate(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => Strategy.Aggregate(func, ct);

    public Task<TAccumulate> Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => Strategy.Aggregate(seed, func, ct);

    public Task<TResult> Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => Strategy.Aggregate(seed,
                                                                                                                                                                                                                                           func,
                                                                                                                                                                                                                                           selector,
                                                                                                                                                                                                                                           ct);

    public Task Add(TItem newItem, CancellationToken ct = default)
        => Strategy.Add(newItem, ct);

    public Task Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.Update(predicate, updatedItem, ct);

    public Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.Remove(predicate, ct);
}
