namespace System;

public static class Creator {
    public static T CreateInstance<T>(params object?[]? args)
        => (T)Activator.CreateInstance(typeof(T), args)!;
}
