namespace DotNetToolbox;

public static class Create {
    private const BindingFlags _allConstructors = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;

    public static T? Instance<[DynamicallyAccessedMembers(PublicConstructors | NonPublicConstructors)] T>(params object?[] args) {
        try {
            return (T?)Activator.CreateInstance(typeof(T), _allConstructors, null, args, null, null);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }

    public static T Instance<[DynamicallyAccessedMembers(PublicConstructors)] T>(IServiceProvider services, params object[] args)
        where T : class {
        try {
            return ActivatorUtilities.CreateInstance<T>(services, args);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }
}
