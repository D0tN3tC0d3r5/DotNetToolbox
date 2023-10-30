namespace System;

public static class Create {
    public static T Instance<[DynamicallyAccessedMembers(PublicConstructors)] T>(params object[] args) {
        try {
            return (T)Activator.CreateInstance(typeof(T), args)!;
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }

    public static T Instance<[DynamicallyAccessedMembers(PublicConstructors)] T>(IServiceProvider services, params object[] args) {
        try {
            return ActivatorUtilities.CreateInstance<T> (services, args);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }
}