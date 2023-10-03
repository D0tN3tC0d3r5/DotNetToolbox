namespace System.Singleton;

public static class SingletonFactory<TSelf>
    where TSelf : new() {
    public static TSelf Singleton { get; } = new();
}