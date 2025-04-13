namespace WebApi.Tokens;

public interface IRenewableTokenFactory<out TToken>
    where TToken : IRenewableTokenFactory<TToken> {
    static abstract TToken Create(string value, DateTimeOffset validUntil, DateTimeOffset? renewableUntil = null, DateTimeOffset? delayStartUntil = null, DateTimeOffset? createdAt = null, TimeProvider? clock = null);
}
