namespace WebApi.Tokens;

/// <summary>
/// Represents an access token used for authentication. Inherits from TemporaryToken and requires a string value for
/// initialization.
/// </summary>
public record AccessToken
    : TemporaryToken
    , IAccessToken
    , IRenewableTokenFactory<AccessToken> {
    public AccessToken() {
    }

    [SetsRequiredMembers]
    public AccessToken(string value, DateTimeOffset validUntil, DateTimeOffset? renewableUntil = null, DateTimeOffset? delayStartUntil = null, DateTimeOffset? createdAt = null, TimeProvider? clock = null)
        : base(AuthTokenType.Access, value, validUntil, delayStartUntil, createdAt, clock) {
        RenewableUntil = renewableUntil;
    }

    public DateTimeOffset? RenewableUntil { get; init; }

    public static AccessToken Create(string value, DateTimeOffset validUntil, DateTimeOffset? renewableUntil = null, DateTimeOffset? delayStartUntil = null, DateTimeOffset? createdAt = null, TimeProvider? clock = null)
        => new(value, validUntil, renewableUntil, delayStartUntil, createdAt, clock);
}