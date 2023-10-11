namespace DotNetToolbox.Http.Options;

public class OAuth2TokenAuthorizationOptions : TokenAuthorizationOptions {
    public OAuth2TokenAuthorizationOptions() {
        Scheme = TokenScheme.Bearer;
    }

    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Authority { get; set; }
    public string[] Scopes { get; set; } = Array.Empty<string>();

    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(ClientId))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(ClientId)));

        if (string.IsNullOrEmpty(ClientSecret))
            result += new ValidationError(CannotBeNullOrEmpty, GetSource(httpClientName, nameof(ClientSecret)));

        if (Scopes.Length == 0)
            result += new ValidationError(CannotBeEmpty, GetSource(httpClientName, nameof(Scopes)));

        return result;
    }
}