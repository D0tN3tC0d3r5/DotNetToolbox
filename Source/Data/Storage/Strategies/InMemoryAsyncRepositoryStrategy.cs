namespace DotNetToolbox.Data.Strategies;
public sealed class InMemoryAsyncRepositoryStrategy<TRepository, TItem>
    : AsyncRepositoryStrategy<TItem>
    where TRepository : IAsyncOrderedRepository<TItem>{

    public InMemoryAsyncRepositoryStrategy() {
    }

    public InMemoryAsyncRepositoryStrategy(IEnumerable<TItem> data)
        : base(data) {
    }

    public override Task SeedAsync(IEnumerable<TItem> seed) {
        OriginalData = seed;
        Query = OriginalData.AsQueryable();
        return Task.CompletedTask;
    }

    private static IAsyncRepository<TResult> ApplyAndCreate<TResult>(Func<IQueryable<TResult>> updateSource){
        var result = updateSource();
        return RepositoryFactory.CreateAsyncRepository<TRepository, TResult>(result);
    }
    private static IAsyncOrderedRepository<TResult> ApplyAndCreateOrdered<TResult>(Func<IQueryable<TResult>> updateSource){
        var result = updateSource();
        return RepositoryFactory.CreateAsyncOrderedRepository<TRepository, TResult>(result);
    }

    public override IAsyncRepository<TResult> OfType<TResult>()
        => ApplyAndCreate(Query.OfType<TResult>);
    public override IAsyncRepository<TResult> Cast<TResult>()
        => ApplyAndCreate(Query.Cast<TResult>);
    public override IAsyncRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => Query.Where(predicate));
    public override IAsyncRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => Query.Where(predicate));
    public override IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        => ApplyAndCreate(() => Query.Select(selector));
    public override IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        => ApplyAndCreate(() => Query.Select(selector));
    public override IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        => ApplyAndCreate(() => Query.SelectMany(selector));
    public override IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        => ApplyAndCreate(() => Query.SelectMany(selector));
    public override IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.SelectMany(collectionSelector, resultSelector));
    public override IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.SelectMany(collectionSelector, resultSelector));
    public override IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.Join(inner, outerKeySelector, innerKeySelector, resultSelector));
    public override IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));
    public override IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
    public override IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));
    public override IAsyncOrderedRepository<TItem> Order()
        => ApplyAndCreateOrdered(Query.Order);
    public override IAsyncOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => Query.Order(comparer));
    public override IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => Query.OrderBy(keySelector));
    public override IAsyncOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => Query.OrderBy(keySelector, comparer));
    public override IAsyncOrderedRepository<TItem> OrderDescending()
        => ApplyAndCreateOrdered(Query.OrderDescending);
    public override IAsyncOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => Query.OrderDescending(comparer));
    public override IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => Query.OrderByDescending(keySelector));
    public override IAsyncOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => Query.OrderByDescending(keySelector, comparer));
    public override IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector));
    public override IAsyncOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector, comparer));
    public override IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector));
    public override IAsyncOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector, comparer));
    public override IAsyncRepository<TItem> Take(int count)
        => ApplyAndCreate(() => Query.Take(count));
    public override IAsyncRepository<TItem> Take(Range range)
        => ApplyAndCreate(() => Query.Take(range));
    public override IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => Query.TakeWhile(predicate));
    public override IAsyncRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => Query.TakeWhile(predicate));
    public override IAsyncRepository<TItem> TakeLast(int count)
        => ApplyAndCreate(() => Query.TakeLast(count));
    public override IAsyncRepository<TItem> Skip(int count)
        => ApplyAndCreate(() => Query.Skip(count));
    public override IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => Query.SkipWhile(predicate));
    public override IAsyncRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => Query.SkipWhile(predicate));
    public override IAsyncRepository<TItem> SkipLast(int count)
        => ApplyAndCreate(() => Query.SkipLast(count));
    public override IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector));
    public override IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector));
    public override IAsyncRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, comparer));
    public override IAsyncRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector, comparer));
    public override IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector, resultSelector));
    public override IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, resultSelector));
    public override IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, resultSelector, comparer));
    public override IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector, resultSelector, comparer));
    public override IAsyncRepository<TItem> Distinct()
        => ApplyAndCreate(Query.Distinct);
    public override IAsyncRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Distinct(comparer));
    public override IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.DistinctBy(keySelector));
    public override IAsyncRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.DistinctBy(keySelector, comparer));
    public override IAsyncRepository<TItem[]> Chunk(int size)
        => ApplyAndCreate(() => Query.Chunk(size));
    public override IAsyncRepository<TItem> Concat(IEnumerable<TItem> source)
        => ApplyAndCreate(() => Query.Concat(source));
    public override IAsyncRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.Zip(source2, resultSelector));
    public override IAsyncRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source) {
        Expression<Func<(TItem First, TSecond Second), IPack<TItem, TSecond>>> convert = z => new Pack<TItem, TSecond>(z.First, z.Second);
        return ApplyAndCreate(() => Query.Zip(source).Select(convert));
    }
    public override IAsyncRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3) {
        Expression<Func<(TItem First, TSecond Second, TThird Third), IPack<TItem, TSecond, TThird>>> convert = z => new Pack<TItem, TSecond, TThird>(z.First, z.Second, z.Third);
        return ApplyAndCreate(() => Query.Zip(source2, source3).Select(convert));
    }
    public override IAsyncRepository<TItem> Union(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => Query.Union(source2));
    public override IAsyncRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Union(source2, comparer));
    public override IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.UnionBy(source2, keySelector));
    public override IAsyncRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.UnionBy(source2, keySelector, comparer));
    public override IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => Query.Intersect(source2));
    public override IAsyncRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Intersect(source2, comparer));
    public override IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.IntersectBy(source2, keySelector));
    public override IAsyncRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.IntersectBy(source2, keySelector, comparer));
    public override IAsyncRepository<TItem> Except(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => Query.Except(source2));
    public override IAsyncRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Except(source2, comparer));
    public override IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.ExceptBy(source2, keySelector));
    public override IAsyncRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.ExceptBy(source2, keySelector, comparer));
    public override IAsyncRepository<TItem?> DefaultIfEmpty()
        => ApplyAndCreate(Query.DefaultIfEmpty);
    public override IAsyncRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => ApplyAndCreate(() => Query.DefaultIfEmpty(defaultValue));
    public override IAsyncRepository<TItem> Reverse()
        => ApplyAndCreate(Query.Reverse);
    public override IAsyncRepository<TItem> Append(TItem element)
        => ApplyAndCreate(() => Query.Append(element));
    public override IAsyncRepository<TItem> Prepend(TItem element)
        => ApplyAndCreate(() => Query.Prepend(element));
    public override Task<TItem> FirstAsync(CancellationToken ct = default)
        => Task.Run(() => Query.First(), ct);
    public override Task<TItem> FirstAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.First(predicate), ct);
    public override Task<TItem?> FirstOrDefaultAsync(CancellationToken ct = default)
        => Task.Run(() => Query.FirstOrDefault(), ct);
    public override Task<TItem> FirstOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => Query.FirstOrDefault() ?? defaultValue, ct);
    public override Task<TItem?> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.FirstOrDefault(predicate), ct);
    public override Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => Query.FirstOrDefault(predicate) ?? defaultValue, ct);
    public override Task<TItem> LastAsync(CancellationToken ct = default)
        => Task.Run(() => Query.Last(), ct);
    public override Task<TItem> LastAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.Last(predicate), ct);
    public override Task<TItem?> LastOrDefaultAsync(CancellationToken ct = default)
        => Task.Run(() => Query.LastOrDefault(), ct);
    public override Task<TItem> LastOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => Query.LastOrDefault() ?? defaultValue, ct);
    public override Task<TItem?> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.LastOrDefault(predicate), ct);
    public override Task<TItem> LastOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => Query.LastOrDefault(predicate) ?? defaultValue, ct);
    public override Task<TItem> SingleAsync(CancellationToken ct = default)
        => Task.Run(() => Query.Single(), ct);
    public override Task<TItem> SingleAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.Single(predicate), ct);
    public override Task<TItem?> SingleOrDefaultAsync(CancellationToken ct = default)
        => Task.Run(() => Query.SingleOrDefault(), ct);
    public override Task<TItem> SingleOrDefaultAsync(TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => Query.SingleOrDefault() ?? defaultValue, ct);
    public override Task<TItem?> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.SingleOrDefault(predicate), ct);
    public override Task<TItem> SingleOrDefaultAsync(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => Task.Run(() => Query.SingleOrDefault(predicate) ?? defaultValue, ct);
    public override Task<TItem> ElementAtAsync(int index, CancellationToken ct = default)
        => Task.Run(() => Query.ElementAt(index), ct);
    public override Task<TItem> ElementAtAsync(Index index, CancellationToken ct = default)
        => Task.Run(() => Query.ElementAt(index), ct);
    public override Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default)
        => Task.Run(() => Query.ElementAtOrDefault(index), ct);
    public override Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => Task.Run(() => Query.ElementAtOrDefault(index), ct);
    public override Task<bool> ContainsAsync(TItem item, CancellationToken ct = default)
        => Task.Run(() => Query.Contains(item), ct);
    public override Task<bool> ContainsAsync(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => Query.Contains(item, comparer), ct);
    public override Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, CancellationToken ct = default)
        => Task.Run(() => Query.SequenceEqual(source2), ct);
    public override Task<bool> SequenceEqualAsync(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => Query.SequenceEqual(source2, comparer), ct);
    public override Task<bool> AnyAsync(CancellationToken ct = default)
        => Task.Run(() => Query.Any(), ct);
    public override Task<bool> AnyAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.Any(predicate), ct);
    public override Task<bool> AllAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.All(predicate), ct);
    public override Task<int> CountAsync(CancellationToken ct = default)
        => Task.Run(() => Query.Count(), ct);
    public override Task<int> CountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.Count(predicate), ct);
    public override Task<long> LongCountAsync(CancellationToken ct = default)
        => Task.Run(() => Query.LongCount(), ct);
    public override Task<long> LongCountAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => Query.LongCount(predicate), ct);
    public override Task<TItem?> MinAsync(CancellationToken ct = default)
        => Task.Run(() => Query.Min(), ct);
    public override Task<TResult?> MinAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        where TResult : default
        => Task.Run(() => Query.Min(selector), ct);
    public override Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Task.Run(() => Query.MinBy(keySelector), ct);
    public override Task<TItem?> MinByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => Query.MinBy(keySelector, comparer), ct);
    public override Task<TItem?> MaxAsync(CancellationToken ct = default)
        => Task.Run(() => Query.Max(), ct);
    public override Task<TResult?> MaxAsync<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        where TResult : default
        => Task.Run(() => Query.Max(selector), ct);
    public override Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => Task.Run(() => Query.MaxBy(keySelector), ct);
    public override Task<TItem?> MaxByAsync<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => Task.Run(() => Query.MaxBy(keySelector, comparer), ct);
    public override Task<int> SumAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<int?> SumAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<long> SumAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<long?> SumAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<float> SumAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<float?> SumAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<double> SumAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<double?> SumAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<decimal> SumAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<decimal?> SumAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Sum(selector), ct);
    public override Task<double> AverageAsync(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<double?> AverageAsync(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<float> AverageAsync(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<float?> AverageAsync(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<double> AverageAsync(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<double?> AverageAsync(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<double> AverageAsync(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<double?> AverageAsync(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<decimal> AverageAsync(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<decimal?> AverageAsync(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Average(selector), ct);
    public override Task<TItem> AggregateAsync(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => Task.Run(() => Query.Aggregate(func), ct);
    public override Task<TAccumulate> AggregateAsync<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => Task.Run(() => Query.Aggregate(seed, func), ct);
    public override Task<TResult> AggregateAsync<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => Task.Run(() => Query.Aggregate(seed, func, selector), ct);
    public override Task<TItem[]> ToArrayAsync(CancellationToken ct = default)
        => Task.Run(() => Query.ToArray(), ct);
    public override Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.Run(() => Query.ToArray(mapping), ct);
    public override Task<List<TItem>> ToListAsync(CancellationToken ct = default)
        => Task.Run(() => Query.ToList(), ct);
    public override Task<List<TResult>> ToListAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.Run(() => Query.ToList(mapping), ct);
    public override Task<HashSet<TItem>> ToHashSetAsync(CancellationToken ct = default)
        => Task.Run(() => Query.ToHashSet(), ct);
    public override Task<HashSet<TResult>> ToHashSetAsync<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => Task.Run(() => Query.ToHashSet(mapping), ct);
    public override Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        => Task.Run(() => Query.ToDictionary(selectKey, selectValue, comparer), ct);
    public override Task AddAsync(TItem newItem, CancellationToken ct = default)
        => Task.Run(() => {
            UpdatableData.Add(newItem);
            Query = UpdatableData.AsQueryable();
        }, ct);

    public override async Task UpdateAsync(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        await RemoveAsync(predicate, ct);
        await AddAsync(updatedItem, ct);
    }

    public override Task RemoveAsync(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => Task.Run(() => {
            var itemToRemove = Query.FirstOrDefault(predicate);
            if (itemToRemove is null)
                return;
            UpdatableData.Add(itemToRemove);
            Query = UpdatableData.AsQueryable();
        }, ct);
}
