namespace DotNetToolbox.Data.Strategies.Key;

public sealed class ClientProvidedKeyHandler<TKey>(IEqualityComparer<TKey>? comparer = null)
    : KeyHandler<TKey>(comparer);
