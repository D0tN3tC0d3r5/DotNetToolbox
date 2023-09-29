namespace System;

public record HasDefault<T> : IHaveSingleton<T> where T : HasDefault<T>, new()
{
    public static T Default => IHaveSingleton<T>.Singleton;
}
