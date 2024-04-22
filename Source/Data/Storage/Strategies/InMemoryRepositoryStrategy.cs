namespace DotNetToolbox.Data.Strategies;

public class InMemoryRepositoryStrategy<TRepository, TItem>
    : RepositoryStrategy<TItem>
    where TRepository : IOrderedRepository<TItem>
    where TItem : class {
    private readonly ISet<TItem> _data;
    private readonly IQueryable<TItem> _query;

    public InMemoryRepositoryStrategy(IEnumerable<TItem> data) {
        _data = data is ISet<TItem> { IsReadOnly: false } list ? list : data.ToHashSet();
        _query = _data.AsQueryable();
    }

    private static IRepository<TResult> ApplyAndCreate<TResult>(Func<IQueryable<TResult>> updateSource)
        where TResult : class {
        var result = updateSource();
        return RepositoryFactory.CreateRepository<TRepository, TResult>(result);
    }

    private static IOrderedRepository<TResult> ApplyAndCreateOrdered<TResult>(Func<IQueryable<TResult>> updateSource)
        where TResult : class {
        var result = updateSource();
        return RepositoryFactory.CreateOrderedRepository<TRepository, TResult>(result);
    }

    public override IRepository<TResult> OfType<TResult>()
        where TResult : class
        => ApplyAndCreate(_query.OfType<TResult>);

    public override IRepository<TResult> Cast<TResult>()
        where TResult : class {
        if (!typeof(TItem).IsAssignableTo(typeof(TResult)))
            throw new InvalidCastException("The cast is invalid.");
        return ApplyAndCreate(_query.Cast<TResult>);
    }

    public override IRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _query.Where(predicate));

    public override IRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _query.Where(predicate));

    public override IRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.Select(selector));

    public override IRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.Select(selector));

    public override IRepository<TResult> SelectMony<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(selector));

    public override IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(selector));

    public override IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(collectionSelector, resultSelector));

    public override IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.SelectMany(collectionSelector, resultSelector));

    public override IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _query.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));

    public override IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));

    public override IOrderedRepository<TItem> Order()
        => ApplyAndCreateOrdered(_query.Order);

    public override IOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => _query.Order(comparer));

    public override IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => _query.OrderBy(keySelector));

    public override IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => _query.OrderBy(keySelector, comparer));

    public override IOrderedRepository<TItem> OrderDescending()
        => ApplyAndCreateOrdered(_query.OrderDescending);

    public override IOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => _query.OrderDescending(comparer));

    public override IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => _query.OrderByDescending(keySelector));

    public override IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => _query.OrderByDescending(keySelector, comparer));

    public override IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector));

    public override IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector, comparer));

    public override IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector));

    public override IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => _query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector, comparer));

    public override IRepository<TItem> Take(int count)
        => ApplyAndCreate(() => _query.Take(count));

    public override IRepository<TItem> Take(Range range)
        => ApplyAndCreate(() => _query.Take(range));

    public override IRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _query.TakeWhile(predicate));

    public override IRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _query.TakeWhile(predicate));

    public override IRepository<TItem> TakeLast(int count)
        => ApplyAndCreate(() => _query.TakeLast(count));

    public override IRepository<TItem> Skip(int count)
        => ApplyAndCreate(() => _query.Skip(count));

    public override IRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => _query.SkipWhile(predicate));

    public override IRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => _query.SkipWhile(predicate));

    public override IRepository<TItem> SkipLast(int count)
        => ApplyAndCreate(() => _query.SkipLast(count));

    public override IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.GroupBy(keySelector));

    public override IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector));

    public override IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, comparer));

    public override IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, comparer));

    public override IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, resultSelector));

    public override IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, resultSelector));

    public override IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, resultSelector, comparer));

    public override IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => ApplyAndCreate(() => _query.GroupBy(keySelector, elementSelector, resultSelector, comparer));

    public override IRepository<TItem> Distinct()
        => ApplyAndCreate(_query.Distinct);

    public override IRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Distinct(comparer));

    public override IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.DistinctBy(keySelector));

    public override IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.DistinctBy(keySelector, comparer));

    public override IRepository<TItem[]> Chunk(int size)
        => ApplyAndCreate(() => _query.Chunk(size));

    public override IRepository<TItem> Concat(IEnumerable<TItem> source)
        => ApplyAndCreate(() => _query.Concat(source));

    public override IRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => ApplyAndCreate(() => _query.Zip(source2, resultSelector));

    public override IRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source) {
        Expression<Func<(TItem First, TSecond Second), IPack<TItem, TSecond>>> convert = z => new Pack<TItem, TSecond>(z.First, z.Second);
        return ApplyAndCreate(() => _query.Zip(source).Select(convert));
    }

    public override IRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3) {
        Expression<Func<(TItem First, TSecond Second, TThird Third), IPack<TItem, TSecond, TThird>>> convert = z => new Pack<TItem, TSecond, TThird>(z.First, z.Second, z.Third);
        return ApplyAndCreate(() => _query.Zip(source2, source3).Select(convert));
    }

    public override IRepository<TItem> Union(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _query.Union(source2));

    public override IRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Union(source2, comparer));

    public override IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.UnionBy(source2, keySelector));

    public override IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.UnionBy(source2, keySelector, comparer));

    public override IRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _query.Intersect(source2));

    public override IRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Intersect(source2, comparer));

    public override IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.IntersectBy(source2, keySelector));

    public override IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.IntersectBy(source2, keySelector, comparer));

    public override IRepository<TItem> Except(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => _query.Except(source2));

    public override IRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => _query.Except(source2, comparer));

    public override IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => _query.ExceptBy(source2, keySelector));

    public override IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => _query.ExceptBy(source2, keySelector, comparer));

