namespace System.Singleton;

public interface IEmpty<out TSelf>
    where TSelf : IEmpty<TSelf>? {

    static abstract TSelf Empty { get; }
}