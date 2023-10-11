namespace DotNetToolbox.Http.Options;

public class ConfigureHttpClientOptions : IConfigureOptions<HttpClientOptions> {
    private readonly IConfiguration _configuration;

    public ConfigureHttpClientOptions(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void Configure(HttpClientOptions options) {
        var section = _configuration.GetSection(nameof(HttpClientOptions));
        IsNotNull(section.Value, "Configuration[HttpClientOptions]");
        options.BaseAddress = IsNotNull(section.GetValue<string>(nameof(HttpClientOptions.BaseAddress)), "Configuration[HttpClientOptions].BaseAddress");
        options.ResponseFormat = section.GetValue<string>(nameof(HttpClientOptions.ResponseFormat)) ?? SimpleHttpClientOptions.DefaultResponseFormat;
        options.CustomHeaders = section.GetValue<Dictionary<string, string[]>>(nameof(HttpClientOptions.CustomHeaders)) ?? new();
        var authSection = section.GetSection(nameof(AuthorizationOptions));
        if (authSection.Value is null)
            return;
        var authType = authSection.GetValue<AuthorizationType>(nameof(AuthorizationOptions.Type));

        options.Authorization = authType switch {
            AuthorizationType.ApiKey => new ApiKeyAuthorizationOptions {
                Type = AuthorizationType.ApiKey,
                ApiKey = authSection.GetValue<string>(nameof(ApiKeyAuthorizationOptions.ApiKey)),
            },
            AuthorizationType.SimpleToken => new SimpleTokenAuthorizationOptions {
                Type = AuthorizationType.SimpleToken,
                Scheme = authSection.GetValue<TokenScheme>(nameof(SimpleTokenAuthorizationOptions.Scheme)),
                Token = authSection.GetValue<string>(nameof(SimpleTokenAuthorizationOptions.Token)),
                ExpiresOn = authSection.GetValue<DateTimeOffset>(nameof(SimpleTokenAuthorizationOptions.ExpiresOn)),
                NotBefore = authSection.GetValue<DateTimeOffset>(nameof(SimpleTokenAuthorizationOptions.NotBefore)),
            },
            AuthorizationType.JsonWebToken => new JsonWebTokenAuthorizationOptions {
                Type = AuthorizationType.JsonWebToken,
                Scheme = authSection.GetValue<TokenScheme>(nameof(SimpleTokenAuthorizationOptions.Scheme)),
                Token = authSection.GetValue<string>(nameof(SimpleTokenAuthorizationOptions.Token)),
                ExpiresOn = authSection.GetValue<DateTimeOffset>(nameof(SimpleTokenAuthorizationOptions.ExpiresOn)),
                NotBefore = authSection.GetValue<DateTimeOffset>(nameof(SimpleTokenAuthorizationOptions.NotBefore)),
                Audience = authSection.GetValue<string>(nameof(JsonWebTokenAuthorizationOptions.Audience)),
                Issuer = authSection.GetValue<string>(nameof(JsonWebTokenAuthorizationOptions.Issuer)),
                Claims = authSection.GetValue<Claim[]>(nameof(JsonWebTokenAuthorizationOptions.Claims)) ?? Array.Empty<Claim>(),
                PrivateKey = authSection.GetValue<string>(nameof(JsonWebTokenAuthorizationOptions.PrivateKey)),
            },
            AuthorizationType.OAuth2Token => new OAuth2TokenAuthorizationOptions {
                Type = AuthorizationType.OAuth2Token,
                Scheme = authSection.GetValue<TokenScheme>(nameof(SimpleTokenAuthorizationOptions.Scheme)),
                Token = authSection.GetValue<string>(nameof(SimpleTokenAuthorizationOptions.Token)),
                ExpiresOn = authSection.GetValue<DateTimeOffset>(nameof(SimpleTokenAuthorizationOptions.ExpiresOn)),
                NotBefore = authSection.GetValue<DateTimeOffset>(nameof(SimpleTokenAuthorizationOptions.NotBefore)),
                //Audience = authSection.GetValue<string>(nameof(JsonWebTokenAuthorizationOptions.Audience)),
                //Issuer = authSection.GetValue<string>(nameof(JsonWebTokenAuthorizationOptions.Issuer)),
                //Claims = authSection.GetValue<Claim[]>(nameof(JsonWebTokenAuthorizationOptions.Claims)) ?? Array.Empty<Claim>(),
                TenantId = authSection.GetValue<string>(nameof(OAuth2TokenAuthorizationOptions.TenantId)),
                ClientId = authSection.GetValue<string>(nameof(OAuth2TokenAuthorizationOptions.ClientId)),
                ClientSecret = authSection.GetValue<string>(nameof(OAuth2TokenAuthorizationOptions.ClientSecret)),
                Authority = authSection.GetValue<string>(nameof(OAuth2TokenAuthorizationOptions.Authority)),
            },
            _ => throw new InvalidOperationException($"Authorization type '{authType}' not supported."),
        };
    }
}