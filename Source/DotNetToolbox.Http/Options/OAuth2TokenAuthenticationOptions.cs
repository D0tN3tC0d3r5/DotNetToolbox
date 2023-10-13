namespace DotNetToolbox.Http.Options;

public class OAuth2TokenAuthenticationOptions : AuthenticationOptions {
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Authority { get; set; }
    public string[] Scopes { get; set; } = Array.Empty<string>();
    public Guid CorrelationId { get; private set; }

    internal IMsalHttpClientFactory? HttpClientFactory { get; set; }

    internal override ValidationResult Validate() {
        var result = base.Validate();

        if (string.IsNullOrWhiteSpace(ClientId))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(ClientId));

        if (string.IsNullOrEmpty(ClientSecret))
            result += new ValidationError(CannotBeNullOrEmpty, nameof(ClientSecret));

        if (Scopes.Length == 0)
            result += new ValidationError(CannotBeEmpty, nameof(Scopes));

        return result;
    }

    internal override void Configure(HttpClient client, ref HttpAuthentication authentication) {
        if (!authentication.IsValid(OAuth2)) authentication = AcquireOauth2Token();
        client.DefaultRequestHeaders.Authorization = authentication;
    }

    internal DateTimeProvider DateTimeProvider { get; set; } = new();

    private HttpAuthentication AcquireOauth2Token() {
        try {
            var result = AuthenticateClient();
            return new() {
                DateTimeProvider = DateTimeProvider,
                Type = OAuth2,
                Value = result.AccessToken,
                Scheme = Bearer,
                ExpiresOn = result.ExpiresOn.DateTime,
            };
        }
        catch (Exception ex) {
            throw new InvalidOperationException("Failed to set authorization header.", ex);
        }
    }

    internal AuthenticationResult? AuthenticationResult { get; set; }

    [ExcludeFromCodeCoverage]
    private AuthenticationResult AuthenticateClient()
        => AuthenticationResult
        ?? CreateApplication()
        .AcquireTokenForClient(Scopes)
        .WithCorrelationId(CorrelationId)
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
        CorrelationId = Guid.NewGuid();
        return builder.Build();
    }
}