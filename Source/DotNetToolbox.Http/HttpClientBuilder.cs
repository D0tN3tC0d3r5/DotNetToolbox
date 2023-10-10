namespace DotNetToolbox.Http;

public class HttpClientBuilder {
    private const string _apiKeyHeaderKey = "x-api-key";

    private readonly IHttpClientFactory _clientFactory;
    private readonly IMsalHttpClientFactory _identityClientFactory;
    private readonly HttpClientOptions _options;

    public HttpClientBuilder(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options, IMsalHttpClientFactory identityClientFactory) {
        _clientFactory = clientFactory;
        _identityClientFactory = identityClientFactory;
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract - False positive
        _options = options.Value;
    }

    public HttpClientBuilder UseApiKey(Action<ApiKeyAuthorizationOptions>? configOptions = null) {
        _options.Authorization ??= new ApiKeyAuthorizationOptions();
        configOptions?.Invoke((ApiKeyAuthorizationOptions)_options.Authorization);
        return this;
    }

    public HttpClientBuilder UseSimpleToken(Action<SimpleTokenAuthorizationOptions>? configOptions = null) {
        _options.Authorization ??= new ApiKeyAuthorizationOptions();
        configOptions?.Invoke((SimpleTokenAuthorizationOptions)_options.Authorization);
        return this;
    }

    public HttpClientBuilder UseJsonWebToken(Action<JsonWebTokenAuthorizationOptions>? configOptions = null) {
        _options.Authorization ??= new JsonWebTokenAuthorizationOptions();
        configOptions?.Invoke((JsonWebTokenAuthorizationOptions)_options.Authorization);
        return this;
    }

    public HttpClientBuilder UseOauth2(Action<OAuth2TokenAuthorizationOptions>? configOptions = null) {
        _options.Authorization ??= new OAuth2TokenAuthorizationOptions();
        configOptions?.Invoke((OAuth2TokenAuthorizationOptions)_options.Authorization);
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
        switch (authorization) {
            case ApiKeyAuthorizationOptions authOptions:
                client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, authOptions.ApiKey);
                break;
            case JsonWebTokenAuthorizationOptions authOptions:
                CreateJwtTokenAsync();
                client.DefaultRequestHeaders.Authorization = new(authOptions.Scheme.ToString()!, authOptions.Token);
                break;
            case OAuth2TokenAuthorizationOptions authOptions:
                AcquireOauth2Token();
                client.DefaultRequestHeaders.Authorization = new(authOptions.Scheme.ToString()!, authOptions.Token);
                break;
            case SimpleTokenAuthorizationOptions authOptions:
                client.DefaultRequestHeaders.Authorization = new(authOptions.Scheme.ToString()!, authOptions.Token);
                break;
        }

        return client;
    }

    private void CreateJwtTokenAsync() {
        var authorization = IsOfType<JsonWebTokenAuthorizationOptions>(_options.Authorization);
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authorization.PrivateKey!));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(authorization.Issuer,
                                                authorization.Audience,
                                                authorization.Claims,
                                                authorization.NotBefore?.DateTime,
                                                authorization.ExpiresOn?.DateTime,
                                                signingCredentials);

        authorization.Scheme = TokenScheme.Bearer;
        authorization.Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private void AcquireOauth2Token() {
        var authorization = IsOfType<OAuth2TokenAuthorizationOptions>(_options.Authorization);
        try {
            if (authorization is { IsActive: true })
                return;

            var result = AuthenticateClient(authorization);
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
    private AuthenticationResult AuthenticateClient(OAuth2TokenAuthorizationOptions options)
        => CreateApplication(options)
        .AcquireTokenForClient(options.Scopes)
        .ExecuteAsync(CancellationToken.None)
        .Result;

    private IConfidentialClientApplication CreateApplication(OAuth2TokenAuthorizationOptions options) {
        var builder = ConfidentialClientApplicationBuilder
                     .Create(options.ClientId)
                     .WithHttpClientFactory(_identityClientFactory)
                     .WithClientSecret(options.ClientSecret);
        if (string.IsNullOrWhiteSpace(options.TenantId))
            builder.WithTenantId(options.TenantId);
        if (string.IsNullOrWhiteSpace(options.Authority))
            builder.WithAuthority(options.Authority);
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
