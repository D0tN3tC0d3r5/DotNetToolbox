﻿// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> MinAsync<TItem>(
            this IQueryable<TItem> source,
            CancellationToken ct = default)
        => source.MinAsync(x => x, ct);

    public static async ValueTask<TResult> MinAsync<TItem, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TResult> selector,
            CancellationToken ct = default)
        => await source.MinByAsync(selector, Comparer<TResult>.Default, selector, ct)
        ?? throw new InvalidOperationException("Collection contains no elements.");

    public static ValueTask<TItem> MinAsync<TItem>(
            this IQueryable<TItem> source,
            IComparer<TItem> itemComparer,
            CancellationToken ct = default)
        => source.MinAsync(x => x, itemComparer, ct);

    public static ValueTask<TResult> MinAsync<TItem, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TResult> selector,
            IComparer<TResult> valueComparer,
            CancellationToken ct = default)
        => source.MinByAsync(selector, valueComparer, selector, ct);
}
