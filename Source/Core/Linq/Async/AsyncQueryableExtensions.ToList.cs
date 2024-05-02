﻿// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<List<TItem>> ToListAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => source.ToListAsync((x, _) => x, ct);

    public static ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default)
    => source.ToListAsync((x, _) => mapping(x), ct);

    public static async ValueTask<List<TResult>> ToListAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, int, TResult> mapping, CancellationToken ct = default) {
        var result = new List<TResult>();
        var index = 0;
        await foreach (var item in IsNotNull(source).AsAsyncQueryable().AsConfigured(ct))
            result.Add(mapping(item, index++));
        return result;
    }
}
