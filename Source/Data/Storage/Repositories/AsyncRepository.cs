namespace DotNetToolbox.Data.Repositories;

public class AsyncRepository<TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : AsyncRepository<AsyncRepository<TStrategy, TItem>, TStrategy, TItem>(data, strategy)
    where TStrategy : class, IAsyncRepositoryStrategy<TItem>
    where TItem : class {
    // ReSharper disable PossibleMultipleEnumeration
    public AsyncRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredAsyncStrategy<TStrategy, TItem>(data)) { }
    // ReSharper enable PossibleMultipleEnumeration
    public AsyncRepository(IStrategyFactory factory)
        : this([], factory) { }
}

public class AsyncRepository<TRepository, TStrategy, TItem>(IEnumerable<TItem> data, TStrategy strategy)
    : IAsyncOrderedRepository<TItem>,
      IEnumerable<TItem>,
      IAsyncEnumerable<TItem>
    where TRepository : AsyncRepository<TRepository, TStrategy, TItem>
    where TStrategy : class, IAsyncRepositoryStrategy<TItem>
    where TItem : class {
    // ReSharper disable once PossibleMultipleEnumeration
    private readonly IQueryable<TItem> _query = data.AsQueryable();

    // ReSharper disable PossibleMultipleEnumeration
    protected AsyncRepository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredAsyncStrategy<TStrategy, TItem>(data)) {
    }
    // ReSharper enable PossibleMultipleEnumeration
    protected AsyncRepository(IStrategyFactory factory)
        : this([], factory) {
    }

    protected TStrategy Strategy { get; } = IsNotNull(strategy);

    public IEnumerator<TItem> GetEnumerator()
        => _query.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1725:Parameter names should match base declaration", Justification = "<Pending>")]
    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken ct = default)
        => new AsyncEnumerator<TItem>(_query.GetEnumerator(), ct);

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

    public Task<IReadOnlyList<TItem>> ToArrayAsync(CancellationToken ct = default)
        => Strategy.ToArrayAsync(ct);

    public Task<IReadOnlyList<TResult>> ToArrayAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Strategy.ToArrayAsync(mapping, ct);

    public Task<IList<TItem>> ToListAsync(CancellationToken ct = default)
        => Strategy.ToListAsync(ct);

    public Task<IList<TResult>> ToListAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Strategy.ToListAsync(mapping, ct);

    public Task<ISet<TItem>> ToHashSetAsync(CancellationToken ct = default)
        => Strategy.ToHashSetAsync(ct);

    public Task<ISet<TResult>> ToHashSetAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Strategy.ToHashSetAsync(mapping, ct);

    public virtual Task<TResultRepository> ToRepositoryAsync<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        where TResultRepository : class, IRepository<TResult>
        => Strategy.ToRepositoryAsync<TResultRepository, TResult>(mapping, ct);

    public Task<IAsyncRepository<TResult>> ToRepositoryAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => Strategy.ToRepositoryAsync(mapping, ct);

    public Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        where TKey : notnull
        => Strategy.ToDictionaryAsync(selectKey, selectValue, comparer, ct);

    public Task<TItem> FirstAsync(CancellationToken ct = default)
        => Strategy.FirstAsync(ct);

    public Task<TItem> FirstAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.FirstAsync(predicate, ct);

    public Task<TItem?> FirstOrDefaultAsync(CancellationToken ct = default)
        => Strategy.FirstOrDefaultAsync(ct);

    public Task<TItem> FirstOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Strategy.FirstOrDefaultAsync(defaultValue, ct);

    public Task<TItem?> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.FirstOrDefaultAsync(predicate, ct);

    public Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Strategy.FirstOrDefaultAsync(predicate, defaultValue, ct);

    public Task<TItem> LastAsync(CancellationToken ct = default)
        => Strategy.LastAsync(ct);

    public Task<TItem> LastAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.LastAsync(predicate, ct);

    public Task<TItem?> LastOrDefaultAsync(CancellationToken ct = default)
        => Strategy.LastOrDefaultAsync(ct);

    public Task<TItem> LastOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Strategy.LastOrDefaultAsync(defaultValue, ct);

    public Task<TItem?> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.LastOrDefaultAsync(predicate, ct);

    public Task<TItem> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Strategy.LastOrDefaultAsync(predicate, defaultValue, ct);

    public Task<TItem> SingleAsync(CancellationToken ct = default)
        => Strategy.SingleAsync(ct);

    public Task<TItem> SingleAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.SingleAsync(predicate, ct);

    public Task<TItem?> SingleOrDefaultAsync(CancellationToken ct = default)
        => Strategy.SingleOrDefaultAsync(ct);

    public Task<TItem> SingleOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Strategy.SingleOrDefaultAsync(defaultValue, ct);

    public Task<TItem?> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.SingleOrDefaultAsync(predicate, ct);

    public Task<TItem> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Strategy.SingleOrDefaultAsync(predicate, defaultValue, ct);

    public Task<TItem> ElementAtAsync(int index, CancellationToken ct = default)
        => Strategy.ElementAtAsync(index, ct);

    public Task<TItem> ElementAtAsync(Index index, CancellationToken ct = default)
        => Strategy.ElementAtAsync(index, ct);

    public Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default)
        => Strategy.ElementAtOrDefault(index, ct);

    public Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => Strategy.ElementAtOrDefault(index, ct);

    public Task<bool> ContainsAsync(TItem item, CancellationToken ct = default)
        => Strategy.ContainsAsync(item, ct);

    public Task<bool> ContainsAsync(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.ContainsAsync(item, comparer, ct);

    public Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, CancellationToken ct = default)
        => Strategy.SequenceEqualAsync(source2, ct);

    public Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.SequenceEqualAsync(source2, comparer, ct);

    public Task<bool> AnyAsync(CancellationToken ct = default)
        => Strategy.AnyAsync(ct);

    public Task<bool> AnyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.AnyAsync(predicate, ct);

    public Task<bool> AllAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.AllAsync(predicate, ct);

    public Task<int> CountAsync(CancellationToken ct = default)
        => Strategy.CountAsync(ct);

    public Task<int> CountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.CountAsync(predicate, ct);

    public Task<long> LongCountAsync(CancellationToken ct = default)
        => Strategy.LongCountAsync(ct);

    public Task<long> LongCountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.LongCountAsync(predicate, ct);

    public Task<TItem?> MinAsync(CancellationToken ct = default)
        => Strategy.MinAsync(ct);

    public Task<TResult?> MinAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => Strategy.MinAsync(selector, ct);

    public Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Strategy.MinByAsync(keySelector, ct);

    public Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.MinByAsync(keySelector, comparer, ct);

    public Task<TItem?> MaxAsync(CancellationToken ct = default)
        => Strategy.MaxAsync(ct);

    public Task<TResult?> MaxAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        => Strategy.MaxAsync(selector, ct);

    public Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Strategy.MaxByAsync(keySelector, ct);

    public Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Strategy.MaxByAsync(keySelector, comparer, ct);

    public Task<int> SumAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<int?> SumAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<long> SumAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<long?> SumAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<float> SumAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<float?> SumAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<double> SumAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<double?> SumAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<decimal> SumAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<decimal?> SumAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Strategy.SumAsync(selector, ct);

    public Task<double> AverageAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<double?> AverageAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<float> AverageAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<float?> AverageAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<double> AverageAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<double?> AverageAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<double> AverageAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<double?> AverageAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<decimal> AverageAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<decimal?> AverageAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Strategy.AverageAsync(selector, ct);

    public Task<TItem> AggregateAsync(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => Strategy.AggregateAsync(func, ct);

    public Task<TAccumulate> AggregateAsync<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => Strategy.AggregateAsync(seed, func, ct);

    public Task<TResult> AggregateAsync<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => Strategy.AggregateAsync(seed,
                                                                                                                                                                                                                                           func,
                                                                                                                                                                                                                                           selector,
                                                                                                                                                                                                                                           ct);

    public Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Strategy.AddAsync(newItem, ct);

    public Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Strategy.UpdateAsync(predicate, updatedItem, ct);

    public Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Strategy.RemoveAsync(predicate, ct);
}
