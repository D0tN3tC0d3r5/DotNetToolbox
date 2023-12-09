namespace DotNetToolbox.Collections.Generic;

public static class EnumerableExtensions {
    public static TItem[] ToArray<TItem>(this IEnumerable<TItem> source, Func<TItem, TItem> transform)
        => IsNotNull(source).ToArray<TItem, TItem>(transform);

    public static TOutput[] ToArray<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => IsNotNull(source).Select(transform).ToArray();

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull
        => IsNotNull(source).ToDictionary<TKey, TValue>(i => i);

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, Func<TValue, TValue> transform)
        where TKey : notnull
        => IsNotNull(source).ToDictionary<TKey, TValue, TValue>(transform);

    public static Dictionary<TKey, TOutputValue> ToDictionary<TKey, TInputValue, TOutputValue>(this IEnumerable<KeyValuePair<TKey, TInputValue>> source, Func<TInputValue, TOutputValue> transform)
        where TKey : notnull
        => IsNotNull(source).ToDictionary(i => i.Key, i => transform(i.Value));
}
