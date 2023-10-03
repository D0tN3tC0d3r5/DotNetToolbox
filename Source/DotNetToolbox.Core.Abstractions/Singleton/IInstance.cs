namespace System.Singleton;

public interface IInstance<out TSelf>
    where TSelf : IInstance<TSelf>?, new() {
    static abstract TSelf Instance { get; }
}