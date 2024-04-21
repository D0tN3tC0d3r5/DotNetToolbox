namespace DotNetToolbox.Data.Strategies;

public sealed class InMemoryAsyncRepositoryStrategy<TRepository, TItem>
    : AsyncRepositoryStrategy<TItem>
    where TRepository : IAsyncOrderedRepository<TItem>
    where TItem : class {
    private readonly IList<TItem> _data;
    private readonly IQueryable<TItem> _query;

    public InMemoryAsyncRepositoryStrategy(IEnumerable<TItem> data) {
        _data = data is IList<TItem> { IsReadOnly: false } list ? list : data.ToList();
        _query = _data.AsQueryable();
    }

    private static IAsyncRepository<TResult> ApplyAndCreate<TResult>(Func<IQueryable<TResult>> updateSource)
        where TResult : class {
        var result = updateSource();
        return RepositoryFactory.CreateAsyncRepository<TRepository, TResult>(result);
    }

    private static IAsyncOrderedRepository<TResult> ApplyAndCreateOrdered<TResult>(Func<IQueryable<TResult>> updateSource)
        where TResult : class {
        var result = updateSource();
        return RepositoryFactory.CreateAsyncOrderedRepository<TRepository, TResult>(result);
    }

    public override IAsyncRepository<TResult> OfType<TResult>()
        where TResult : class
        => ApplyAndCreate(_query.OfType<TResult>);

    public override IAsyncRepository<TResult> Cast<TResult>()
        where TResult : class
        => ApplyAndCreate(_query.Cast<TResult>);

    public override IAsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _query.Where(predicate));

    public override IAsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _query.Where(predicate));

    public override IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        => ApplyAndCreate(() => _query.Select(selector));

    public override IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        => ApplyAndCreate(() => _query.Select(selector));

    public override IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        => ApplyAndCreate(() => _query.SelectMany(selector));

    public override IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        => ApplyAndCreate(() => _query.SelectMany(selector));

    public override IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.SelectMany(collectionSelector, resultSelector));

    public override IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.SelectMany(collectionSelector, resultSelector));

    public override IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IAsyncOrderedRepository<TItem> Order()
        => ApplyAndCreateOrdered(_query.Order);

    public override IAsyncOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => _query.Order(comparer));

    public override IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => _query.OrderBy(keySelector));

    public override IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => _query.OrderBy(keySelector, comparer));

    public override IAsyncOrderedRepository<TItem> OrderDescending()
        => ApplyAndCreateOrdered(_query.OrderDescending);

    public override IAsyncOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => _query.OrderDescending(comparer));

    public override IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => _query.OrderByDescending(keySelector));

    public override IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => _query.OrderByDescending(keySelector, comparer));

    public override IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector));

    public override IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector, comparer));

    public override IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector));

    public override IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector, comparer));

    public override IAsyncRepository<TItem> Take(int count)
        => ApplyAndCreate(() => _query.Take(count));

    public override IAsyncRepository<TItem> Take(Range range)
        => ApplyAndCreate(() => _query.Take(range));

    public override IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _query.TakeWhile(predicate));

    public override IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _query.TakeWhile(predicate));

    public override IAsyncRepository<TItem> TakeLast(int count)
        => ApplyAndCreate(() => _query.TakeLast(count));

    public override IAsyncRepository<TItem> Skip(int count)
        => ApplyAndCreate(() => _query.Skip(count));

    public override IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _query.SkipWhile(predicate));

    public override IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _query.SkipWhile(predicate));

    public override IAsyncRepository<TItem> SkipLast(int count)
        => ApplyAndCreate(() => _query.SkipLast(count));

    public override IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.GroupBy(keySelector));

    public override IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector));

    public override IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, comparer));

    public override IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, comparer));

    public override IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, resultSelector));

    public override IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, resultSelector));

    public override IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, resultSelector, comparer));

    public override IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, resultSelector, comparer));

    public override IAsyncRepository<TItem> Distinct()
        => ApplyAndCreate(_query.Distinct);

    public override IAsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Distinct(comparer));

    public override IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.DistinctBy(keySelector));

    public override IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.DistinctBy(keySelector, comparer));

    public override IAsyncRepository<TItem[]> Chunk(int size)
        => ApplyAndCreate(() => _query.Chunk(size));

    public override IAsyncRepository<TItem> Concat(IEnumerable<TItem> source)
        => ApplyAndCreate(() => _query.Concat(source));

    public override IAsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        => ApplyAndCreate(() => _query.Zip(source2, resultSelector));

    public override IAsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source) {
        Expression<Func<(TItem First, TSecond Second), IPack<TItem, TSecond>>> convert = z => new Pack<TItem, TSecond>(z.First, z.Second);
        return ApplyAndCreate(() => _query.Zip(source).Select(convert));
    }

    public override IAsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3) {
        Expression<Func<(TItem First, TSecond Second, TThird Third), IPack<TItem, TSecond, TThird>>> convert = z => new Pack<TItem, TSecond, TThird>(z.First, z.Second, z.Third);
        return ApplyAndCreate(() => _query.Zip(source2, source3).Select(convert));
    }

    public override IAsyncRepository<TItem> Union(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _query.Union(source2));

    public override IAsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Union(source2, comparer));

    public override IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.UnionBy(source2, keySelector));

    public override IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.UnionBy(source2, keySelector, comparer));

    public override IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _query.Intersect(source2));

    public override IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Intersect(source2, comparer));

    public override IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.IntersectBy(source2, keySelector));

    public override IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.IntersectBy(source2, keySelector, comparer));

    public override IAsyncRepository<TItem> Except(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _query.Except(source2));

    public override IAsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Except(source2, comparer));

    public override IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.ExceptBy(source2, keySelector));

    public override IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.ExceptBy(source2, keySelector, comparer));

#pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    public override IAsyncRepository<TItem?> DefaultIfEmpty()
        => ApplyAndCreate(_query.DefaultIfEmpty);
#pragma warning restore CS8634

    public override IAsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => ApplyAndCreate(() => _query.DefaultIfEmpty(defaultValue));

    public override IAsyncRepository<TItem> Reverse()
        => ApplyAndCreate(_query.Reverse);

    public override IAsyncRepository<TItem> Append(TItem element)
        => ApplyAndCreate(() => _query.Append(element));

    public override IAsyncRepository<TItem> Prepend(TItem element)
        => ApplyAndCreate(() => _query.Prepend(element));

    public override Task<TItem> FirstAsync(CancellationToken ct = default)
        => Task.Run(() => _query.First(), ct);

    public override Task<TItem> FirstAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.First(predicate), ct);

    public override Task<TItem?> FirstOrDefaultAsync(CancellationToken ct = default)
        => Task.Run(() => _query.FirstOrDefault(), ct);

    public override Task<TItem> FirstOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => _query.FirstOrDefault() ?? defaultValue, ct);

    public override Task<TItem?> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.FirstOrDefault(predicate), ct);

    public override Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => _query.FirstOrDefault(predicate) ?? defaultValue, ct);

    public override Task<TItem> LastAsync(CancellationToken ct = default)
        => Task.Run(() => _query.Last(), ct);

    public override Task<TItem> LastAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.Last(predicate), ct);

    public override Task<TItem?> LastOrDefaultAsync(CancellationToken ct = default)
        => Task.Run(() => _query.LastOrDefault(), ct);

    public override Task<TItem> LastOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => _query.LastOrDefault() ?? defaultValue, ct);

    public override Task<TItem?> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.LastOrDefault(predicate), ct);

    public override Task<TItem> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => _query.LastOrDefault(predicate) ?? defaultValue, ct);

    public override Task<TItem> SingleAsync(CancellationToken ct = default)
        => Task.Run(() => _query.Single(), ct);

    public override Task<TItem> SingleAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.Single(predicate), ct);

    public override Task<TItem?> SingleOrDefaultAsync(CancellationToken ct = default)
        => Task.Run(() => _query.SingleOrDefault(), ct);

    public override Task<TItem> SingleOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => _query.SingleOrDefault() ?? defaultValue, ct);

    public override Task<TItem?> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.SingleOrDefault(predicate), ct);

    public override Task<TItem> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => _query.SingleOrDefault(predicate) ?? defaultValue, ct);

    public override Task<TItem> ElementAtAsync(int index, CancellationToken ct = default)
        => Task.Run(() => _query.ElementAt(index), ct);

    public override Task<TItem> ElementAtAsync(Index index, CancellationToken ct = default)
        => Task.Run(() => _query.ElementAt(index), ct);

    public override Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default)
        => Task.Run(() => _query.ElementAtOrDefault(index), ct);

    public override Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => Task.Run(() => _query.ElementAtOrDefault(index), ct);

    public override Task<bool> ContainsAsync(TItem item, CancellationToken ct = default)
        => Task.Run(() => _query.Contains(item), ct);

    public override Task<bool> ContainsAsync(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => _query.Contains(item, comparer), ct);

    public override Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, CancellationToken ct = default)
        => Task.Run(() => _query.SequenceEqual(source2), ct);

    public override Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => _query.SequenceEqual(source2, comparer), ct);

    public override Task<bool> AnyAsync(CancellationToken ct = default)
        => Task.Run(() => _query.Any(), ct);

    public override Task<bool> AnyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.Any(predicate), ct);

    public override Task<bool> AllAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.All(predicate), ct);

    public override Task<int> CountAsync(CancellationToken ct = default)
        => Task.Run(() => _query.Count(), ct);

    public override Task<int> CountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.Count(predicate), ct);

    public override Task<long> LongCountAsync(CancellationToken ct = default)
        => Task.Run(() => _query.LongCount(), ct);

    public override Task<long> LongCountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => _query.LongCount(predicate), ct);

    public override Task<TItem?> MinAsync(CancellationToken ct = default)
        => Task.Run(() => _query.Min(), ct);

    public override Task<TResult?> MinAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        where TResult : default
        => Task.Run(() => _query.Min(selector), ct);

    public override Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Task.Run(() => _query.MinBy(keySelector), ct);

    public override Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => _query.MinBy(keySelector, comparer), ct);

    public override Task<TItem?> MaxAsync(CancellationToken ct = default)
        => Task.Run(() => _query.Max(), ct);

    public override Task<TResult?> MaxAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        where TResult : default
        => Task.Run(() => _query.Max(selector), ct);

    public override Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Task.Run(() => _query.MaxBy(keySelector), ct);

    public override Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => _query.MaxBy(keySelector, comparer), ct);

    public override Task<int> SumAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<int?> SumAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<long> SumAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<long?> SumAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<float> SumAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<float?> SumAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<double> SumAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<double?> SumAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<decimal> SumAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<decimal?> SumAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Sum(selector), ct);

    public override Task<double> AverageAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<double?> AverageAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<float> AverageAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<float?> AverageAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<double> AverageAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<double?> AverageAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<double> AverageAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<double?> AverageAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<decimal> AverageAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<decimal?> AverageAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Average(selector), ct);

    public override Task<TItem> AggregateAsync(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => Task.Run(() => _query.Aggregate(func), ct);

    public override Task<TAccumulate> AggregateAsync<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => Task.Run(() => _query.Aggregate(seed, func), ct);

    public override Task<TResult> AggregateAsync<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => Task.Run(() => _query.Aggregate(seed, func, selector), ct);

    public override Task<IReadOnlyList<TItem>> ToArrayAsync(CancellationToken ct = default)
        => Task.Run<IReadOnlyList<TItem>>(() => _query.ToArray(), ct);

    public override Task<IReadOnlyList<TResult>> ToArrayAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.Run<IReadOnlyList<TResult>>(() => _query.ToArray(mapping), ct);

    public override Task<IList<TItem>> ToListAsync(CancellationToken ct = default)
        => Task.Run<IList<TItem>>(() => _query.ToArray(), ct);

    public override Task<IList<TResult>> ToListAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.Run<IList<TResult>>(() => _query.ToList(mapping), ct);

    public override Task<ISet<TItem>> ToHashSetAsync(CancellationToken ct = default)
        => Task.Run<ISet<TItem>>(() => _query.ToHashSet(), ct);

    public override Task<ISet<TResult>> ToHashSetAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.Run<ISet<TResult>>(() => _query.ToHashSet(mapping), ct);

    public override Task<TResultRepository> ToRepositoryAsync<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResultRepository : class
        => Task.FromResult(InstanceFactory.Create<TResultRepository>(_query.ToList(mapping)));

    public override Task<IAsyncRepository<TResult>> ToRepositoryAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.FromResult<IAsyncRepository<TResult>>(new InMemoryAsyncRepository<TResult>(_query.ToList(mapping)));

    public override Task<IDictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        => Task.Run<IDictionary<TKey, TValue>>(() => _query.ToDictionary(selectKey, selectValue, comparer), ct);

    public override Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Task.Run(() => _data.Add(newItem), ct);

    public override Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default)
        => Task.Run(() => {
            var itemToUpdate = _query.FirstOrDefault(predicate);
            if (itemToUpdate is null)
                return;
            var index = _data.IndexOf(itemToUpdate);
            _data[index] = updatedItem;
        },
                    ct);

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => {
            var itemToRemove = _query.FirstOrDefault(predicate);
            if (itemToRemove is null)
                return;
            _data.Remove(itemToRemove);
        },
                    ct);
}
