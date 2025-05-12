// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static async ValueTask<bool> AllAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var negatedPredicate = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(predicate.Body), predicate.Parameters);
        var enumerable = IsNotNull(source).Where(negatedPredicate).AsAsyncEnumerable(ct);
        await foreach (var _ in enumerable) {
            ct.ThrowIfCancellationRequested();
            return false;
        }
        return true;
    }
}
