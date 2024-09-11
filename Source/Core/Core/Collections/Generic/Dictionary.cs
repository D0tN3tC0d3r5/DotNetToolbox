// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

public static class Dictionary {
    public static Dictionary<TKey, TValue> Empty<TKey, TValue>()
        where TKey : notnull
        => new();
}
