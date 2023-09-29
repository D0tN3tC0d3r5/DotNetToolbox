namespace System;

public record HasEmpty<T> : IHaveSingleton<T> where T : HasEmpty<T>, new()
{
    public static T Empty => IHaveSingleton<T>.Singleton;
}
