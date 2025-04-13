namespace WebApi.Contracts.Authentication;

/// <summary>
/// Represents a request to sign out a user.
/// </summary>
public sealed record SignOutRequest
    : Request {
    /// <summary>
    /// Represents a required unique user identifier.
    /// </summary>
    public required string Identifier { get; init; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Identifier))
            result += new Error("The identifier is required.", nameof(Identifier));
        return result;
    }
}
