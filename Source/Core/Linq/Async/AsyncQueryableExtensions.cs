// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    private static ConfiguredCancelableAsyncEnumerable<TItem> AsConfigured<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken)
        => IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false);
}
