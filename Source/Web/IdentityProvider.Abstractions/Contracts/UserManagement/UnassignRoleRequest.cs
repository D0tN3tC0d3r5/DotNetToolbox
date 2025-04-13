namespace WebApi.Contracts.UserManagement;

/// <summary>
/// Represents the data required to assign a role to a user.
/// </summary>
public sealed record UnassignRoleRequest
    : Request {
    /// <summary>
    /// Gets or initializes the name of the role to assign (e.g., "User", "Administrator").
    /// Must match one of the predefined roles.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public required string Role { get; init; }

    /// <summary>
    /// Validates the request data.
    /// </summary>
    /// <param name="context">Validation context (optional).</param>
    /// <returns>A Result indicating success or containing validation errors.</returns>
    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Role))
            result += new Error("The identifier is required.", nameof(Role));
        return result;
    }
}
