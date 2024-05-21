using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System;

public static class InstanceFactory {
    private const BindingFlags _allConstructors = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance;

    [return: NotNull]
    public static T Create<[DynamicallyAccessedMembers(PublicConstructors | NonPublicConstructors)] T>(params object?[] args)
        where T : class {
        try {
            return (T)IsNotNull(Activator.CreateInstance(typeof(T), _allConstructors, null, args, null, null), typeof(T).Name);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to create instance of type {typeof(T).Name}", ex);
        }
    }

    [return: NotNull]
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
