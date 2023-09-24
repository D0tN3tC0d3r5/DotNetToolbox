namespace DotNetToolbox.Utilities;

public record HasDefault<T> : IHaveSingleton<T> where T : HasDefault<T>, new()
{
    public static T Default => IHaveSingleton<T>.Singleton;
}

public record HasEmpty<T> : IHaveSingleton<T> where T : HasEmpty<T>, new()
{
    public static T Empty => IHaveSingleton<T>.Singleton;
}

public interface IHaveSingleton<T>
    where T : new()
{
    protected static T Singleton { get; } = new();
}

public static class Create
{
    public static T Instance<T>(params object?[]? args)
        => (T)Activator.CreateInstance(typeof(T), args)!;
}
