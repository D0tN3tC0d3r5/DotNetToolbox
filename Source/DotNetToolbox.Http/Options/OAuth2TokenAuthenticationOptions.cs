namespace DotNetToolbox.Http.Options;

public record OAuth2TokenAuthenticationOptions : AuthenticationOptions {
    public OAuth2TokenAuthenticationOptions() {
    }

    [SetsRequiredMembers]
    public OAuth2TokenAuthenticationOptions(IConfiguration config)
        : this() {
        TenantId = config.GetValue<string>(nameof(TenantId));
        ClientId = config.GetValue<string>(nameof(ClientId));
        ClientSecret = config.GetValue<string>(nameof(ClientSecret));
        Authority = config.GetValue<string>(nameof(Authority));
    }

    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Authority { get; set; }
    public string[] Scopes { get; set; } = Array.Empty<string>();
    internal IMsalHttpClientFactory? HttpClientFactory { get; set; }

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

    internal override void Configure(HttpClient client, ref HttpClientAuthentication authentication) {
        if (!authentication.IsValid(OAuth2)) authentication = AcquireOauth2Token();
        client.DefaultRequestHeaders.Authorization = authentication;
    }

    private HttpClientAuthentication AcquireOauth2Token() {
        try {
            var result = AuthenticateClient();
            return new() {
                Type = OAuth2,
                Value = result.AccessToken,
                Scheme = Bearer,
                ExpiresOn = result.ExpiresOn.ToUniversalTime().DateTime
            };
        }
        catch (Exception ex) {
            throw new InvalidOperationException("Failed to set authorization header.", ex);
        }
    }

    internal AuthenticationResult? AuthenticationResult { get; set; }

    [ExcludeFromCodeCoverage]
    private AuthenticationResult AuthenticateClient()
        => AuthenticationResult ?? CreateApplication()
                                  .AcquireTokenForClient(Scopes)
                                  .ExecuteAsync(CancellationToken.None)
                                  .Result;

    private IConfidentialClientApplication CreateApplication() {
        var builder = ConfidentialClientApplicationBuilder
                     .Create(ClientId)
                     .WithHttpClientFactory(HttpClientFactory)
                     .WithClientSecret(ClientSecret);
        if (!string.IsNullOrWhiteSpace(TenantId))
            builder.WithTenantId(TenantId);
        if (!string.IsNullOrWhiteSpace(Authority))
            builder.WithAuthority(Authority);
        return builder.Build();
    }
}