// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System;

public static partial class Ensure {
    public static string IsNotNullOrEmpty([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
        return argument;
    }

    public static string IsNotNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null) {
        ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
        return argument;
    }

    public static string DefaultIfNullOrEmpty([NotNull] string? argument, string defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string? paramName = null) {
        argument = string.IsNullOrEmpty(argument) ? IsNotNullOrEmpty(defaultValue, paramName) : argument;
        return argument;
    }

    public static string DefaultIfNullOrWhiteSpace([NotNull] string? argument, string defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string? paramName = null) {
        argument = string.IsNullOrWhiteSpace(argument) ? IsNotNullOrWhiteSpace(defaultValue, paramName) : argument;
        return argument;
    }
}
