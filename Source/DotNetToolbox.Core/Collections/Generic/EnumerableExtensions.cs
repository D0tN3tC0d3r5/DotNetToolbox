namespace System.Collections.Generic;

public static class EnumerableExtensions
{
    public static TItem[] ToArray<TItem>(this IEnumerable<TItem> source, Func<TItem, TItem> transform)
        => source?.ToArray<TItem, TItem>(transform) ?? Array.Empty<TItem>();

    public static TOutput[] ToArray<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => source?.Select(transform).ToArray() ?? Array.Empty<TOutput>();
}
