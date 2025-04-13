namespace WebApi.Tokens;

/// <summary>
/// Represents a token with a specific type, value
/// </summary>
public interface IToken {
    /// <summary>
    /// Represents a unique identifier for the token.
    /// </summary>
    Guid Id { get; }
    /// <summary>
    /// Represents the type the token as a string.
    /// </summary>
    string Type { get; }
    /// <summary>
    /// Represents the value of the token.
    /// </summary>
    string Value { get; }
}