namespace DotNetToolbox.Data.Repositories;

public record Pack<TFirst, TSecond>(TFirst First, TSecond Second)
    : IPack<TFirst, TSecond>;

public record Pack<TFirst, TSecond, TThird>(TFirst First, TSecond Second, TThird Third)
    : Pack<TFirst, TSecond>(First, Second), IPack<TFirst, TSecond, TThird>;
