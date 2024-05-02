// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static async Task LoadAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default) {
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
        }
    }
}
