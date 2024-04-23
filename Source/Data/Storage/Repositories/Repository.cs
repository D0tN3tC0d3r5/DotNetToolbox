namespace DotNetToolbox.Data.Repositories;

public abstract class Repository<TStrategy, TItem>
    : Repository<Repository<TStrategy, TItem>, TStrategy, TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {
    protected Repository(IStrategyFactory factory)
        : base(factory) {
    }
    protected Repository(TStrategy strategy)
        : base(strategy) {
    }
    protected Repository(IEnumerable<TItem> data, IStrategyFactory factory)
        : base(data, factory) {
    }
    protected Repository(IEnumerable<TItem> data, TStrategy strategy)
        : base(data, strategy) {
    }
}

public abstract class Repository<TRepository, TStrategy, TItem> : IOrderedRepository<TItem>, IEnumerable<TItem>
    where TRepository : Repository<TRepository, TStrategy, TItem>
    where TStrategy : class, IRepositoryStrategy<TItem> {

    protected Repository(TStrategy strategy)
        : this([], strategy) {
    }
    protected Repository(IStrategyFactory factory)
        : this([], factory) {
    }
    protected Repository(IEnumerable<TItem> data, IStrategyFactory factory)
        : this(data, IsNotNull(factory).GetRequiredStrategy<TItem, TStrategy>()) {
    }
    protected Repository(IEnumerable<TItem> data, TStrategy strategy) {
        Strategy = IsNotNull(strategy);
        Strategy.Seed(data.ToList());
    }

    protected TStrategy Strategy { get; init; }

    public void Seed(IEnumerable<TItem> seed)
        => Strategy.Seed(seed);

    public IEnumerator<TItem> GetEnumerator()
        => IsOfType<IQueryable<TItem>>(Strategy.Query).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public IRepository<TResult> OfType<TResult>()
        => Strategy.OfType<TResult>();
    public IRepository<TResult> Cast<TResult>()
        => Strategy.Cast<TResult>();
    public IRepository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => Strategy.Where(predicate);
    public IRepository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.Where(predicate);
    public IRepository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        => Strategy.Select(selector);
    public IRepository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        => Strategy.Select(selector);
    public IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        => Strategy.SelectMany(selector);
    public IRepository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        => Strategy.SelectMany(selector);
    public IRepository<TResult> SelectMany<TSource, TResult>(Expression<Func<TItem, IEnumerable<TSource>>> collectionSelector, Expression<Func<TItem, TSource, TResult>> resultSelector)
        => Strategy.SelectMany(collectionSelector, resultSelector);
    public IRepository<TResult> SelectMany<TSource, TResult>(Expression<Func<TItem, int, IEnumerable<TSource>>> collectionSelector, Expression<Func<TItem, TSource, TResult>> resultSelector)
        => Strategy.SelectMany(collectionSelector, resultSelector);
    public IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                           Expression<Func<TItem, TKey>> outerKeySelector,
                                                           Expression<Func<TInner, TKey>> innerKeySelector,
                                                           Expression<Func<TItem, TInner, TResult>> resultSelector)
        => Strategy.Join(inner,
                          outerKeySelector,
                          innerKeySelector,
                          resultSelector);
    public IRepository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                           Expression<Func<TItem, TKey>> outerKeySelector,
                                                           Expression<Func<TInner, TKey>> innerKeySelector,
                                                           Expression<Func<TItem, TInner, TResult>> resultSelector,
                                                           IEqualityComparer<TKey>? comparer)
        => Strategy.Join(inner,
                          outerKeySelector,
                          innerKeySelector,
                          resultSelector,
                          comparer);
    public IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                Expression<Func<TItem, TKey>> outerKeySelector,
                                                                Expression<Func<TInner, TKey>> innerKeySelector,
                                                                Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector)
        => Strategy.GroupJoin(inner,
                               outerKeySelector,
                               innerKeySelector,
                               resultSelector);
    public IRepository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                                Expression<Func<TItem, TKey>> outerKeySelector,
                                                                Expression<Func<TInner, TKey>> innerKeySelector,
                                                                Expression<Func<TItem, IEnumerable<TInner>, TResult>> resultSelector,
                                                                IEqualityComparer<TKey>? comparer)
        => Strategy.GroupJoin(inner,
                               outerKeySelector,
                               innerKeySelector,
                               resultSelector,
                               comparer);
    public IOrderedRepository<TItem> Order()
        => Strategy.Order();
    public IOrderedRepository<TItem> Order(IComparer<TItem> comparer)
        => Strategy.Order(comparer);
    public IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.OrderBy(keySelector);
    public IOrderedRepository<TItem> OrderBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.OrderBy(keySelector, comparer);
    public IOrderedRepository<TItem> OrderDescending()
        => Strategy.OrderDescending();
    public IOrderedRepository<TItem> OrderDescending(IComparer<TItem> comparer)
        => Strategy.OrderDescending(comparer);
    public IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.OrderByDescending(keySelector);
    public IOrderedRepository<TItem> OrderByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.OrderByDescending(keySelector, comparer);
    public IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenBy(keySelector);
    public IOrderedRepository<TItem> ThenBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenBy(keySelector, comparer);
    public IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ThenByDescending(keySelector);
    public IOrderedRepository<TItem> ThenByDescending<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TKey>? comparer)
        => Strategy.ThenByDescending(keySelector, comparer);
    public IRepository<TItem> Take(int count)
        => Strategy.Take(count);
    public IRepository<TItem> Take(Range range)
        => Strategy.Take(range);
    public IRepository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => Strategy.TakeWhile(predicate);
    public IRepository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.TakeWhile(predicate);
    public IRepository<TItem> TakeLast(int count)
        => Strategy.TakeLast(count);
    public IRepository<TItem> Skip(int count)
        => Strategy.Skip(count);
    public IRepository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => Strategy.SkipWhile(predicate);
    public IRepository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => Strategy.SkipWhile(predicate);
    public IRepository<TItem> SkipLast(int count)
        => Strategy.SkipLast(count);
    public IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.GroupBy(keySelector);
    public IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => Strategy.GroupBy(keySelector, elementSelector);
    public IRepository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, comparer);
    public IRepository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, elementSelector, comparer);
    public IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        => Strategy.GroupBy(keySelector, elementSelector, resultSelector);
    public IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        => Strategy.GroupBy(keySelector, resultSelector);
    public IRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector, resultSelector, comparer);
    public IRepository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        => Strategy.GroupBy(keySelector,
                             elementSelector,
                             resultSelector,
                             comparer);
    public IRepository<TItem> Distinct()
        => Strategy.Distinct();
    public IRepository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => Strategy.Distinct(comparer);
    public IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.DistinctBy(keySelector);
    public IRepository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.DistinctBy(keySelector, comparer);
    public IRepository<TItem[]> Chunk(int size)
        => Strategy.Chunk(size);
    public IRepository<TItem> Concat(IEnumerable<TItem> source)
        => Strategy.Concat(source);
    public IRepository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        => Strategy.Combine(source2, resultSelector);
    public IRepository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => Strategy.Zip(source);
    public IRepository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => Strategy.Zip(source2, source3);
    public IRepository<TItem> Union(IEnumerable<TItem> source2)
        => Strategy.Union(source2);
    public IRepository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Union(source2, comparer);
    public IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.UnionBy(source2, keySelector);
    public IRepository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.UnionBy(source2, keySelector, comparer);
    public IRepository<TItem> Intersect(IEnumerable<TItem> source2)
        => Strategy.Intersect(source2);
    public IRepository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Intersect(source2, comparer);
    public IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.IntersectBy(source2, keySelector);
    public IRepository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.IntersectBy(source2, keySelector, comparer);
    public IRepository<TItem> Except(IEnumerable<TItem> source2)
        => Strategy.Except(source2);
    public IRepository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.Except(source2, comparer);
    public IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => Strategy.ExceptBy(source2, keySelector);
    public IRepository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => Strategy.ExceptBy(source2, keySelector, comparer);
    public IRepository<TItem?> DefaultIfEmpty()
        => Strategy.DefaultIfEmpty();
    public IRepository<TItem> DefaultIfEmpty(TItem defaultValue)
        => Strategy.DefaultIfEmpty(defaultValue);
    public IRepository<TItem> Reverse()
        => Strategy.Reverse();
    public IRepository<TItem> Append(TItem element)
        => Strategy.Append(element);
    public IRepository<TItem> Prepend(TItem element)
        => Strategy.Prepend(element);
    public TItem[] ToArray()
        => Strategy.ToArray();
    public TResult[] ToArray<TResult>(Expression<Func<TItem, TResult>> mapping)
        => Strategy.ToArray(mapping);
    public List<TItem> ToList()
        => Strategy.ToList();
    public List<TResult> ToList<TResult>(Expression<Func<TItem, TResult>> mapping)
        => Strategy.ToList(mapping);
    public HashSet<TItem> ToHashSet()
        => Strategy.ToHashSet();
    public HashSet<TResult> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping)
        => Strategy.ToHashSet(mapping);
    public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null)
        where TKey : notnull
        => Strategy.ToDictionary(selectKey, selectValue, comparer);
    public TItem First()
        => Strategy.First();
    public TItem First(Expression<Func<TItem, bool>> predicate)
        => Strategy.First(predicate);
    public TItem? FirstOrDefault()
        => Strategy.FirstOrDefault();
    public TItem FirstOrDefault(TItem defaultValue)
        => Strategy.FirstOrDefault(defaultValue);
    public TItem? FirstOrDefault(Expression<Func<TItem, bool>> predicate)
        => Strategy.FirstOrDefault(predicate);
    public TItem FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => Strategy.FirstOrDefault(predicate, defaultValue);
    public TItem Last()
        => Strategy.Last();
    public TItem Last(Expression<Func<TItem, bool>> predicate)
        => Strategy.Last(predicate);
    public TItem? LastOrDefault()
        => Strategy.LastOrDefault();
    public TItem LastOrDefault(TItem defaultValue)
        => Strategy.LastOrDefault(defaultValue);
    public TItem? LastOrDefault(Expression<Func<TItem, bool>> predicate)
        => Strategy.LastOrDefault(predicate);
    public TItem LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => Strategy.LastOrDefault(predicate, defaultValue);
    public TItem Single()
        => Strategy.Single();
    public TItem Single(Expression<Func<TItem, bool>> predicate)
        => Strategy.Single(predicate);
    public TItem? SingleOrDefault()
        => Strategy.SingleOrDefault();
    public TItem SingleOrDefault(TItem defaultValue)
        => Strategy.SingleOrDefault(defaultValue);
    public TItem? SingleOrDefault(Expression<Func<TItem, bool>> predicate)
        => Strategy.SingleOrDefault(predicate);
    public TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => Strategy.SingleOrDefault(predicate, defaultValue);
    public TItem ElementAt(int index)
        => Strategy.ElementAt(index);
    public TItem ElementAt(Index index)
        => Strategy.ElementAt(index);
    public TItem? ElementAtOrDefault(int index)
        => Strategy.ElementAtOrDefault(index);
    public TItem? ElementAtOrDefault(Index index)
        => Strategy.ElementAtOrDefault(index);
    public bool Contains(TItem item)
        => Strategy.Contains(item);
    public bool Contains(TItem item, IEqualityComparer<TItem>? comparer)
        => Strategy.Contains(item, comparer);
    public bool SequenceEqual(IEnumerable<TItem> source2)
        => Strategy.SequenceEqual(source2);
    public bool SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => Strategy.SequenceEqual(source2, comparer);
    public bool Any()
        => Strategy.Any();
    public bool Any(Expression<Func<TItem, bool>> predicate)
        => Strategy.Any(predicate);
    public bool All(Expression<Func<TItem, bool>> predicate)
        => Strategy.All(predicate);
    public int Count()
        => Strategy.Count();
    public int Count(Expression<Func<TItem, bool>> predicate)
        => Strategy.Count(predicate);
    public long LongCount()
        => Strategy.LongCount();
    public long LongCount(Expression<Func<TItem, bool>> predicate)
        => Strategy.LongCount(predicate);
    public TItem? Min()
        => Strategy.Min();
    public TResult? Min<TResult>(Expression<Func<TItem, TResult>> selector)
        => Strategy.Min(selector);
    public TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.MinBy(keySelector);
    public TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => Strategy.MinBy(keySelector, comparer);
    public TItem? Max()
        => Strategy.Max();
    public TResult? Max<TResult>(Expression<Func<TItem, TResult>> selector)
        => Strategy.Max(selector);
    public TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => Strategy.MaxBy(keySelector);
    public TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => Strategy.MaxBy(keySelector, comparer);
    public int Sum(Expression<Func<TItem, int>> selector)
        => Strategy.Sum(selector);
    public int? Sum(Expression<Func<TItem, int?>> selector)
        => Strategy.Sum(selector);
    public long Sum(Expression<Func<TItem, long>> selector)
        => Strategy.Sum(selector);
    public long? Sum(Expression<Func<TItem, long?>> selector)
        => Strategy.Sum(selector);
    public float Sum(Expression<Func<TItem, float>> selector)
        => Strategy.Sum(selector);
    public float? Sum(Expression<Func<TItem, float?>> selector)
        => Strategy.Sum(selector);
    public double Sum(Expression<Func<TItem, double>> selector)
        => Strategy.Sum(selector);
    public double? Sum(Expression<Func<TItem, double?>> selector)
        => Strategy.Sum(selector);
    public decimal Sum(Expression<Func<TItem, decimal>> selector)
        => Strategy.Sum(selector);
    public decimal? Sum(Expression<Func<TItem, decimal?>> selector)
        => Strategy.Sum(selector);
    public double Average(Expression<Func<TItem, int>> selector)
        => Strategy.Average(selector);
    public double? Average(Expression<Func<TItem, int?>> selector)
        => Strategy.Average(selector);
    public float Average(Expression<Func<TItem, float>> selector)
        => Strategy.Average(selector);
    public float? Average(Expression<Func<TItem, float?>> selector)
        => Strategy.Average(selector);
    public double Average(Expression<Func<TItem, long>> selector)
        => Strategy.Average(selector);
    public double? Average(Expression<Func<TItem, long?>> selector)
        => Strategy.Average(selector);
    public double Average(Expression<Func<TItem, double>> selector)
        => Strategy.Average(selector);
    public double? Average(Expression<Func<TItem, double?>> selector)
        => Strategy.Average(selector);
    public decimal Average(Expression<Func<TItem, decimal>> selector)
        => Strategy.Average(selector);
    public decimal? Average(Expression<Func<TItem, decimal?>> selector)
        => Strategy.Average(selector);
    public TItem Aggregate(Expression<Func<TItem, TItem, TItem>> func)
        => Strategy.Aggregate(func);
    public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func)
        => Strategy.Aggregate(seed, func);
    public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        => Strategy.Aggregate(seed, func, selector);
    public void Add(TItem newItem)
        => Strategy.Add(newItem);
    public void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => Strategy.Update(predicate, updatedItem);
    public void Remove(Expression<Func<TItem, bool>> predicate)
        => Strategy.Remove(predicate);
}
