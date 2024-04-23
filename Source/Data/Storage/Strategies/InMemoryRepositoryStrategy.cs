namespace DotNetToolbox.Data.Strategies;
public class InMemoryRepositoryStrategy<TRepository, TItem>
    : RepositoryStrategy<TItem>
    where TRepository : IOrderedRepository<TItem>{

    public InMemoryRepositoryStrategy() {
        UpdatableData = OriginalData.Cast<TItem>().ToList();
    }

    internal InMemoryRepositoryStrategy(IEnumerable data, IQueryable query)
        : base(data, query) {
        UpdatableData = Query.Cast<TItem>().ToList();
    }

    public override void Seed(IEnumerable<TItem> seed) {
        OriginalData = seed;
        Query = OriginalData.AsQueryable();
        UpdatableData = Query.Cast<TItem>().ToList();
    }

    protected IList<TItem> UpdatableData { get; set; }

    private static IRepository<TResult> ApplyAndCreate<TResult>(Func<IQueryable<TResult>> updateSource){
        var result = updateSource();
        return RepositoryFactory.CreateRepository<TRepository, TResult>(result);
    }
    private static IOrderedRepository<TResult> ApplyAndCreateOrdered<TResult>(Func<IQueryable<TResult>> updateSource){
        var result = updateSource();
        return RepositoryFactory.CreateOrderedRepository<TRepository, TResult>(result);
    }

    public override IRepository<TResult> OfType<TResult>()
        => ApplyAndCreate(Query.OfType<TResult>);
    public override IRepository<TResult> Cast<TResult>()
        => typeof(TItem).IsAssignableTo(typeof(TResult))
               ? ApplyAndCreate(Query.Cast<TResult>)
               : throw new InvalidCastException("The cast is invalid.");
    public override IRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => Query.Where(predicate));
    public override IRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => Query.Where(predicate));
    public override IRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        => ApplyAndCreate(() => Query.Select(selector));
    public override IRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        => ApplyAndCreate(() => Query.Select(selector));
    public override IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        => ApplyAndCreate(() => Query.SelectMany(selector));
    public override IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        => ApplyAndCreate(() => Query.SelectMany(selector));
    public override IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.SelectMany(collectionSelector, resultSelector));
    public override IRepository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.SelectMany(collectionSelector, resultSelector));
    public override IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.Join(inner, outerKeySelector, innerKeySelector, resultSelector));
    public override IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));
    public override IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector));
    public override IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TItem, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer));
    public override IOrderedRepository<TItem> Order()
        => ApplyAndCreateOrdered(Query.Order);
    public override IOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => Query.Order(comparer));
    public override IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => Query.OrderBy(keySelector));
    public override IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => Query.OrderBy(keySelector, comparer));
    public override IOrderedRepository<TItem> OrderDescending()
        => ApplyAndCreateOrdered(Query.OrderDescending);
    public override IOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => ApplyAndCreateOrdered(() => Query.OrderDescending(comparer));
    public override IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreateOrdered(() => Query.OrderByDescending(keySelector));
    public override IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => ApplyAndCreateOrdered(() => Query.OrderByDescending(keySelector, comparer));
    public override IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector));
    public override IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenBy(keySelector, comparer));
    public override IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector));
    public override IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Query is not IOrderedQueryable<TItem> query
        ? throw new NotSupportedException("The collection must be ordered.")
        : ApplyAndCreateOrdered(() => query.ThenByDescending(keySelector, comparer));
    public override IRepository<TItem> Take(int count)
        => ApplyAndCreate(() => Query.Take(count));
    public override IRepository<TItem> Take(Range range)
        => ApplyAndCreate(() => Query.Take(range));
    public override IRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => Query.TakeWhile(predicate));
    public override IRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => Query.TakeWhile(predicate));
    public override IRepository<TItem> TakeLast(int count)
        => ApplyAndCreate(() => Query.TakeLast(count));
    public override IRepository<TItem> Skip(int count)
        => ApplyAndCreate(() => Query.Skip(count));
    public override IRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => ApplyAndCreate(() => Query.SkipWhile(predicate));
    public override IRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => ApplyAndCreate(() => Query.SkipWhile(predicate));
    public override IRepository<TItem> SkipLast(int count)
        => ApplyAndCreate(() => Query.SkipLast(count));
    public override IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector));
    public override IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector));
    public override IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, comparer));
    public override IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector, comparer));
    public override IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector, resultSelector));
    public override IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, resultSelector));
    public override IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, resultSelector, comparer));
    public override IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.GroupBy(keySelector, elementSelector, resultSelector, comparer));
    public override IRepository<TItem> Distinct()
        => ApplyAndCreate(Query.Distinct);
    public override IRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Distinct(comparer));
    public override IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.DistinctBy(keySelector));
    public override IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.DistinctBy(keySelector, comparer));
    public override IRepository<TItem[]> Chunk(int size)
        => ApplyAndCreate(() => Query.Chunk(size));
    public override IRepository<TItem> Concat(IEnumerable<TItem> source)
        => ApplyAndCreate(() => Query.Concat(source));
    public override IRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        => ApplyAndCreate(() => Query.Zip(source2, resultSelector));
    public override IRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source) {
        Expression<Func<(TItem First, TSecond Second), IPack<TItem, TSecond>>> convert = z => new Pack<TItem, TSecond>(z.First, z.Second);
        return ApplyAndCreate(() => Query.Zip(source).Select(convert));
    }
    public override IRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3) {
        Expression<Func<(TItem First, TSecond Second, TThird Third), IPack<TItem, TSecond, TThird>>> convert = z => new Pack<TItem, TSecond, TThird>(z.First, z.Second, z.Third);
        return ApplyAndCreate(() => Query.Zip(source2, source3).Select(convert));
    }
    public override IRepository<TItem> Union(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => Query.Union(source2));
    public override IRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Union(source2, comparer));
    public override IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.UnionBy(source2, keySelector));
    public override IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.UnionBy(source2, keySelector, comparer));
    public override IRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => Query.Intersect(source2));
    public override IRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Intersect(source2, comparer));
    public override IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.IntersectBy(source2, keySelector));
    public override IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.IntersectBy(source2, keySelector, comparer));
    public override IRepository<TItem> Except(IEnumerable<TItem> source2)
        => ApplyAndCreate(() => Query.Except(source2));
    public override IRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => ApplyAndCreate(() => Query.Except(source2, comparer));
    public override IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => ApplyAndCreate(() => Query.ExceptBy(source2, keySelector));
    public override IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => ApplyAndCreate(() => Query.ExceptBy(source2, keySelector, comparer));
    public override IRepository<TItem?> DefaultIfEmpty()
        => ApplyAndCreate(Query.DefaultIfEmpty);
    public override IRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => ApplyAndCreate(() => Query.DefaultIfEmpty(defaultValue));
    public override IRepository<TItem> Reverse()
        => ApplyAndCreate(Query.Reverse);
    public override IRepository<TItem> Append(TItem element)
        => ApplyAndCreate(() => Query.Append(element));
    public override IRepository<TItem> Prepend(TItem element)
        => ApplyAndCreate(() => Query.Prepend(element));
    public override TItem First()
        => Query.First();
    public override TItem First(Expression<Func<TItem, bool>> predicate)
        => Query.First(predicate);
    public override TItem? FirstOrDefault()
        => Query.FirstOrDefault();
    public override TItem FirstOrDefault(TItem defaultValue)
        => Query.FirstOrDefault() ?? defaultValue;
    public override TItem? FirstOrDefault(Expression<Func<TItem, bool>> predicate)
        => Query.FirstOrDefault(predicate);
    public override TItem Last()
        => Query.Last();
    public override TItem Last(Expression<Func<TItem, bool>> predicate)
        => Query.Last(predicate);
    public override TItem? LastOrDefault()
        => Query.LastOrDefault();
    public override TItem LastOrDefault(TItem defaultValue)
        => Query.LastOrDefault() ?? defaultValue;
    public override TItem? LastOrDefault(Expression<Func<TItem, bool>> predicate)
        => Query.LastOrDefault(predicate);
    public override TItem LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => Query.LastOrDefault(predicate) ?? defaultValue;
    public override TItem Single()
        => Query.Single();
    public override TItem Single(Expression<Func<TItem, bool>> predicate)
        => Query.Single(predicate);
    public override TItem? SingleOrDefault()
        => Query.SingleOrDefault();
    public override TItem SingleOrDefault(TItem defaultValue)
        => Query.SingleOrDefault() ?? defaultValue;
    public override TItem? SingleOrDefault(Expression<Func<TItem, bool>> predicate)
        => Query.SingleOrDefault(predicate);
    public override TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => Query.SingleOrDefault(predicate) ?? defaultValue;
    public override TItem ElementAt(int index)
        => Query.ElementAt(index);
    public override TItem ElementAt(Index index)
        => Query.ElementAt(index);
    public override TItem? ElementAtOrDefault(int index)
        => Query.ElementAtOrDefault(index);
    public override TItem? ElementAtOrDefault(Index index)
        => Query.ElementAtOrDefault(index);
    public override bool Contains(TItem item)
        => Query.Contains(item);
    public override bool Contains(TItem item, IEqualityComparer<TItem>? comparer)
        => Query.Contains(item, comparer);
    public override bool SequenceEqual(IEnumerable<TItem> source2)
        => Query.SequenceEqual(source2);
    public override bool SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Query.SequenceEqual(source2, comparer);
    public override bool Any()
        => Query.Any();
    public override bool Any(Expression<Func<TItem, bool>> predicate)
        => Query.Any(predicate);
    public override bool All(Expression<Func<TItem, bool>> predicate)
        => Query.All(predicate);
    public override int Count()
        => Query.Count();
    public override int Count(Expression<Func<TItem, bool>> predicate)
        => Query.Count(predicate);
    public override long LongCount()
        => Query.LongCount();
    public override long LongCount(Expression<Func<TItem, bool>> predicate)
        => Query.LongCount(predicate);
    public override TItem? Min()
        => Query.Min();
    public override TResult? Min<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : default
        => Query.Min(selector);
    public override TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Query.MinBy(keySelector);
    public override TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => Query.MinBy(keySelector, comparer);
    public override TItem? Max()
        => Query.Max();
    public override TResult? Max<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : default
        => Query.Max(selector);
    public override TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Query.MaxBy(keySelector);
    public override TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => Query.MaxBy(keySelector, comparer);
    public override int Sum(Expression<Func<TItem, int>> selector)
        => Query.Sum(selector);
    public override int? Sum(Expression<Func<TItem, int?>> selector)
        => Query.Sum(selector);
    public override long Sum(Expression<Func<TItem, long>> selector)
        => Query.Sum(selector);
    public override long? Sum(Expression<Func<TItem, long?>> selector)
        => Query.Sum(selector);
    public override float Sum(Expression<Func<TItem, float>> selector)
        => Query.Sum(selector);
    public override float? Sum(Expression<Func<TItem, float?>> selector)
        => Query.Sum(selector);
    public override double Sum(Expression<Func<TItem, double>> selector)
        => Query.Sum(selector);
    public override double? Sum(Expression<Func<TItem, double?>> selector)
        => Query.Sum(selector);
    public override decimal Sum(Expression<Func<TItem, decimal>> selector)
        => Query.Sum(selector);
    public override decimal? Sum(Expression<Func<TItem, decimal?>> selector)
        => Query.Sum(selector);
    public override double Average(Expression<Func<TItem, int>> selector)
        => Query.Average(selector);
    public override double? Average(Expression<Func<TItem, int?>> selector)
        => Query.Average(selector);
    public override float Average(Expression<Func<TItem, float>> selector)
        => Query.Average(selector);
    public override float? Average(Expression<Func<TItem, float?>> selector)
        => Query.Average(selector);
    public override double Average(Expression<Func<TItem, long>> selector)
        => Query.Average(selector);
    public override double? Average(Expression<Func<TItem, long?>> selector)
        => Query.Average(selector);
    public override double Average(Expression<Func<TItem, double>> selector)
        => Query.Average(selector);
    public override double? Average(Expression<Func<TItem, double?>> selector)
        => Query.Average(selector);
    public override decimal Average(Expression<Func<TItem, decimal>> selector)
        => Query.Average(selector);
    public override decimal? Average(Expression<Func<TItem, decimal?>> selector)
        => Query.Average(selector);
    public override TItem Aggregate(Expression<Func<TItem, TItem, TItem>> func)
        => Query.Aggregate(func);
    public override TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func)
        => Query.Aggregate(seed, func);
    public override TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        => Query.Aggregate(seed, func, selector);
    public override IReadOnlyList<TItem> ToArray()
        => [.. Query];
    public override IReadOnlyList<TResult> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping)
        => Query.ToArray(mapping);
    public override IList<TItem> ToList()
        => [.. Query];
    public override IList<TResult> ToList<TResult>(Expression<Func<TItem, TResult>> mapping)
        => Query.ToList(mapping);
    public override ISet<TItem> ToHashSet()
        => Query.ToHashSet();
    public override ISet<TResult> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping)
        => Query.ToHashSet(mapping);
    public override TResultRepository ToRepository<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping)
        where TResultRepository : class
        => InstanceFactory.Create<TResultRepository>(Data, Query.Select(mapping).Expression);
    public override IRepository<TResult> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping)
        => new InMemoryRepository<TResult>(OriginalData, Query.Cast<TItem>().Select(mapping).Expression);

    public override IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null)
        => Query.ToDictionary(selectKey, selectValue, comparer);
    public override void Add(TItem newItem)
        => UpdatableData.Add(newItem);
    public override void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem) {
        Remove(predicate);
        Add(updatedItem);
    }
    public override void Remove(Expression<Func<TItem, bool>> predicate) {
        var itemToRemove = Query.FirstOrDefault(predicate);
        if (itemToRemove is null)
            return;
        UpdatableData.Remove(itemToRemove);
    }
}
