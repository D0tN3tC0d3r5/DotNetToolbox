namespace System;

public interface IHaveSingleton<T>
    where T : new()
{
    protected static T Singleton { get; } = new();
}