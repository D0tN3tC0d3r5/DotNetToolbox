namespace DotNetToolbox.Data.Repositories;

public class AsyncRepository<TItem> : AsyncRepository<AsyncRepository<TItem>, TItem>
    where TItem : class {
    public AsyncRepository(IStrategyFactory? provider = null)
        : base([], provider) { }

    public AsyncRepository(IEnumerable<TItem> data, IStrategyFactory? provider = null)
        : base(data, provider) { }
}

public class AsyncRepository<TRepository, TItem>
    : IAsyncOrderedRepository<TItem>,
      IEnumerable<TItem>,
      IAsyncEnumerable<TItem>
    where TRepository : IAsyncOrderedRepository<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data;

    protected AsyncRepository(IStrategyFactory? provider = null)
        : this([], provider) {
    }

    protected AsyncRepository(IEnumerable<TItem> source, IStrategyFactory? provider = null) {
        var list = source.ToList();
        _data = IsNotNull(list).AsQueryable();
        Strategy = provider?.GetAsyncRepositoryStrategy(_data)
            ?? new InMemoryAsyncRepositoryStrategy<TRepository, TItem>(_data);
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

    public IAsyncRepository<TResult> OfType<TResult>()
        where TResult : class
        => Strategy.OfType<TResult>();

    public IAsyncRepository<TResult> Cast<TResult>()
        where TResult : class
        => Strategy.Cast<TResult>();

    public IAsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => Strategy.Where(predicate);

    public IAsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.Where(predicate);

    public IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => Strategy.Select(selector);

    public IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => Strategy.Select(selector);

    public IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => Strategy.SelectMany(selector);

    public IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => Strategy.SelectMany(selector);

    public IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => Strategy.SelectMany(collectionSelector, resultSelector);

    public IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => Strategy.SelectMany(collectionSelector, resultSelector);

    public IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                 Expression<Func<TItem, TKey>> outerKeySelector,
                                                                 Expression<Func<TInner, TKey>> innerKeySelector,
                                                                 Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class
        => Strategy.Join(inner,
                          outerKeySelector,
                          innerKeySelector,
                          resultSelector);

    public IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                Expression<Func<TItem, TKey>> outerKeySelector,
                                                                Expression<Func<TInner, TKey>> innerKeySelector,
                                                                Expression<Func<TItem, TInner, TResult>> resultSelector,
                                                                IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.Join(inner,
                          outerKeySelector,
                          innerKeySelector,
                          resultSelector,
                          comparer);

    public IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                      Expression<Func<TItem, TKey>> outerKeySelector,
                                                                      Expression<Func<TInner, TKey>> innerKeySelector,
                                                                      Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => Strategy.GroupJoin(inner,
                               outerKeySelector,
                               innerKeySelector,
                               resultSelector);

    public IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                     Expression<Func<TItem, TKey>> outerKeySelector,
                                                                     Expression<Func<TInner, TKey>> innerKeySelector,
                                                                     Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector,
                                                                     IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.GroupJoin(inner,
                               outerKeySelector,
                               innerKeySelector,
                               resultSelector,
                               comparer);

    public IAsyncOrderedRepository<TItem> Order()
        => Strategy.Order();

    public IAsyncOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => Strategy.Order(comparer);

    public IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.OrderBy(keySelector);

    public IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.OrderBy(keySelector, comparer);

    public IAsyncOrderedRepository<TItem> OrderDescending()
        => Strategy.OrderDescending();

    public IAsyncOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => Strategy.OrderDescending(comparer);

    public IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.OrderByDescending(keySelector);

    public IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.OrderByDescending(keySelector, comparer);

    public IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenBy(keySelector);

    public IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenBy(keySelector, comparer);

    public IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenByDescending(keySelector);

    public IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenByDescending(keySelector, comparer);

    public IAsyncRepository<TItem> Take(int count)
        => Strategy.Take(count);

    public IAsyncRepository<TItem> Take(Range range)
        => Strategy.Take(range);

    public IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => Strategy.TakeWhile(predicate);

    public IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.TakeWhile(predicate);

    public IAsyncRepository<TItem> TakeLast(int count)
        => Strategy.TakeLast(count);

    public IAsyncRepository<TItem> Skip(int count)
        => Strategy.Skip(count);

    public IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => Strategy.SkipWhile(predicate);

    public IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.SkipWhile(predicate);

    public IAsyncRepository<TItem> SkipLast(int count)
        => Strategy.SkipLast(count);

    public IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.GroupBy(keySelector);

    public IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => Strategy.GroupBy(keySelector, elementSelector);

    public IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, comparer);

    public IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, elementSelector, comparer);

    public IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => Strategy.GroupBy(keySelector, elementSelector, resultSelector);

    public IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => Strategy.GroupBy(keySelector, resultSelector);

    public IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.GroupBy(keySelector, resultSelector, comparer);

    public IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => Strategy.GroupBy(keySelector,
                             elementSelector,
                             resultSelector,
                             comparer);

    public IAsyncRepository<TItem> Distinct()
        => Strategy.Distinct();

    public IAsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => Strategy.Distinct(comparer);

    public IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.DistinctBy(keySelector);

    public IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.DistinctBy(keySelector, comparer);

    public IAsyncRepository<TItem[]> Chunk(int size)
        => Strategy.Chunk(size);

    public IAsyncRepository<TItem> Concat(IEnumerable<TItem> source)
        => Strategy.Concat(source);

    public IAsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => Strategy.Combine(source2, resultSelector);

    public IAsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => Strategy.Zip(source);

    public IAsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => Strategy.Zip(source2, source3);

    public IAsyncRepository<TItem> Union(IEnumerable<TItem> source2)
        => Strategy.Union(source2);

    public IAsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Union(source2, comparer);

    public IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.UnionBy(source2, keySelector);

    public IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.UnionBy(source2, keySelector, comparer);

    public IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => Strategy.Intersect(source2);

    public IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Intersect(source2, comparer);

    public IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.IntersectBy(source2, keySelector);

    public IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.IntersectBy(source2, keySelector, comparer);

    public IAsyncRepository<TItem> Except(IEnumerable<TItem> source2)
        => Strategy.Except(source2);

    public IAsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Except(source2, comparer);

    public IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ExceptBy(source2, keySelector);

    public IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.ExceptBy(source2, keySelector, comparer);

    #pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    public IAsyncRepository<TItem?> DefaultIfEmpty()
        => Strategy.DefaultIfEmpty();
    #pragma warning restore CS8634

    public IAsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => Strategy.DefaultIfEmpty(defaultValue);

    public IAsyncRepository<TItem> Reverse()
        => Strategy.Reverse();

    public IAsyncRepository<TItem> Append(TItem element)
        => Strategy.Append(element);

    public IAsyncRepository<TItem> Prepend(TItem element)
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

    public virtual Task<TResultRepository> ToRepository<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResultRepository : class, IRepository<TResult>
        where TResult : class
        => Strategy.ToRepository<TResultRepository, TResult>(mapping, ct);

    public Task<IRepository<TResult>> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => Strategy.ToRepository(mapping, ct);

    public Task<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(Expression<Func<TItem, TKey>> selectKey, Expression<Func<TItem, TValue>> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ToDictionary(selectKey, selectValue, comparer, ct);

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
