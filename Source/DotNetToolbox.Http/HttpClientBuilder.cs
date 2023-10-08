using static DotNetToolbox.Http.Options.HttpClientAuthorizationScheme;
using static DotNetToolbox.Http.Options.HttpClientAuthorizationType;

namespace DotNetToolbox.Http;

public class HttpClientBuilder {
    private const string _apiKeyHeaderKey = "x-api-key";

    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClientOptions _options;

    public HttpClientBuilder(IHttpClientFactory clientFactory, IConfiguration configuration) {
        _clientFactory = clientFactory;
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract - False positive
        _options = configuration.GetSection(nameof(HttpClientOptions))
                                .Get<HttpClientOptions>()
                                ?? new HttpClientOptions();
    }

    public HttpClientBuilder UseApiKey(string? apiKey = null) {
        _options.Authorization = new() {
            Type = ApiKey,
            Token = apiKey ?? _options.Authorization?.Token,
        };

        return this;
    }

    public HttpClientBuilder UseJsonWebToken(
        string? secret = null,
        string? issuer = null,
        string? audience = null,
        DateTimeOffset? expiresOn = null,
        DateTimeOffset? notBefore = null,
        IEnumerable<Claim>? claims = null) {
        _options.Authorization = new() {
            Type = Jwt,
            Scheme = Bearer,
            Secret = secret ?? _options.Authorization?.Secret,
            Issuer = issuer ?? _options.Authorization?.Issuer,
            Audience = audience ?? _options.Authorization?.Audience,
            NotBefore = notBefore ?? _options.Authorization?.NotBefore,
            ExpiresOn = expiresOn ?? _options.Authorization?.ExpiresOn,
            Claims = claims?.ToArray() ?? _options.Authorization?.Claims ?? Array.Empty<Claim>(),
        };

        return this;
    }

    public HttpClientBuilder UseSimpleToken(HttpClientAuthorizationScheme? scheme = null, string? token = null, DateTimeOffset? notBefore = null, DateTimeOffset? expiresOn = null) {
        _options.Authorization = new() {
            Type = SimpleToken,
            Scheme = scheme ?? _options.Authorization?.Scheme,
            Token = token ?? _options.Authorization?.Token,
            NotBefore = notBefore ?? _options.Authorization?.NotBefore,
            ExpiresOn = expiresOn ?? _options.Authorization?.ExpiresOn,
        };

        return this;
    }

    public HttpClientBuilder UseOauth2(string? tenantId = null, string? clientId = null, string ? clientSecret = null, string ? authority = null, IEnumerable<string>? scopes = null) {
        _options.Authorization = new() {
            Type = OAuth2,
            Scheme = Bearer,
            TenantId = tenantId ?? _options.Authorization?.TenantId,
            ClientId = clientId ?? _options.Authorization?.ClientId,
            Secret = clientSecret ?? _options.Authorization?.Secret,
            Authority = authority ?? _options.Authorization?.Authority,
            Scopes = scopes?.ToArray() ?? _options.Authorization?.Scopes ?? Array.Empty<string>(),
        };

        _options.EnsureIsValid("Http client options are invalid.");
        return this;
    }

    public HttpClient Build(string? name = null) {
        if (name is not null) {
            if (!_options.Clients.TryGetValue(name, out var namedOptions)) {
                throw new ArgumentException($"Http client options for '{name}' not found.", nameof(name));
            }

            _options.BaseAddress = namedOptions.BaseAddress ?? _options.BaseAddress;
            _options.ResponseFormat = namedOptions.ResponseFormat ?? _options.ResponseFormat;
            _options.Authorization = namedOptions.Authorization ?? _options.Authorization;
            _options.CustomHeaders = namedOptions.CustomHeaders.UnionBy(_options.CustomHeaders.ExceptBy(namedOptions.CustomHeaders.Keys, i => i.Key), i => i.Key).ToDictionary(k => k.Key, v => v.Value);
        }

        _options.EnsureIsValid("Http client options are invalid.");

        var client = _clientFactory.CreateClient();
        client.BaseAddress = new(_options.BaseAddress!);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(_options.ResponseFormat!));
        foreach ((var key, var value) in _options.CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        if (_options.Authorization is null)
            return client;

        var authorization = _options.Authorization;
        switch (authorization.Type) {
            case ApiKey:
                client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, authorization.Token);
                break;
            case Jwt:
                CreateJwtTokenAsync();
                client.DefaultRequestHeaders.Authorization = new(authorization.Scheme.ToString()!, authorization.Token);
                break;
            case OAuth2:
                AcquireOauth2Token();
                client.DefaultRequestHeaders.Authorization = new(authorization.Scheme.ToString()!, authorization.Token);
                break;
            case SimpleToken:
                client.DefaultRequestHeaders.Authorization = new(authorization.Scheme.ToString()!, authorization.Token);
                break;
        }

        return client;
    }

    private void CreateJwtTokenAsync() {
        var authorization = IsNotNull(_options.Authorization);
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorization.Secret!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(authorization.Issuer,
                                                authorization.Audience,
                                                authorization.Claims,
                                                authorization.NotBefore?.DateTime,
                                                authorization.ExpiresOn?.DateTime,
                                                signingCredentials);

        authorization.Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private void AcquireOauth2Token() {
        try {
            if (_options.Authorization is { IsActive: true })
                return;

            _options.EnsureIsValid("Http client options are invalid.");

            var authorization = _options.Authorization!;
            var result = AuthenticateClient();
            authorization.Token = result.AccessToken;
            authorization.ExpiresOn = result.ExpiresOn;
            authorization.NotBefore = null;
            authorization.Scopes = result.Scopes.ToArray();
        }
        catch (Exception ex) {
            throw new InvalidOperationException("Failed to set authorization header.", ex);
        }
    }

    [ExcludeFromCodeCoverage]
    private AuthenticationResult AuthenticateClient()
        => CreateApplication()
        .AcquireTokenForClient(_options.Authorization!.Scopes)
        .ExecuteAsync(CancellationToken.None).Result;

    private IConfidentialClientApplication CreateApplication() {
        var authorization = IsNotNull(_options.Authorization);
        var builder = ConfidentialClientApplicationBuilder
                     .Create(authorization.ClientId)
                     .WithClientSecret(authorization.Secret);
        if (string.IsNullOrWhiteSpace(authorization.TenantId))
            builder.WithTenantId(authorization.TenantId);
        if (string.IsNullOrWhiteSpace(authorization.Authority))
            builder.WithAuthority(authorization.Authority);
        return builder.Build();
    }

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateWithAuthorizationCodeAsync(string authorizationCode)
    //    => CreateApplication()
    //    .AcquireTokenByAuthorizationCode(_options.Authorization.Scopes, authorizationCode)
    //    .ExecuteAsync(CancellationToken.None);

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateAccountAsync(IAccount account)
    //    => CreateApplication()
    //    .AcquireTokenSilent(_options.Authorization.Scopes, account)
    //    .ExecuteAsync(CancellationToken.None);
}
