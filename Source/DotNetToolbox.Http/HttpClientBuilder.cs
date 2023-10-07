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
            Value = apiKey ?? _options.Authorization?.Value,
        };
        return this;
    }

    public HttpClientBuilder UseJsonWebToken(string? issuer = null, string? audience = null, DateTimeOffset? expiresOn = null, DateTimeOffset? notBefore = null, IEnumerable<Claim>? claims = null) {
        _options.Authorization = new() {
            Type = Jwt,
            Scheme = HttpClientAuthorizationScheme.Bearer,
            Issuer = issuer ?? _options.Authorization?.Issuer,
            Audience = audience ?? _options.Authorization?.Audience,
            ExpiresOn = expiresOn ?? _options.Authorization?.ExpiresOn,
            NotBefore = notBefore ?? _options.Authorization?.NotBefore,
            Claims = claims?.ToArray() ?? _options.Authorization?.Claims ?? Array.Empty<Claim>(),
        };
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
                client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, authorization.Value);
                break;
            case Jwt:
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorization.ClientSecret!));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(authorization.Issuer,
                                                        authorization.Audience,
                                                        authorization.Claims,
                                                        authorization.NotBefore?.DateTime,
                                                        authorization.ExpiresOn?.DateTime,
                                                        signingCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                client.DefaultRequestHeaders.Authorization = new(authorization.Scheme.ToString()!, tokenString);
                break;
            default:
                throw new InvalidOperationException($"Http client authorization of type '{authorization.Type}' is not supported.");
        }

        return client;
    }

    //public Task<HttpClientBuilder> AuthorizeClientAsync()
    //    => AcquireTokenAsync(HttpClientAuthorizationType.Client, AuthenticateClientAsync);

    //public Task<HttpClientBuilder> AuthorizeByCodeAsync(string authorizationCode)
    //    => AcquireTokenAsync(HttpClientAuthorizationType.ByCode, () => AuthenticateWithAuthorizationCodeAsync(authorizationCode));

    //public Task<HttpClientBuilder> AuthorizeAccountAsync(IAccount account)
    //    => AcquireTokenAsync(HttpClientAuthorizationType .Account, () => AuthenticateAccountAsync(account));

    //private async Task<HttpClientBuilder> AcquireTokenAsync(HttpClientAuthorizationType type, Func<Task<AuthenticationResult>> authenticateAsync) {
    //    try {
    //        await EnsureIsAuthenticatedAsync(type, authenticateAsync);
    //        var authHeader = $"{_options.Authorization.TokenType} {_options.Authorization.Token}";
    //        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeader);
    //        return this;
    //    }
    //    catch (Exception ex) {
    //        throw new InvalidOperationException("Failed to set authorization header.", ex);
    //    }
    //}

    //private async Task EnsureIsAuthenticatedAsync(HttpClientAuthorizationType type, Func<Task<AuthenticationResult>> authenticateAsync) {
    //    var auth = _options.Authorization;
    //    if (auth is { Token: not null, IsActive: false })
    //        return;
    //    var result = await authenticateAsync();
    //    _options = _options with {
    //        Authorization = _options.Authorization with {
    //            Type = type,
    //            TokenType = result.TokenType,
    //            Token = result.AccessToken,
    //            ExpiresOn = result.ExpiresOn,
    //            TenantId = result.TenantId,
    //            Scopes = result.Scopes.ToArray(),
    //        },
    //    };
    //}

    //private IConfidentialClientApplication CreateApplication() {
    //    var app = ConfidentialClientApplicationBuilder
    //             .Create(IsNotNullOrWhiteSpace(_options.Authorization.ClientId))
    //             .WithAuthority(IsNotNullOrWhiteSpace(_options.Authorization.Authority))
    //             .WithClientSecret(IsNotNullOrWhiteSpace(_options.Authorization.ClientSecret))
    //             .Build();
    //    return app;
    //}

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateClientAsync()
    //    => CreateApplication()
    //    .AcquireTokenForClient(_options.Authorization.Scopes)
    //    .ExecuteAsync(CancellationToken.None);

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
