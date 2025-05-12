// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static async ValueTask<bool> AllAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var negatedPredicate = (Expression<Func<TItem, bool>>)Expression.Lambda(Expression.Not(predicate.Body), predicate.Parameters);
        await foreach (var _ in IsNotNull(source).Where(negatedPredicate)) {
            ct.ThrowIfCancellationRequested();
            return false;
        }
        return true;
    }
}
