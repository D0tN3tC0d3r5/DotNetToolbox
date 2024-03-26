namespace DotNetToolbox.Collections.Generic;

public static class DictionaryExtensions {
    #region ForEach

    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> source, Action<TKey, TValue> action)
        where TKey : notnull
        => source.ToList()
                 .ForEach(i => action(i.Key, i.Value));

    #endregion
}
