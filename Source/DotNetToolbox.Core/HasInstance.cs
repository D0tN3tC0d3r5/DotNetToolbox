namespace System;

public record HasInstance<T> : IHaveSingleton<T> where T : HasInstance<T>, new()
{
    public static T Instance => IHaveSingleton<T>.Singleton;
}
