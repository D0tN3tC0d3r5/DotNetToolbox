namespace WebApi.Contracts.UserManagement;

/// <summary>
/// Represents the data returned when querying user information.
/// Excludes sensitive information like passwords.
/// </summary>
public sealed record UserResponse
    : Response {
    /// <summary>
    /// Gets the primary identifier (e.g., username) of the user.
    /// </summary>
    public required string Identifier { get; init; }

    /// <summary>
    /// Gets whether the user's account has been confirmed (e.g., via email).
    /// </summary>
    public bool AccountIsConfirmed { get; init; }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Gets whether the user's account has been confirmed (e.g., via email).
    /// </summary>
    public bool EmailIsConfirmed { get; init; }

    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Gets whether the user's account has been confirmed (e.g., via email).
    /// </summary>
    public bool PhoneNumberIsConfirmed { get; init; }

    /// <summary>
    /// Gets the type of two-factor authentication enabled for the user.
    /// </summary>
    public TwoFactorAuthenticationType TwoFactorAuthenticationType { get; init; }

    /// <summary>
    /// Gets whether the user's account is currently locked out.
    /// </summary>
    public bool IsLocked { get; init; }

    /// <summary>
    /// Gets whether the user's account is currently locked out.
    /// </summary>
    public bool IsBlocked { get; init; }

    /// <summary>
    /// A dictionary that holds key-value pairs of claims, where both keys and values are strings.
    /// </summary>
    public Dictionary<string, string> Claims { get; init; } = [];

    /// <summary>
    /// Gets the roles assigned to the user (e.g., "User", "Administrator").
    /// </summary>
    public string[] Roles { get; init; } = [];

    /// <summary>
    /// Represents a list of available login providers for authentication.
    /// </summary>
    public string[] LoginProviders { get; init; } = [];
}