#pragma warning disable CS8634 // This warning is wrong. TItem has the class constraint.
    public override IRepository<TItem?> DefaultIfEmpty()
        => ApplyAndCreate(_query.DefaultIfEmpty);
#pragma warning restore CS8634

    public override IRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => ApplyAndCreate(() => _query.DefaultIfEmpty(defaultValue));

    public override IRepository<TItem> Reverse()
        => ApplyAndCreate(_query.Reverse);

    public override IRepository<TItem> Append(TItem element)
        => ApplyAndCreate(() => _query.Append(element));

    public override IRepository<TItem> Prepend(TItem element)
        => ApplyAndCreate(() => _query.Prepend(element));

    public override TItem First()
        => _query.First();

    public override TItem First(Expression<Func<TItem, bool>> predicate)
        => _query.First(predicate);

    public override TItem? FirstOrDefault()
        => _query.FirstOrDefault();

    public override TItem FirstOrDefault(TItem defaultValue)
        => _query.FirstOrDefault() ?? defaultValue;

    public override TItem? FirstOrDefault(Expression<Func<TItem, bool>> predicate)
        => _query.FirstOrDefault(predicate);

    public override TItem Last()
        => _query.Last();

    public override TItem Last(Expression<Func<TItem, bool>> predicate)
        => _query.Last(predicate);

    public override TItem? LastOrDefault()
        => _query.LastOrDefault();

    public override TItem LastOrDefault(TItem defaultValue)
        => _query.LastOrDefault() ?? defaultValue;

    public override TItem? LastOrDefault(Expression<Func<TItem, bool>> predicate)
        => _query.LastOrDefault(predicate);

    public override TItem LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => _query.LastOrDefault(predicate) ?? defaultValue;

    public override TItem Single()
        => _query.Single();

    public override TItem Single(Expression<Func<TItem, bool>> predicate)
        => _query.Single(predicate);

    public override TItem? SingleOrDefault()
        => _query.SingleOrDefault();

    public override TItem SingleOrDefault(TItem defaultValue)
        => _query.SingleOrDefault() ?? defaultValue;

    public override TItem? SingleOrDefault(Expression<Func<TItem, bool>> predicate)
        => _query.SingleOrDefault(predicate);

    public override TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => _query.SingleOrDefault(predicate) ?? defaultValue;

    public override TItem ElementAt(int index)
        => _query.ElementAt(index);

    public override TItem ElementAt(Index index)
        => _query.ElementAt(index);

    public override TItem? ElementAtOrDefault(int index)
        => _query.ElementAtOrDefault(index);

    public override TItem? ElementAtOrDefault(Index index)
        => _query.ElementAtOrDefault(index);

    public override bool Contains(TItem item)
        => _query.Contains(item);

    public override bool Contains(TItem item, IEqualityComparer<TItem>? comparer)
        => _query.Contains(item, comparer);

    public override bool SequenceEqual(IEnumerable<TItem> source2)
        => _query.SequenceEqual(source2);

    public override bool SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => _query.SequenceEqual(source2, comparer);

    public override bool Any()
        => _query.Any();

    public override bool Any(Expression<Func<TItem, bool>> predicate)
        => _query.Any(predicate);

    public override bool All(Expression<Func<TItem, bool>> predicate)
        => _query.All(predicate);

    public override int Count()
        => _query.Count();

    public override int Count(Expression<Func<TItem, bool>> predicate)
        => _query.Count(predicate);

    public override long LongCount()
        => _query.LongCount();

    public override long LongCount(Expression<Func<TItem, bool>> predicate)
        => _query.LongCount(predicate);

    public override TItem? Min()
        => _query.Min();

    public override TResult? Min<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : default
        => _query.Min(selector);

    public override TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _query.MinBy(keySelector);

    public override TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => _query.MinBy(keySelector, comparer);

    public override TItem? Max()
        => _query.Max();

    public override TResult? Max<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : default
        => _query.Max(selector);

    public override TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => _query.MaxBy(keySelector);

    public override TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => _query.MaxBy(keySelector, comparer);

    public override int Sum(Expression<Func<TItem, int>> selector)
        => _query.Sum(selector);

    public override int? Sum(Expression<Func<TItem, int?>> selector)
        => _query.Sum(selector);

    public override long Sum(Expression<Func<TItem, long>> selector)
        => _query.Sum(selector);

    public override long? Sum(Expression<Func<TItem, long?>> selector)
        => _query.Sum(selector);

    public override float Sum(Expression<Func<TItem, float>> selector)
        => _query.Sum(selector);

    public override float? Sum(Expression<Func<TItem, float?>> selector)
        => _query.Sum(selector);

    public override double Sum(Expression<Func<TItem, double>> selector)
        => _query.Sum(selector);

    public override double? Sum(Expression<Func<TItem, double?>> selector)
        => _query.Sum(selector);

    public override decimal Sum(Expression<Func<TItem, decimal>> selector)
        => _query.Sum(selector);

    public override decimal? Sum(Expression<Func<TItem, decimal?>> selector)
        => _query.Sum(selector);

    public override double Average(Expression<Func<TItem, int>> selector)
        => _query.Average(selector);

    public override double? Average(Expression<Func<TItem, int?>> selector)
        => _query.Average(selector);

    public override float Average(Expression<Func<TItem, float>> selector)
        => _query.Average(selector);

    public override float? Average(Expression<Func<TItem, float?>> selector)
        => _query.Average(selector);

    public override double Average(Expression<Func<TItem, long>> selector)
        => _query.Average(selector);

    public override double? Average(Expression<Func<TItem, long?>> selector)
        => _query.Average(selector);

    public override double Average(Expression<Func<TItem, double>> selector)
        => _query.Average(selector);

    public override double? Average(Expression<Func<TItem, double?>> selector)
        => _query.Average(selector);

    public override decimal Average(Expression<Func<TItem, decimal>> selector)
        => _query.Average(selector);

    public override decimal? Average(Expression<Func<TItem, decimal?>> selector)
        => _query.Average(selector);

    public override TItem Aggregate(Expression<Func<TItem, TItem, TItem>> func)
        => _query.Aggregate(func);

    public override TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func)
        => _query.Aggregate(seed, func);

    public override TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        => _query.Aggregate(seed, func, selector);

    public override IReadOnlyList<TItem> ToArray()
        => [.. _query];

    public override IReadOnlyList<TResult> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping)
        => _query.ToArray(mapping);

    public override IList<TItem> ToList()
        => [.. _query];

    public override IList<TResult> ToList<TResult>(Expression<Func<TItem, TResult>> mapping)
        => _query.ToList(mapping);

    public override ISet<TItem> ToHashSet()
        => _query.ToHashSet();

    public override ISet<TResult> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping)
        => _query.ToHashSet(mapping);

    public override TResultRepository ToRepository<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping)
        where TResultRepository : class
        => InstanceFactory.Create<TResultRepository>(_query.ToList(mapping));

    public override IRepository<TResult> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping)
        where TResult : class
        => new InMemoryRepository<TResult>(_query.ToList(mapping));

    public override IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null)
        => _query.ToDictionary(selectKey, selectValue, comparer);

    public override void Add(TItem newItem)
        => _data.Add(newItem);

    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        Remove(predicate);
        Add(updatedItem);
    }

    public override void Remove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = _query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return;
        _data.Remove(itemToRemove);
    }
}
