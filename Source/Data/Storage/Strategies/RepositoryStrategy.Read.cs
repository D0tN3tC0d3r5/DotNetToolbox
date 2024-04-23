namespace DotNetToolbox.Data.Strategies;

public abstract partial class RepositoryStrategy<TItem> {
    public virtual TItem[] ToArray()
        => throw new NotImplementedException();

    public virtual TResult[] ToArray<TResult>(Expression<Func<TItem, TResult>> mapping)
        => throw new NotImplementedException();

    public virtual List<TItem> ToList()
        => throw new NotImplementedException();

    public virtual List<TResult> ToList<TResult>(Expression<Func<TItem, TResult>> mapping)
        => throw new NotImplementedException();

    public virtual HashSet<TItem> ToHashSet()
        => throw new NotImplementedException();

    public virtual HashSet<TResult> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping)
        => throw new NotImplementedException();

    public virtual Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null)
        where TKey : notnull
        => throw new NotImplementedException();

    public virtual TItem First()
        => throw new NotImplementedException();

    public virtual TItem First(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem? FirstOrDefault()
        => throw new NotImplementedException();

    public virtual TItem FirstOrDefault(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem? FirstOrDefault(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem Last()
        => throw new NotImplementedException();

    public virtual TItem Last(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem? LastOrDefault()
        => throw new NotImplementedException();

    public virtual TItem LastOrDefault(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem? LastOrDefault(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem Single()
        => throw new NotImplementedException();

    public virtual TItem Single(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem? SingleOrDefault()
        => throw new NotImplementedException();

    public virtual TItem SingleOrDefault(TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem? SingleOrDefault(Expression<Func<TItem, bool>> predicate)
        => throw new NotImplementedException();

    public virtual TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue)
        => throw new NotImplementedException();

    public virtual TItem ElementAt(int index)
        => throw new NotImplementedException();

    public virtual TItem ElementAt(Index index)
        => throw new NotImplementedException();

    public virtual TItem? ElementAtOrDefault(int index)
        => throw new NotImplementedException();

    public virtual TItem? ElementAtOrDefault(Index index)
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

    public virtual TItem? Min()
        => throw new NotImplementedException();

    public virtual TResult? Min<TResult>(Expression<Func<TItem, TResult>> selector)
        => throw new NotImplementedException();

    public virtual TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual TItem? MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
        => throw new NotImplementedException();

    public virtual TItem? Max()
        => throw new NotImplementedException();

    public virtual TResult? Max<TResult>(Expression<Func<TItem, TResult>> selector)
        => throw new NotImplementedException();

    public virtual TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector)
        => throw new NotImplementedException();

    public virtual TItem? MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer)
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
}
