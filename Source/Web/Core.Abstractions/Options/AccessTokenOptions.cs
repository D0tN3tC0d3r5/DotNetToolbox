namespace WebApi.Options;

public record AccessTokenOptions
    : JsonWebTokenOptions {
    public const uint DefaultExtensionTimeInSeconds = 7 * 24 * 60 * 60; // 7 days

    public AccessTokenOptions()
        : base(DefaultLifetimeInSeconds) {
    }

    protected AccessTokenOptions(uint lifetimeInSeconds, uint refreshDurationInSeconds)
        : base(lifetimeInSeconds) {
        RefreshDurationInSeconds = refreshDurationInSeconds;
    }

    public uint RefreshDurationInSeconds { get; init; } = DefaultExtensionTimeInSeconds;
}