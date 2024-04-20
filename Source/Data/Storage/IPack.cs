namespace DotNetToolbox.Data;

public interface IPack<out TFirst, out TSecond, out TThird>
    : IPack<TFirst, TSecond> {
    TThird Third { get; }
}

public interface IPack<out TFirst, out TSecond> {
    TFirst First { get; }
    TSecond Second { get; }
}
