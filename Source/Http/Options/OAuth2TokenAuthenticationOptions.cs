namespace DotNetToolbox.Http.Options;

public class OAuth2TokenAuthenticationOptions : AuthenticationOptions {
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Authority { get; set; }
    public string[] Scopes { get; set; } = [];
    public Guid CorrelationId { get; private set; }

    internal IMsalHttpClientFactory? HttpClientFactory { get; set; }

    public override Result Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate(context);

        if (string.IsNullOrWhiteSpace(ClientId))
            result += new ValidationError(ValueCannotBeNullOrWhiteSpace, GetSourcePath(nameof(ClientId)));

        if (string.IsNullOrEmpty(ClientSecret))
            result += new ValidationError(ValueCannotBeNullOrEmpty, GetSourcePath(nameof(ClientSecret)));

        if (Scopes.Length == 0)
            result += new ValidationError(ValueCannotBeNullOrEmpty, GetSourcePath(nameof(Scopes)));

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }

    internal override HttpAuthentication Configure(HttpClient client, HttpAuthentication authentication) {
        if (!authentication.IsValid(OAuth2)) authentication = AcquireOauth2Token();
        client.DefaultRequestHeaders.Authorization = authentication;
        return authentication;
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
