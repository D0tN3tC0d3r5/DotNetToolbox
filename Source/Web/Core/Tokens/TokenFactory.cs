namespace WebApi.Tokens;

public class TokenFactory(TimeProvider clock) :
    ITokenFactory {
    public TemporaryToken CreateTemporaryToken(TemporaryTokenOptions options, string type, string value, uint delayStartBySeconds = 0)
        => new(type, value, options.LifetimeInSeconds, delayStartBySeconds, clock);

    public AccessToken CreateAccessToken(AccessTokenOptions options, ClaimsIdentity subject, uint delayStartBySeconds = 0) {
        var now = clock.GetUtcNow();
        var keyBytes = Convert.FromBase64String(options.Key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var delayStartUntil = now.AddSeconds(delayStartBySeconds);
        var expiration = delayStartUntil.AddSeconds(options.LifetimeInSeconds);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = subject,
            Issuer = options.Issuer,
            Audience = options.Audience,
            IssuedAt = now.UtcDateTime,
            NotBefore = delayStartUntil.UtcDateTime,
            Expires = expiration.UtcDateTime,
            SigningCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
        var canRefreshUntil = options.RefreshDurationInSeconds > options.LifetimeInSeconds
                ? delayStartUntil.AddSeconds(options.RefreshDurationInSeconds)
                : (DateTimeOffset?)null;
        return new(tokenHandler.WriteToken(jwtToken), expiration, canRefreshUntil, delayStartUntil, now);
    }
}