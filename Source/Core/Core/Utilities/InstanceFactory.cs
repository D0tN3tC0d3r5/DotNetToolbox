namespace DotNetToolbox.Utilities;

public static class InstanceFactory {
    private const BindingFlags _allConstructors = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;

    public static T Create<[DynamicallyAccessedMembers(PublicConstructors | NonPublicConstructors)] T>(params object?[] args)
        where T : class {
        try {
            return (T)IsNotNull(Activator.CreateInstance(typeof(T), _allConstructors, null, args, null, null), typeof(T).Name);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }

    public static T Create<[DynamicallyAccessedMembers(PublicConstructors)] T>(IServiceProvider services, params object?[] args)
        where T : class {
        try {
            return ActivatorUtilities.CreateInstance<T>(services, args!);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }
}
