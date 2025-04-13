namespace WebApi.Contracts;

/// <summary>
/// Represents the response containing a renewable token.
/// </summary>
public record RenewableTokenResponse
    : Response {
    public required Guid Id { get; init; }
    public required string Value { get; init; }
    public DateTimeOffset? Expiration { get; init; }
    public DateTimeOffset? Start { get; init; }
    public DateTimeOffset? RenewalEnd { get; init; }
}
