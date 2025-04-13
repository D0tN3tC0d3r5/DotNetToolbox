namespace WebApi.Utilities;

/// <summary>
/// Represents a value that may or may not be present.
/// Used to distinguish between a value explicitly set (potentially to null)
/// and a value that was not provided at all.
/// Inspired by functional programming Option/Maybe types.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly struct Optional<T> : IEquatable<Optional<T>> {
    private readonly T? _value; // Use nullable backing field for flexibility

    /// <summary>
    /// Gets the value if IsSet is true. Accessing when IsSet is false
    /// might throw or return default, depending on desired strictness.
    /// Using GetValueOrDefault() or pattern matching is safer.
    /// </summary>
    public T? Value => IsSet ? _value : default; // Return default if not set

    /// <summary>
    /// Gets a value indicating whether a value is present.
    /// </summary>
    public bool IsSet { get; }

    // Private constructor for internal use (including default)
    private Optional(T? value, bool isSet) {
        _value = value;
        IsSet = isSet;
    }

    /// <summary>
    /// Represents the absence of a value.
    /// </summary>
    public static Optional<T> None => new(default, false); // Default struct has IsSet = false

    /// <summary>
    /// Creates an Optional instance representing a present value (Return operation).
    /// </summary>
    /// <param name="value">The value to wrap (can be null for reference types).</param>
    /// <returns>An Optional instance with IsSet = true.</returns>
    public static Optional<T> Some(T? value) => new(value, true);

    // Allow implicit conversion from T to Optional<T> for convenience
    public static implicit operator Optional<T>(T? value) => Some(value);

    /// <summary>
    /// Gets the value if present; otherwise, returns a default value.
    /// </summary>
    /// <param name="defaultValue">The value to return if IsSet is false.</param>
    /// <returns>The value or the default value.</returns>
    public T? GetValueOrDefault(T? defaultValue = default) => IsSet ? _value : defaultValue;

    // --- Equality and Overrides ---
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Optional<T> other && Equals(other);

    public bool Equals(Optional<T> other)
        => IsSet == other.IsSet
        && (!IsSet || EqualityComparer<T?>.Default.Equals(_value, other._value));

    public override int GetHashCode() => IsSet ? HashCode.Combine(IsSet, _value) : IsSet.GetHashCode();

    public static bool operator ==(Optional<T> left, Optional<T> right) => left.Equals(right);
    public static bool operator !=(Optional<T> left, Optional<T> right) => !left.Equals(right);

    public override string ToString() => IsSet ? $"Some({_value?.ToString() ?? "null"})" : "None";
}