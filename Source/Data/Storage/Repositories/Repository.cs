namespace DotNetToolbox.Data.Repositories;

public class Repository<TItem>
    : IRepository<TItem>,
      IEnumerable<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _data;
    private readonly RepositoryStrategy<TItem> _strategy;

    public Repository(IStrategyFactory? provider = null)
        : this([], provider) {
    }

    public Repository(IEnumerable<TItem> data, IStrategyFactory? provider = null) {
        var list = data.ToList();
        _data = IsNotNull(list).AsQueryable();
        _strategy = provider?.GetRepositoryStrategy<TItem>(_data)
            ?? new InMemoryRepositoryStrategy<Repository<TItem>, TItem>(_data);
    }

    public IEnumerator<TItem> GetEnumerator()
        => _data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public Repository<TResult> OfType<TResult>()
        where TResult : class
        => _strategy.OfType<TResult>();

    public Repository<TResult> Cast<TResult>()
        where TResult : class
        => _strategy.Cast<TResult>();

    public Repository<TItem> Where(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public Repository<TItem> Where(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public Repository<TResult> Select<TResult>(Expression<Func<TItem, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> Select<TResult>(Expression<Func<TItem, int, TResult>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> SelectMany<TResult>(Expression<Func<TItem, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> SelectMany<TResult>(Expression<Func<TItem, int, IEnumerable<TResult>>> selector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> SelectMany<TCollection, TResult>(Expression<Func<TItem, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TItem, TCollection, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TInner, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> Join<TInner, TKey, TResult>(IEnumerable<TInner> inner,
                                                           Expression<Func<TKey>> outerKeySelector,
                                                           Expression<Func<TInner, TKey>> innerKeySelector,
                                                           Expression<Func<TInner, TResult>> resultSelector,
                                                           IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner, Expression<Func<TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<IEnumerable<TInner>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> GroupJoin<TInner, TKey, TResult>(IEnumerable<TInner> inner,
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

    public Repository<TItem> Take(int count)
        => throw new NotImplementedException();

    public Repository<TItem> Take(Range range)
        => throw new NotImplementedException();

    public Repository<TItem> TakeWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public Repository<TItem> TakeWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public Repository<TItem> TakeLast(int count)
        => throw new NotImplementedException();

    public Repository<TItem> Skip(int count)
        => throw new NotImplementedException();

    public Repository<TItem> SkipWhile(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public Repository<TItem> SkipWhile(Expression<Func<TItem, int, bool>> predicate)
        => throw new NotImplementedException();

    public Repository<TItem> SkipLast(int count)
        => throw new NotImplementedException();

    public Repository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public Repository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector)
        => throw new NotImplementedException();

    public Repository<IGrouping<TKey, TItem>> GroupBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public Repository<IGrouping<TKey, TElement>> GroupBy<TKey, TElement>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public Repository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> GroupBy<TKey, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TItem>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TResult> GroupBy<TKey, TElement, TResult>(Expression<Func<TItem, TKey>> keySelector, Expression<Func<TItem, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<TItem> Distinct()
        => throw new NotImplementedException();

    public Repository<TItem> Distinct(IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public Repository<TItem> DistinctBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem[]> Chunk(int size)
        => throw new NotImplementedException();

    public Repository<TItem> Concat(IEnumerable<TItem> source)
        => throw new NotImplementedException();

    public Repository<TResult> Combine<TSecond, TResult>(IEnumerable<TSecond> source2, Expression<Func<TItem, TSecond, TResult>> resultSelector)
        where TResult : class
        => throw new NotImplementedException();

    public Repository<IPack<TItem, TSecond>> Zip<TSecond>(IEnumerable<TSecond> source)
        => throw new NotImplementedException();

    public Repository<IPack<TItem, TSecond, TThird>> Zip<TSecond, TThird>(IEnumerable<TSecond> source2, IEnumerable<TThird> source3)
        => throw new NotImplementedException();

    public Repository<TItem> Union(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public Repository<TItem> Union(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public Repository<TItem> UnionBy<TKey>(IEnumerable<TItem> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> Intersect(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public Repository<TItem> Intersect(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public Repository<TItem> IntersectBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> Except(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public Repository<TItem> Except(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public Repository<TItem> ExceptBy<TKey>(IEnumerable<TKey> source2, Expression<Func<TItem, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        => throw new NotImplementedException();

    public Repository<TItem> DefaultIfEmpty()
        => throw new NotImplementedException();

    public Repository<TItem> DefaultIfEmpty(TItem defaultValue)
        => throw new NotImplementedException();

    public Repository<TItem> Reverse()
        => throw new NotImplementedException();

    public Repository<TItem> Append(TItem element)
        => throw new NotImplementedException();

    public Repository<TItem> Prepend(TItem element)
        => throw new NotImplementedException();

    public virtual TItem First()
        => throw new NotImplementedException();

    public virtual TItem First(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem FirstOrDefault()
        => throw new NotImplementedException();

    public virtual TItem FirstOrDefault(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem FirstOrDefault(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem Last()
        => throw new NotImplementedException();

    public virtual TItem Last(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem LastOrDefault()
        => throw new NotImplementedException();

    public virtual TItem LastOrDefault(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem LastOrDefault(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem Single()
        => throw new NotImplementedException();

    public virtual TItem Single(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem SingleOrDefault()
        => throw new NotImplementedException();

    public virtual TItem SingleOrDefault(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem ElementAt(int index)
        => throw new NotImplementedException();

    public virtual TItem ElementAt(Index index)
        => throw new NotImplementedException();

    public virtual TItem ElementAtOrDefault(int index)
        => throw new NotImplementedException();

    public virtual TItem ElementAtOrDefault(Index index)
        => throw new NotImplementedException();

    public virtual bool Contains(TItem item)
        => throw new NotImplementedException();

    public virtual bool Contains(TItem item, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual bool SequenceEqual(IEnumerable<TItem> source2)
        => throw new NotImplementedException();

    public virtual bool SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual bool Any()
        => throw new NotImplementedException();

    public virtual bool Any(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual bool All(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual int Count()
        => throw new NotImplementedException();

    public virtual int Count(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual long LongCount()
        => throw new NotImplementedException();

    public virtual long LongCount(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem Min()
        => throw new NotImplementedException();

    public virtual TResult Min<TResult>(Expression<Func<TItem, TResult>> selector)
        => throw new NotImplementedException();

    public virtual TItem MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual TItem MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual TItem Max()
        => throw new NotImplementedException();

    public virtual TResult Max<TResult>(Expression<Func<TItem, TResult>> selector)
        => throw new NotImplementedException();

    public virtual TItem MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual TItem MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual int Sum(Expression<Func<TItem, int>> selector)
        => throw new NotImplementedException();

    public virtual int? Sum(Expression<Func<TItem, int?>> selector)
        => throw new NotImplementedException();

    public virtual long Sum(Expression<Func<TItem, long>> selector)
        => throw new NotImplementedException();

    public virtual long? Sum(Expression<Func<TItem, long?>> selector)
        => throw new NotImplementedException();

    public virtual float Sum(Expression<Func<TItem, float>> selector)
        => throw new NotImplementedException();

    public virtual float? Sum(Expression<Func<TItem, float?>> selector)
        => throw new NotImplementedException();

    public virtual double Sum(Expression<Func<TItem, double>> selector)
        => throw new NotImplementedException();

    public virtual double? Sum(Expression<Func<TItem, double?>> selector)
        => throw new NotImplementedException();

    public virtual decimal Sum(Expression<Func<TItem, decimal>> selector)
        => throw new NotImplementedException();

    public virtual decimal? Sum(Expression<Func<TItem, decimal?>> selector)
        => throw new NotImplementedException();

    public virtual double Average(Expression<Func<TItem, int>> selector)
        => throw new NotImplementedException();

    public virtual double? Average(Expression<Func<TItem, int?>> selector)
        => throw new NotImplementedException();

    public virtual float Average(Expression<Func<TItem, float>> selector)
        => throw new NotImplementedException();

    public virtual float? Average(Expression<Func<TItem, float?>> selector)
        => throw new NotImplementedException();

    public virtual double Average(Expression<Func<TItem, long>> selector)
        => throw new NotImplementedException();

    public virtual double? Average(Expression<Func<TItem, long?>> selector)
        => throw new NotImplementedException();

    public virtual double Average(Expression<Func<TItem, double>> selector)
        => throw new NotImplementedException();

    public virtual double? Average(Expression<Func<TItem, double?>> selector)
        => throw new NotImplementedException();

    public virtual decimal Average(Expression<Func<TItem, decimal>> selector)
        => throw new NotImplementedException();

    public virtual decimal? Average(Expression<Func<TItem, decimal?>> selector)
        => throw new NotImplementedException();

    public virtual TItem Aggregate(Expression<Func<TItem, TItem, TItem>> func)
        => throw new NotImplementedException();

    public virtual TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func)
        => throw new NotImplementedException();

    public virtual TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
        => throw new NotImplementedException();

    public virtual IReadOnlyList<TItem> ToArray()
        => throw new NotImplementedException();

    public virtual IReadOnlyList<TResult> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping)
        => throw new NotImplementedException();

    public virtual IList<TItem> ToList()
        => throw new NotImplementedException();

    public virtual IList<TResult> ToList<TResult>(Expression<Func<TItem, TResult>> mapping)
        => throw new NotImplementedException();

    public virtual ISet<TItem> ToHashSet()
        => throw new NotImplementedException();

    public virtual ISet<TResult> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping)
        => throw new NotImplementedException();

    public virtual IRepository<TResult> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping)
        where TResult : class
        => throw new NotImplementedException();

    public virtual IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(Expression<Func<TItem, TKey>> selectKey, Expression<Func<TItem, TValue>> selectValue)
        where TKey : notnull
        => throw new NotImplementedException();

    public virtual void Add(TItem newItem)
        => throw new NotImplementedException();

    public virtual void Update(Expression<Func<TItem, bool>> predicate, TItem updatedItem)
        => throw new NotImplementedException();

    public virtual void Remove(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();
}
