// ReSharper disable once CheckNamespace
namespace System.Collections.Generic;

[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>")]
public static class Dictionary {
    public static Dictionary<TKey, TValue> Empty<TKey, TValue>()
        where TKey : notnull
        => [];
}
