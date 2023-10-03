namespace System.Singleton;

public interface IDefault<out TSelf>
    where TSelf : IDefault<TSelf>?, new() {
    static abstract TSelf Default { get; }
}