namespace DotNetToolbox.Singleton;

public interface IInstance<out TSelf>
    where TSelf : IInstance<TSelf>? {
    static abstract TSelf Instance { get; }
}