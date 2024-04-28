// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => source.AnyAsync(_ => true, cancellationToken);

    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            if (predicate(item))
                return true;
        }

        return false;
    }
}
