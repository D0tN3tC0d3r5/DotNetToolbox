// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static async Task LoadAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default) {
        await foreach (var item in source.AsConfigured(cancellationToken)) { }
    }
}
