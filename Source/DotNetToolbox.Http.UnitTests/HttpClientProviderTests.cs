namespace DotNetToolbox.Http;

public sealed class HttpClientProviderTests : IDisposable  {
    private HttpClientOptions _defaultOptions = new() {
        BaseAddress = "http://example.com/api/",
    };

    private static readonly AuthenticationResult? _fakeAuthenticationResult
        = new(accessToken: "SomeToken",
            isExtendedLifeTimeToken: false,
            uniqueId: null,
            expiresOn: DateTime.Now.AddDays(1),
            extendedExpiresOn: default!,
            tenantId: "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            account: null,
            idToken: null,
            scopes: new[] { "https://graph.microsoft.com/.default" },
            correlationId: Guid.NewGuid());

    private readonly HttpClientProvider _provider;

    public HttpClientProviderTests() {
        _provider = CreateHttpClientBuilder(_defaultOptions);
    }

    private static HttpClientProvider CreateHttpClientBuilder(HttpClientOptions? clientOptions = null) {
        var clientFactory = Substitute.For<IHttpClientFactory>();
        var options = Substitute.For<IOptions<HttpClientOptions>>();
        options.Value.Returns(clientOptions);
        var identityFactory = Substitute.For<IMsalHttpClientFactory>();

        var client = new HttpClient();
        clientFactory.CreateClient(Arg.Any<string>()).Returns(client);

        var identityClient = new HttpClient();
        identityFactory.GetHttpClient().Returns(identityClient);

        // Act
        var result = new HttpClientProvider(clientFactory, options, identityFactory);
        return result;
    }

    private bool _isDisposed;
    public void Dispose() {
        if (_isDisposed) return;
        HttpClientProvider.RevokeAuthorization();
        _isDisposed = true;
    }

    [Fact]
    public void GetHttpClient_WithNoOptions_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder();

        // Act
        var result = () => builder.GetHttpClient();

        // Assert
        result.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void GetHttpClient_WithDefaultOptions_Throws() {
        // Arrange
        _defaultOptions = new();
        var builder = CreateHttpClientBuilder(new());

        // Act
        var result = () => builder.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("'BaseAddress' cannot be null or whitespace.");
    }

    [Fact]
    public void GetHttpClient_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.ResponseFormat = string.Empty;

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("'ResponseFormat' cannot be null or whitespace.");
    }

    [Fact]
    public void GetHttpClient_WithBasicOptions_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.ResponseFormat = "text/xml";
        _defaultOptions.CustomHeaders = new() {
            ["x-custom-string"] = new[] { "SomeValue" },
            ["x-custom-int"] = new[] { "42" },
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        result.BaseAddress.Should().Be("http://example.com/api/");
        result.DefaultRequestHeaders.GetValue("x-custom-string").Should().Be("SomeValue");
        result.DefaultRequestHeaders.GetValue<int>("x-custom-int").Should().Be(42);
        result.DefaultRequestHeaders.TryGetValue("x-custom-string", out var stringValue).Should().BeTrue();
        stringValue.Should().Be("SomeValue");
        result.DefaultRequestHeaders.TryGetValue<int>("x-custom-int", out var intValue).Should().BeTrue();
        intValue.Should().Be(42);
        result.DefaultRequestHeaders.TryGetValue("x-invalid", out _).Should().BeFalse();
        result.DefaultRequestHeaders.TryGetValue<int>("x-invalid", out _).Should().BeFalse();
        result.DefaultRequestHeaders.TryGetValue<int>("x-custom-string", out _).Should().BeFalse();
    }

    [Fact]
    public void GetHttpClient_WithInvalidName_Throws() {
        // Arrange
        _defaultOptions.ResponseFormat = string.Empty;

        // Act
        var result = () => _provider.GetHttpClient("Invalid");

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetHttpClient_WithNamedClient_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.Clients["NamedClient"] = new() {
            BaseAddress = _defaultOptions.BaseAddress,
            ResponseFormat = "text/xml",
            CustomHeaders = new() {
                ["x-custom-string"] = new[] { "SomeValue" },
                ["x-custom-int"] = new[] { "42" },
            },
        };

        // Act
        var result = _provider.GetHttpClient("NamedClient");

        // Assert
        result.BaseAddress.Should().Be("http://example.com/api/");
        result.DefaultRequestHeaders.GetValue("x-custom-string").Should().Be("SomeValue");
        result.DefaultRequestHeaders.GetValue<int>("x-custom-int").Should().Be(42);
        result.DefaultRequestHeaders.TryGetValue("x-custom-string", out var stringValue).Should().BeTrue();
        stringValue.Should().Be("SomeValue");
        result.DefaultRequestHeaders.TryGetValue<int>("x-custom-int", out var intValue).Should().BeTrue();
        intValue.Should().Be(42);
        result.DefaultRequestHeaders.TryGetValue("x-invalid", out _).Should().BeFalse();
        result.DefaultRequestHeaders.TryGetValue<int>("x-invalid", out _).Should().BeFalse();
        result.DefaultRequestHeaders.TryGetValue<int>("x-custom-string", out _).Should().BeFalse();
    }

    [Fact]
    public void UseApiKey_FromConfiguration_AddsApiKeyHeader() {
        // Arrange
        _defaultOptions.Authentication = new ApiKeyAuthenticationOptions {
            ApiKey = "abc123",
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_FromParameter_OverridesConfiguration() {
        // Arrange
        _defaultOptions.Authentication = new ApiKeyAuthenticationOptions {
            ApiKey = "def456",
        };

        // Act
        _provider.UseApiKey(opt => opt.ApiKey = "abc123");
        var result = _provider.GetHttpClient();

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authentication = new ApiKeyAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseSimpleToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new StaticTokenAuthenticationOptions {
            Token = "SomeToken",
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Basic");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_FromParameter_OverridesConfiguration() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new StaticTokenAuthenticationOptions {
            Token = "OtherToken",
        };

        // Act
        _provider.UseSimpleToken(opt => opt.Token = "SomeToken");
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authentication = new StaticTokenAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseJsonWebToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseJsonWebToken_FromParameter_OverridesConfiguration() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "OtherSecretValue256BitsOr32Chars",
        };

        // Act
        _provider.UseJsonWebToken(opt => opt.PrivateKey = "ASecretValueWith256BitsOr32Chars");
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseJsonWebToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authentication = new JwtAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseOAuth2Token_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = new[] { "https://graph.microsoft.com/.default" },
            AuthenticationResult = _fakeAuthenticationResult,
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_FromParameter_OverridesConfiguration() {
        // Arrange
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "OtherTenant",
            ClientId = "OtherClient",
            ClientSecret = "OtherSecret",
            Authority = "OtherAuthority",
            AuthenticationResult = _fakeAuthenticationResult,
        };

        // Act
        _provider.UseOAuth2Token(opt => {
            opt.TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.ClientId = "SomeClient";
            opt.ClientSecret = "SomeSecret";
            opt.Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.Scopes = new[] { "https://graph.microsoft.com/Directory.Read" };
        });
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = new[] { "https://graph.microsoft.com/.default" },
        };

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UseOAuth2Token_WithFailedAuthorization_Throws() {
        // Arrange
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().HaveCount(3);
    }
}
