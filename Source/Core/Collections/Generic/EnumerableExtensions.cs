namespace DotNetToolbox.Collections.Generic;

public static class EnumerableExtensions {
    public static TItem[] ToArray<TItem>(this IEnumerable<TItem> source, Func<TItem, TItem> transform)
        => ToArray<TItem, TItem>(source, transform);
    public static TOutput[] ToArray<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => [..IsNotNull(source).Select(transform)];

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, Func<TValue, TValue> transformValue)
        where TKey : notnull
        => ToDictionary<TKey, TValue, TValue>(source, transformValue);
    public static Dictionary<TKey, TOutputValue> ToDictionary<TKey, TInputValue, TOutputValue>(this IEnumerable<KeyValuePair<TKey, TInputValue>> source, Func<TInputValue, TOutputValue> transformValue)
        where TKey : notnull
        => IsNotNull(source).ToDictionary(i => i.Key, i => transformValue(i.Value));

    public static Dictionary<TKey, TValue> ToDictionary<TInput, TKey, TValue>(this IEnumerable<TInput> source, Func<TInput, TInput> transformElement, Func<TInput, TKey> selectKey, Func<TInput, TValue> selectValue)
        where TKey : notnull
        => ToDictionary<TInput, TInput, TKey, TValue>(source, transformElement, selectKey, selectValue);
    public static Dictionary<TKey, TValue> ToDictionary<TInput, TOutput, TKey, TValue>(this IEnumerable<TInput> source, Func<TInput, TOutput> transformElement, Func<TOutput, TKey> selectKey, Func<TOutput, TValue> selectValue)
        where TKey : notnull
        => IsNotNull(source).Select(transformElement).ToDictionary(selectKey, selectValue);

    public static HashSet<TItem> ToHashSet<TItem>(this IEnumerable<TItem> source, Func<TItem, TItem> transform)
        => ToHashSet<TItem, TItem>(source, transform);
    public static HashSet<TOutput> ToHashSet<TItem, TOutput>(this IEnumerable<TItem> source, Func<TItem, TOutput> transform)
        => [..IsNotNull(source).Select(transform)];
}
