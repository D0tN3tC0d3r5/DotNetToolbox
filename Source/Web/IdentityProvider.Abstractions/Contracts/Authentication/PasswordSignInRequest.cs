namespace WebApi.Contracts.Authentication;

/// <summary>
/// Represents a request for password-based sign-in with an identifier and password.
/// </summary>
public sealed record PasswordSignInRequest
    : Request {
    /// <summary>
    /// Represents a required unique user identifier.
    /// </summary>
    public required string Identifier { get; init; }
    /// <summary>
    /// A required property that holds a string representing a user's password.
    /// </summary>
    public required string Password { get; init; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Identifier))
            result += new Error("The identifier is required.", nameof(Identifier));
        if (string.IsNullOrWhiteSpace(Password))
            result += new Error("Password is required.", nameof(Password));
        return result;
    }
}
