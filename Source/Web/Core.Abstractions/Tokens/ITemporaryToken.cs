namespace WebApi.Tokens;

/// <summary>
/// Represents a temporary token with creation, start, and expiration timestamps. Inherits from the Token class.
/// </summary>
public interface ITemporaryToken
    : IToken {
    /// <summary>
    /// Represents the date and time when the token was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; init; }
    /// <summary>
    /// Represents the date and time when the token will be active.
    /// </summary>
    DateTimeOffset DelayStartUntil { get; init; }
    /// <summary>
    /// Represents the date and time when the token expires.
    /// </summary>
    DateTimeOffset ValidUntil { get; init; }
}

/// <summary>
/// Represents a temporary token that can be refreshed.
/// </summary>
public interface IAccessToken
    : ITemporaryToken {
    /// <summary>
    /// Represents the date and time until which a refresh operation can occur.
    /// </summary>
    DateTimeOffset? RenewableUntil { get; init; }
}