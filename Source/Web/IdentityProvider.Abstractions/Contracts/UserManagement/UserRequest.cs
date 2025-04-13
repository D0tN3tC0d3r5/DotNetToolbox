namespace WebApi.Contracts.UserManagement;

/// <summary>
/// Represents a request for a single user.
/// </summary>
public record UserRequest
    : Request {
    /// <summary>
    /// Gets or initializes the primary identifier for the user.
    /// Must be unique.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public required string Identifier { get; init; }

    /// <summary>
    /// Validates the request data.
    /// </summary>
    /// <param name="context">Validation context (optional).</param>
    /// <returns>A Result indicating success or containing validation errors.</returns>
    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Identifier))
            result += new Error("The identifier is required.", nameof(Identifier));
        return result;
    }
}
