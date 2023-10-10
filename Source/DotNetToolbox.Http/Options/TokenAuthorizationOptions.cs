namespace DotNetToolbox.Http.Options;

public class TokenAuthorizationOptions : AuthorizationOptions {
    public TokenScheme Scheme { get; set; }
    public string? Token { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; }
    public DateTimeOffset? NotBefore { get; set; }

    public bool IsActive {
        get {
            var now = DateTimeOffset.UtcNow;
            return Token is not null
                && ((ExpiresOn == null) || (ExpiresOn > now))
                && ((NotBefore == null) || (NotBefore < now));
        }
    }

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);
        var now = DateTimeOffset.UtcNow;

        if (ExpiresOn <= now)
            result += new ValidationError("'{0}' has expired on '{1}'. Request time was '{2}'.", GetSource(httpClientName, nameof(ExpiresOn)), ExpiresOn, now);

        if (NotBefore >= now)
            result += new ValidationError("'{0}' is not active before '{1}'. Request time was '{2}'.", GetSource(httpClientName, nameof(NotBefore)), NotBefore, now);

        return result;
    }
}