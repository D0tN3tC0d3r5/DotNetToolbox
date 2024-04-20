namespace DotNetToolbox.Data.Repositories;

internal sealed class InMemoryAsyncRepositoryStrategy<TRepository, TItem>
    : AsyncRepositoryStrategy<TItem>
    where TRepository : IAsyncOrderedRepository<TItem>
    where TItem : class {
    private readonly IList<TItem> _data;
    private readonly IQueryable<TItem> _query;

    public InMemoryAsyncRepositoryStrategy(IEnumerable<TItem> data) {
        _data = data as IList<TItem> ?? data.ToList();
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
        where TResult : class
        => ApplyAndCreate(() => _query.Select(selector));

    public override IAsyncRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.Select(selector));

    public override IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(selector));

    public override IAsyncRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(selector));

    public override IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(collectionSelector, resultSelector));

    public override IAsyncRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(collectionSelector, resultSelector));

    public override IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IAsyncRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IAsyncRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
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
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, resultSelector));

    public override IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, resultSelector));

    public override IAsyncRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, resultSelector, comparer));

    public override IAsyncRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
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
        where TResult : class
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

    public override Task<TItem> First(CancellationToken ct = default)
        => _query.First();

    public override Task<TItem> First(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.First(predicate);

    public override Task<TItem?> FirstOrDefault(CancellationToken ct = default)
        => _query.FirstOrDefault();

    public override Task<TItem> FirstOrDefault(TItem defaultValue, CancellationToken ct = default)
        => _query.FirstOrDefault() ?? defaultValue;

    public override Task<TItem?> FirstOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.FirstOrDefault(predicate);

    public override Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => _query.FirstOrDefault(predicate) ?? defaultValue;

    public override Task<TItem> Last(CancellationToken ct = default)
        => _query.Last();

    public override Task<TItem> Last(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.Last(predicate);

    public override Task<TItem?> LastOrDefault(CancellationToken ct = default)
        => _query.LastOrDefault();

    public override Task<TItem> LastOrDefault(TItem defaultValue, CancellationToken ct = default)
        => _query.LastOrDefault() ?? defaultValue;

    public override Task<TItem?> LastOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.LastOrDefault(predicate);

    public override Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => _query.LastOrDefault(predicate) ?? defaultValue;

    public override Task<TItem> Single(CancellationToken ct = default)
        => _query.Single();

    public override Task<TItem> Single(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.Single(predicate);

    public override Task<TItem?> SingleOrDefault(CancellationToken ct = default)
        => _query.SingleOrDefault();

    public override Task<TItem> SingleOrDefault(TItem defaultValue, CancellationToken ct = default)
        => _query.SingleOrDefault() ?? defaultValue;

    public override Task<TItem?> SingleOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.SingleOrDefault(predicate);

    public override Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default)
        => _query.SingleOrDefault(predicate) ?? defaultValue;

    public override Task<TItem> ElementAt(int index, CancellationToken ct = default)
        => _query.ElementAt(index);

    public override Task<TItem> ElementAt(Index index, CancellationToken ct = default)
        => _query.ElementAt(index);

    public override Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default)
        => _query.ElementAtOrDefault(index);

    public override Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default)
        => _query.ElementAtOrDefault(index);

    public override Task<bool> Contains(TItem item, CancellationToken ct = default)
        => _query.Contains(item);

    public override Task<bool> Contains(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => _query.Contains(item, comparer);

    public override Task<bool> SequenceEqual(IEnumerable<TItem> source2, CancellationToken ct = default)
        => _query.SequenceEqual(source2);

    public override Task<bool> SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default)
        => _query.SequenceEqual(source2, comparer);

    public override Task<bool> Any(CancellationToken ct = default)
        => _query.Any();

    public override Task<bool> Any(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.Any(predicate);

    public override Task<bool> All(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.All(predicate);

    public override int Count(CancellationToken ct = default)
        => _query.Count();

    public override int Count(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.Count(predicate);

    public override long LongCount(CancellationToken ct = default)
        => _query.LongCount();

    public override long LongCount(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => _query.LongCount(predicate);

    public override Task<TItem?> Min(CancellationToken ct = default)
        => _query.Min();

    public override TResult? Min<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        where TResult : default
        => _query.Min(selector);

    public override Task<TItem?> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => _query.MinBy(keySelector);

    public override Task<TItem?> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => _query.MinBy(keySelector, comparer);

    public override Task<TItem?> Max(CancellationToken ct = default)
        => _query.Max();

    public override TResult? Max<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default)
        where TResult : default
        => _query.Max(selector);

    public override Task<TItem?> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default)
        => _query.MaxBy(keySelector);

    public override Task<TItem?> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default)
        => _query.MaxBy(keySelector, comparer);

    public override int Sum(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<int?> Sum(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override long Sum(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<long?> Sum(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<float> Sum(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<float?> Sum(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<double> Sum(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<double?> Sum(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<decimal> Sum(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<decimal?> Sum(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => _query.Sum(selector);

    public override Task<double> Average(Expression<Func<TItem, int>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<double>? Average(Expression<Func<TItem, int?>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<float> Average(Expression<Func<TItem, float>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<float?> Average(Expression<Func<TItem, float?>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<double> Average(Expression<Func<TItem, long>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<double>? Average(Expression<Func<TItem, long?>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<double> Average(Expression<Func<TItem, double>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<double>? Average(Expression<Func<TItem, double?>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<decimal> Average(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<decimal?> Average(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default)
        => _query.Average(selector);

    public override Task<TItem> Aggregate(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default)
        => _query.Aggregate(func);

    public override Task<TAccumulate> Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default)
        => _query.Aggregate(seed, func);

    public override Task<TResult> Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default)
        => _query.Aggregate(seed, func, selector);

    public override Task<IReadOnlyList<TItem>> ToArray(CancellationToken ct = default)
        => [.. _query];

    public override Task<IReadOnlyList<TResult>> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => _query.ToArray(mapping);

    public override Task<IList<TItem>> ToList(CancellationToken ct = default)
        => [.. _query];

    public override Task<IList<TResult>> ToList<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => _query.ToList(mapping);

    public override Task<ISet<TItem>> ToHashSet(CancellationToken ct = default)
        => _query.ToHashSet();

    public override Task<ISet<TResult>> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => _query.ToHashSet(mapping);

    public override Task<TResultRepository> ToRepository<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResultRepository : class
        where TResult : class
        => InstanceFactory.Create<TResultRepository>(_query.ToList(mapping));

    public override Task<IRepository<TResult>> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class
        => new Repository<TResult>(_query.ToList(mapping));

    public override Task<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        => _query.ToDictionary(selectKey, selectValue, comparer);

    public override Task Add(TItem newItem, CancellationToken ct = default) => _data.Add(newItem);

    public override Task Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem, CancellationToken ct = default) {
        var itemToUpdate = _query.FirstOrDefault(predicate);
        if (itemToUpdate is null) return;
        var index = _data.IndexOf(itemToUpdate);
        _data[index] = updatedItem;
    }

    public override Task Remove(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        var itemToRemove = _query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return;
        _data.Remove(itemToRemove);
    }
}
