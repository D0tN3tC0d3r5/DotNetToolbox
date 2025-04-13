namespace WebApi.Tokens;

/// <summary>
/// Represents an access token that is owned by another entity.
/// </summary>
public record OwnedAccessToken<TOwner>
    : AccessToken
    where TOwner : class {
    public OwnedAccessToken() {
    }

    public TOwner Owner { get; init; } = null!;

    [SetsRequiredMembers]
    public OwnedAccessToken(TOwner owner, string value, DateTimeOffset validUntil, DateTimeOffset? renewableUntil = null, DateTimeOffset? delayStartUntil = null, DateTimeOffset? createdAt = null, TimeProvider? clock = null)
        : base(value, validUntil, renewableUntil, delayStartUntil, createdAt, clock) {
        Owner = Ensure.IsNotNull(owner);
    }
}
