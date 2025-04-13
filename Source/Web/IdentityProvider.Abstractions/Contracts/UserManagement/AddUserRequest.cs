namespace WebApi.Contracts.UserManagement;

/// <summary>
/// Represents the data required to add a new user.
/// </summary>
public record AddUserRequest
    : Request {
    /// <summary>
    /// Gets or initializes the primary identifier for the user.
    /// Must be unique.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public required string Identifier { get; init; }

    /// <summary>
    /// Gets or initializes the user's email address.
    /// Must be unique.
    /// </summary>
    [EmailAddress]
    [Required(AllowEmptyStrings = false)]
    public required string Email { get; init; }

    /// <summary>
    /// Gets or initializes the user's initial password.
    /// Password complexity requirements are enforced by the Identity service options.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public required string Password { get; init; }

    /// <summary>
    /// Validates the request data based on attributes and basic checks.
    /// </summary>
    /// <param name="context">Validation context (optional).</param>
    /// <returns>A Result indicating success or containing validation errors.</returns>
    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Identifier))
            result += new Error("The identifier is required.", nameof(Identifier));
        if (string.IsNullOrWhiteSpace(Email))
            result += new Error("The email is required.", nameof(Email));
        if (string.IsNullOrWhiteSpace(Password))
            result += new Error("Password is required.", nameof(Password));
        return result;
    }
}
