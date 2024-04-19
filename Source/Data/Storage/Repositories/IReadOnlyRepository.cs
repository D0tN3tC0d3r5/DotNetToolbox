namespace DotNetToolbox.Data.Repositories;

public interface IReadOnlyRepository<TItem>
    : IQueryableRepository<TItem>
    where TItem : class {
    IReadOnlyList<TItem> ToArray();

    IReadOnlyList<TResult> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping);

    IList<TItem> ToList();

    IList<TResult> ToList<TResult>(Expression<Func<TItem, TResult>> mapping);

    ISet<TItem> ToHashSet();

    ISet<TResult> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping);

    IRepository<TResult> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping)
        where TResult : class;

    IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(Expression<Func<TItem, TKey>> selectKey, Expression<Func<TItem, TValue>> selectValue)
        where TKey : notnull;

    TItem First();

    TItem First(Expression<Func<TItem, bool>> predicate);

    TItem FirstOrDefault();

    TItem FirstOrDefault(TItem defaultValue);

    TItem FirstOrDefault(Expression<Func<TItem, bool>> predicate);

    TItem FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue);

    TItem Last();

    TItem Last(Expression<Func<TItem, bool>> predicate);

    TItem LastOrDefault();

    TItem LastOrDefault(TItem defaultValue);

    TItem LastOrDefault(Expression<Func<TItem, bool>> predicate);

    TItem LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue);

    TItem Single();

    TItem Single(Expression<Func<TItem, bool>> predicate);

    TItem SingleOrDefault();

    TItem SingleOrDefault(TItem defaultValue);

    TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate);

    TItem SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue);

    TItem ElementAt(int index);

    TItem ElementAt(Index index);

    TItem ElementAtOrDefault(int index);

    TItem ElementAtOrDefault(Index index);

    bool Contains(TItem item);

    bool Contains(TItem item, IEqualityComparer<TItem>? comparer);

    bool SequenceEqual(IEnumerable<TItem> source2);

    bool SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer);

    bool Any();

    bool Any(Expression<Func<TItem, bool>> predicate);

    bool All(Expression<Func<TItem, bool>> predicate);

    int Count();

    int Count(Expression<Func<TItem, bool>> predicate);

    long LongCount();

    long LongCount(Expression<Func<TItem, bool>> predicate);

    TItem Min();

    TResult Min<TResult>(Expression<Func<TItem, TResult>> selector);

    TItem MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    TItem MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer);

    TItem Max();

    TResult Max<TResult>(Expression<Func<TItem, TResult>> selector);

    TItem MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector);

    TItem MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer);

    int Sum(Expression<Func<TItem, int>> selector);

    int? Sum(Expression<Func<TItem, int?>> selector);

    long Sum(Expression<Func<TItem, long>> selector);

    long? Sum(Expression<Func<TItem, long?>> selector);

    float Sum(Expression<Func<TItem, float>> selector);

    float? Sum(Expression<Func<TItem, float?>> selector);

    double Sum(Expression<Func<TItem, double>> selector);

    double? Sum(Expression<Func<TItem, double?>> selector);

    decimal Sum(Expression<Func<TItem, decimal>> selector);

    decimal? Sum(Expression<Func<TItem, decimal?>> selector);

    double Average(Expression<Func<TItem, int>> selector);

    double? Average(Expression<Func<TItem, int?>> selector);

    float Average(Expression<Func<TItem, float>> selector);

    float? Average(Expression<Func<TItem, float?>> selector);

    double Average(Expression<Func<TItem, long>> selector);

    double? Average(Expression<Func<TItem, long?>> selector);

    double Average(Expression<Func<TItem, double>> selector);

    double? Average(Expression<Func<TItem, double?>> selector);

    decimal Average(Expression<Func<TItem, decimal>> selector);

    decimal? Average(Expression<Func<TItem, decimal?>> selector);

    TItem Aggregate(Expression<Func<TItem, TItem, TItem>> func);

    TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func);

    TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector);
}