namespace DotNetToolbox.Singleton;

public interface IDefault<out TSelf>
    where TSelf : IDefault<TSelf>? {
    static abstract TSelf Default { get; }
}