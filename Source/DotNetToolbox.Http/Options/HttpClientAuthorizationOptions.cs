using static DotNetToolbox.Http.Options.HttpClientAuthorizationType;

namespace DotNetToolbox.Http.Options;

public class HttpClientAuthorizationOptions {
    public required HttpClientAuthorizationType Type { get; set; }

    public HttpClientAuthorizationScheme? Scheme { get; set; }
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

    public IReadOnlyList<Claim> Claims { get; set; } = Array.Empty<Claim>();

    public string? Issuer { get; set; }
    public string? Audience { get; set; }

    public string[] Scopes { get; set; } = Array.Empty<string>();
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? Secret { get; set; }
    public string? Authority { get; set; }

    internal ValidationResult Validate(string? httpClientName = null) {
        var result = Success();

        result += Type switch {
            ApiKey => ValidateForApiKey(httpClientName),
            SimpleToken => ValidateForSimpleToken(httpClientName),
            Jwt => ValidateForJsonWebToken(httpClientName),
            OAuth2 => ValidateForOAuth2(httpClientName),
            _ => Success(),
        };

        return result;
    }

    private ValidationResult ValidateForApiKey(string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(Token))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(Token)));

        return result;
    }

    private ValidationResult ValidateForJsonWebToken(string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(Secret))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(Secret)));

        return result;
    }

    private ValidationResult ValidateForOAuth2(string? httpClientName = null) {
        var result = Success();

        if (string.IsNullOrWhiteSpace(ClientId))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(ClientId)));

        if (string.IsNullOrEmpty(Secret))
            result += new ValidationError(CannotBeNullOrEmpty, GetSource(httpClientName, nameof(Secret)));

        return result;
    }

    private ValidationResult ValidateForSimpleToken(string? httpClientName = null) {
        var result = Success();
        var now = DateTimeOffset.UtcNow;

        if (string.IsNullOrWhiteSpace(Token))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(Token)));

        if (ExpiresOn <= now)
            result += new ValidationError("'{0}' has expired on '{1}'. Request time was '{2}'.", GetSource(httpClientName, nameof(Secret)), ExpiresOn, now);

        if (NotBefore >= now)
            result += new ValidationError("'{0}' is not active before '{1}'. Request time was '{2}'.", GetSource(httpClientName, nameof(Secret)), NotBefore, now);

        return result;
    }

    private static string GetSource(string? name, params string[] fields)
        => $"{(name is null ? string.Empty : $"{name}.")}"
         + $"{string.Join(".", fields)}";
}
