// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<bool> AllAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (!predicate(item))
                return false;
        }

        return true;
    }
}
