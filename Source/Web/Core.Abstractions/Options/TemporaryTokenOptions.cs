namespace WebApi.Options;

public record TemporaryTokenOptions
    : TokenOptions {
    public const uint DefaultLifetimeInSeconds = 30 * 60; // 30 minutes

    public TemporaryTokenOptions() {
    }

    protected TemporaryTokenOptions(uint lifetimeInSeconds) {
        LifetimeInSeconds = lifetimeInSeconds;
    }

    public uint LifetimeInSeconds { get; set; } = DefaultLifetimeInSeconds;
}
