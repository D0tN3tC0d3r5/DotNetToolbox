namespace DotNetToolbox.Http.Options;

public class OAuth2TokenAuthorizationOptions : TokenAuthorizationOptions {
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Authority { get; set; }
    public string[] Scopes { get; set; } = Array.Empty<string>();
    public IReadOnlyList<Claim> Claims { get; set; } = Array.Empty<Claim>();

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(ClientId))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(ClientId)));

        if (string.IsNullOrEmpty(ClientSecret))
            result += new ValidationError(CannotBeNullOrEmpty, GetSource(httpClientName, nameof(ClientSecret)));

        return result;
    }
}