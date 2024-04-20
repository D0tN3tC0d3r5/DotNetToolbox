namespace DotNetToolbox.Data.Repositories;

public interface IAsyncReadOnlyRepository<TItem>
    where TItem : class {
    Task<IReadOnlyList<TItem>> ToArray(CancellationToken ct = default);

    Task<IReadOnlyList<TResult>> ToArray<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);

    Task<IList<TItem>> ToList(CancellationToken ct = default);

    Task<IList<TResult>> ToList<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);

    Task<ISet<TItem>> ToHashSet(CancellationToken ct = default);

    Task<ISet<TResult>> ToHashSet<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default);

    Task<TResultRepository> ToRepository<TResultRepository, TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResultRepository : class, IRepository<TResult>
        where TResult : class;

    Task<IRepository<TResult>> ToRepository<TResult>(Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        where TResult : class;

    Task<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(Func<TItem, TKey> selectKey, Func<TItem, TValue> selectValue, IEqualityComparer<TKey>? comparer = null, CancellationToken ct = default)
        where TKey : notnull;

    Task<TItem> First(CancellationToken ct = default);

    Task<TItem> First(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem?> FirstOrDefault(CancellationToken ct = default);

    Task<TItem> FirstOrDefault(TItem defaultValue, CancellationToken ct = default);

    Task<TItem?> FirstOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem> FirstOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default);

    Task<TItem> Last(CancellationToken ct = default);

    Task<TItem> Last(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem?> LastOrDefault(CancellationToken ct = default);

    Task<TItem> LastOrDefault(TItem defaultValue, CancellationToken ct = default);

    Task<TItem?> LastOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem> LastOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default);

    Task<TItem> Single(CancellationToken ct = default);

    Task<TItem> Single(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem?> SingleOrDefault(CancellationToken ct = default);

    Task<TItem> SingleOrDefault(TItem defaultValue, CancellationToken ct = default);

    Task<TItem?> SingleOrDefault(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem> SingleOrDefault(Expression<Func<TItem, bool>> predicate, TItem defaultValue, CancellationToken ct = default);

    Task<TItem> ElementAt(int index, CancellationToken ct = default);

    Task<TItem> ElementAt(Index index, CancellationToken ct = default);

    Task<TItem?> ElementAtOrDefault(int index, CancellationToken ct = default);

    Task<TItem?> ElementAtOrDefault(Index index, CancellationToken ct = default);

    Task<bool> Contains(TItem item, CancellationToken ct = default);

    Task<bool> Contains(TItem item, IEqualityComparer<TItem>? comparer, CancellationToken ct = default);

    Task<bool> SequenceEqual(IEnumerable<TItem> source2, CancellationToken ct = default);

    Task<bool> SequenceEqual(IEnumerable<TItem> source2, IEqualityComparer<TItem>? comparer, CancellationToken ct = default);

    Task<bool> Any(CancellationToken ct = default);

    Task<bool> Any(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<bool> All(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<int> Count(CancellationToken ct = default);

    Task<int> Count(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<long> LongCount(CancellationToken ct = default);

    Task<long> LongCount(Expression<Func<TItem, bool>> predicate, CancellationToken ct = default);

    Task<TItem?> Min(CancellationToken ct = default);

    Task<TResult?> Min<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default);

    Task<TItem?> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default);

    Task<TItem?> MinBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default);

    Task<TItem?> Max(CancellationToken ct = default);

    Task<TResult?> Max<TResult>(Expression<Func<TItem, TResult>> selector, CancellationToken ct = default);

    Task<TItem?> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, CancellationToken ct = default);

    Task<TItem?> MaxBy<TKey>(Expression<Func<TItem, TKey>> keySelector, IComparer<TItem>? comparer, CancellationToken ct = default);

    Task<int> Sum(Expression<Func<TItem, int>> selector, CancellationToken ct = default);

    Task<int?> Sum(Expression<Func<TItem, int?>> selector, CancellationToken ct = default);

    Task<long> Sum(Expression<Func<TItem, long>> selector, CancellationToken ct = default);

    Task<long?> Sum(Expression<Func<TItem, long?>> selector, CancellationToken ct = default);

    Task<float> Sum(Expression<Func<TItem, float>> selector, CancellationToken ct = default);

    Task<float?> Sum(Expression<Func<TItem, float?>> selector, CancellationToken ct = default);

    Task<double> Sum(Expression<Func<TItem, double>> selector, CancellationToken ct = default);

    Task<double?> Sum(Expression<Func<TItem, double?>> selector, CancellationToken ct = default);

    Task<decimal> Sum(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default);

    Task<decimal?> Sum(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default);

    Task<double> Average(Expression<Func<TItem, int>> selector, CancellationToken ct = default);

    Task<double?> Average(Expression<Func<TItem, int?>> selector, CancellationToken ct = default);

    Task<float> Average(Expression<Func<TItem, float>> selector, CancellationToken ct = default);

    Task<float?> Average(Expression<Func<TItem, float?>> selector, CancellationToken ct = default);

    Task<double> Average(Expression<Func<TItem, long>> selector, CancellationToken ct = default);

    Task<double?> Average(Expression<Func<TItem, long?>> selector, CancellationToken ct = default);

    Task<double> Average(Expression<Func<TItem, double>> selector, CancellationToken ct = default);

    Task<double?> Average(Expression<Func<TItem, double?>> selector, CancellationToken ct = default);

    Task<decimal> Average(Expression<Func<TItem, decimal>> selector, CancellationToken ct = default);

    Task<decimal?> Average(Expression<Func<TItem, decimal?>> selector, CancellationToken ct = default);

    Task<TItem> Aggregate(Expression<Func<TItem, TItem, TItem>> func, CancellationToken ct = default);

    Task<TAccumulate> Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, CancellationToken ct = default);

    Task<TResult> Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TItem, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector, CancellationToken ct = default);
}
