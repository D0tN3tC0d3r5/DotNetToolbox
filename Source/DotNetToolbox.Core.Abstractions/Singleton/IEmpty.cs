namespace System.Singleton;

public interface IEmpty<out TSelf>
    where TSelf : IEmpty<TSelf>?, new() {

    static abstract TSelf Empty { get; }
}