namespace WebApi.Tokens;

/// <inheritdoc cref="ITemporaryToken" />
public record TemporaryToken
    : Token
    , ITemporaryToken {
    public TemporaryToken() {
    }

    [SetsRequiredMembers]
    public TemporaryToken(string type, string value, DateTimeOffset validUntil, DateTimeOffset? delayStartUntil = null, DateTimeOffset? createdAt = null, TimeProvider? clock = null)
        : base(type, value) {
        CreatedAt = createdAt ?? (clock ?? TimeProvider.System).GetUtcNow();
        DelayStartUntil = delayStartUntil ?? CreatedAt;
        ValidUntil = Ensure.IsGreaterThan(validUntil, DelayStartUntil);
    }

    [SetsRequiredMembers]
    public TemporaryToken(string type, string value, uint lifetimeInSeconds, uint delayStartBySeconds = 0, TimeProvider? clock = null)
        : base(type, value) {
        CreatedAt = (clock ?? TimeProvider.System).GetUtcNow();
        DelayStartUntil = CreatedAt.AddSeconds(delayStartBySeconds);
        ValidUntil = DelayStartUntil.AddSeconds(Ensure.IsNotZero(lifetimeInSeconds));
    }

    /// <inheritdoc />
    public required DateTimeOffset CreatedAt { get; init; }

    /// <inheritdoc />
    public required DateTimeOffset DelayStartUntil { get; init; }

    /// <inheritdoc />
    public required DateTimeOffset ValidUntil { get; init; }
}