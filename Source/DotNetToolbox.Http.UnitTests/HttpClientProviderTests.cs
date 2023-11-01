using System.Security.Claims;

namespace DotNetToolbox.Http;

public sealed class HttpClientProviderTests : IDisposable {
    private readonly HttpClientConfiguration _defaultConfiguration = new() {
        BaseAddress = "http://example.com/api/",
    };

    private static AuthenticationResult GenerateResult(string token, Guid correlationId)
        => new(accessToken: token,
            expiresOn: DateTime.Parse("2022-01-01").AddMinutes(5),
            isExtendedLifeTimeToken: false,
            extendedExpiresOn: default!,
            tenantId: "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            uniqueId: null,
            account: null,
            idToken: Guid.NewGuid().ToString(),
            scopes: ["https://graph.microsoft.com/.default",],
            correlationId: correlationId);

    private readonly HttpClientProvider _provider;

    public HttpClientProviderTests() {
        _provider = CreateHttpClientBuilder(_defaultConfiguration);
    }

    private static HttpClientProvider CreateHttpClientBuilder(HttpClientConfiguration? clientOptions = null) {
        var clientFactory = Substitute.For<IHttpClientFactory>();
        var options = Substitute.For<IOptions<HttpClientConfiguration>>();
        _ = options.Value.Returns(clientOptions);
        var identityFactory = Substitute.For<IMsalHttpClientFactory>();

        var client = new HttpClient();
        _ = clientFactory.CreateClient(Arg.Any<string>()).Returns(client);

        var identityClient = new HttpClient();
        _ = identityFactory.GetHttpClient().Returns(identityClient);

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
    public void GetHttpClient_WithDefaultOptions_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder(new());

        // Act
        var result = () => builder.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().ContainSingle();
        _ = exception.Errors.First().FormattedMessage.Should().Be("BaseAddress: Value cannot be null or white space.");
    }

    [Fact]
    public void GetHttpClient_WithInvalidOptions_Throws() {
        // Arrange
        _defaultConfiguration.ResponseFormat = string.Empty;

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().ContainSingle();
        _ = exception.Errors.First().FormattedMessage.Should().Be("ResponseFormat: Value cannot be null or white space.");
    }

    [Fact]
    public void GetHttpClient_MinimumConfiguration_ReturnsHttpClient() {
        // Act
        var result = _provider.GetHttpClient();

        // Assert
        _ = result.BaseAddress.Should().Be("http://example.com/api/");
        _ = result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Fact]
    public void GetHttpClient_FromConfiguration_ReturnsHttpClient() {
        // Arrange
        _defaultConfiguration.ResponseFormat = "text/xml";
        _defaultConfiguration.CustomHeaders = new() {
            ["x-custom-string"] = ["SomeValue",],
            ["x-custom-int"] = ["42",],
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        _ = result.BaseAddress.Should().Be("http://example.com/api/");
        _ = result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("text/xml"));
        _ = result.DefaultRequestHeaders.GetValue("x-custom-string").Should().Be("SomeValue");
        _ = result.DefaultRequestHeaders.GetValue<int>("x-custom-int").Should().Be(42);
        _ = result.DefaultRequestHeaders.TryGetValue("x-custom-string", out var stringValue).Should().BeTrue();
        _ = stringValue.Should().Be("SomeValue");
        _ = result.DefaultRequestHeaders.TryGetValue<int>("x-custom-int", out var intValue).Should().BeTrue();
        _ = intValue.Should().Be(42);
        _ = result.DefaultRequestHeaders.TryGetValue("x-invalid", out _).Should().BeFalse();
        _ = result.DefaultRequestHeaders.TryGetValue<int>("x-invalid", out _).Should().BeFalse();
        _ = result.DefaultRequestHeaders.TryGetValue<int>("x-custom-string", out _).Should().BeFalse();
    }

    [Fact]
    public void GetHttpClient_FromParameters_ReturnsHttpClient() {
        // Arrange
        // Act
        var result = _provider.GetHttpClient(config => {
            _ = config.SetBaseAddress("http://example.com/api/v2/");
            _ = config.SetResponseFormat("text/xml");
            _ = config.AddCustomHeader("x-custom-string", "SomeValue");
            _ = config.AddCustomHeader("x-custom-string", "SomeValue");
            _ = config.AddCustomHeader("x-custom-string", "SomeOtherValue");
            _ = config.AddCustomHeader("x-custom-int", "42");
        });

        // Assert
        _ = result.BaseAddress.Should().Be("http://example.com/api/v2/");
        _ = result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("text/xml"));
        _ = result.DefaultRequestHeaders.GetValues("x-custom-string").Should().BeEquivalentTo("SomeValue", "SomeOtherValue");
    }

    [Fact]
    public void GetHttpClient_WithInvalidName_Throws() {
        // Arrange
        _defaultConfiguration.ResponseFormat = string.Empty;

        // Act
        var result = () => _provider.GetHttpClient("Invalid");

        // Assert
        _ = result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetHttpClient_WithNamedClient_ReturnsHttpClient() {
        // Arrange
        _defaultConfiguration.Clients["NamedClient1"] = new() {
            BaseAddress = _defaultConfiguration.BaseAddress,
            ResponseFormat = "text/xml",
            CustomHeaders = new() {
                ["x-custom-string"] = ["SomeValue",],
                ["x-custom-int"] = ["42",],
            },
        };

        // Act
        var result = _provider.GetHttpClient("NamedClient1");

        // Assert
        _ = result.BaseAddress.Should().Be("http://example.com/api/");
        _ = result.DefaultRequestHeaders.GetValue("x-custom-string").Should().Be("SomeValue");
        _ = result.DefaultRequestHeaders.GetValue<int>("x-custom-int").Should().Be(42);
        _ = result.DefaultRequestHeaders.TryGetValue("x-custom-string", out var stringValue).Should().BeTrue();
        _ = stringValue.Should().Be("SomeValue");
        _ = result.DefaultRequestHeaders.TryGetValue<int>("x-custom-int", out var intValue).Should().BeTrue();
        _ = intValue.Should().Be(42);
        _ = result.DefaultRequestHeaders.TryGetValue("x-invalid", out _).Should().BeFalse();
        _ = result.DefaultRequestHeaders.TryGetValue<int>("x-invalid", out _).Should().BeFalse();
        _ = result.DefaultRequestHeaders.TryGetValue<int>("x-custom-string", out _).Should().BeFalse();
    }

    [Fact]
    public void GetHttpClient_WithInvalidNamedClient_ReturnsHttpClient() {
        // Arrange
        _defaultConfiguration.Clients = new() {
            ["NamedClient1"] = new() {
                BaseAddress = _defaultConfiguration.BaseAddress,
                ResponseFormat = "text/xml",
                CustomHeaders = new() {
                    ["x-custom-string"] = ["SomeValue",],
                    ["x-custom-int"] = ["42",],
                },
            },
            ["NamedClient2"] = new(),
        };

        // Act
        var result = () => _provider.GetHttpClient("NamedClient2");

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().ContainSingle();
        _ = exception.Errors.First().FormattedMessage.Should().Be("BaseAddress: Value cannot be null or white space.");
    }

    [Fact]
    public void UseApiKey_FromConfiguration_AddsApiKeyHeader() {
        // Arrange
        _defaultConfiguration.Authentication = new ApiKeyAuthenticationOptions {
            ApiKey = "abc123",
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        _ = result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_FromParameter_OverridesConfiguration() {
        // Act
        var result = _provider.GetHttpClient(options => options.UseApiKeyAuthentication(opt => opt.ApiKey = "abc123"));

        // Assert
        _ = result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_WithInvalidOptions_Throws() {
        // Arrange
        _defaultConfiguration.Authentication = new ApiKeyAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseSimpleToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultConfiguration.Authentication = new StaticTokenAuthenticationOptions {
            Token = expectedToken,
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        _ = authorization.Scheme.Should().Be("Basic");
        _ = authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_FromParameter_OverridesConfiguration() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultConfiguration.Authentication = new StaticTokenAuthenticationOptions {
            Token = "OtherToken",
        };

        // Act
        var result = _provider.GetHttpClient(options => options.UseSimpleTokenAuthentication(opt => {
            opt.Token = expectedToken;
            opt.Scheme = Bearer;
        }));

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        _ = authorization.Scheme.Should().Be(Bearer.ToString());
        _ = authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultConfiguration.Authentication = new StaticTokenAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseJsonWebToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
                                   + ".e30"
                                   + ".QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        _ = authorization.Scheme.Should().Be(Bearer.ToString());
        _ = authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseJsonWebToken_AfterASimpleCall_CreatesNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((JwtAuthenticationOptions)_defaultConfiguration.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_TokenWithoutExpiration_ReusesSameToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((JwtAuthenticationOptions)_defaultConfiguration.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_PreviousTokenStillValid_ReusesSameToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
            DateTimeProvider = dateTimeProvider,
            ExpiresAfter = TimeSpan.FromMinutes(5),
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((JwtAuthenticationOptions)_defaultConfiguration.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_PreviousTokenExpired_CreatedNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
            DateTimeProvider = dateTimeProvider,
            ExpiresAfter = TimeSpan.FromMinutes(5),
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(6));
        ((JwtAuthenticationOptions)_defaultConfiguration.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().NotBe(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_FromParameter_OverridesConfiguration() {
        // Arrange
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "OtherSecretValue256BitsOr32Chars",
        };
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));

        // Act
        var result = _provider.GetHttpClient(options => options.UseJsonWebTokenAuthentication(opt => {
            opt.DateTimeProvider = dateTimeProvider;
            opt.PrivateKey = "ASecretValueWith256BitsOr32Chars";
            opt.Audience = "SomeAudience";
            opt.Issuer = "SomeIssue";
            opt.Claims = new Claim[] {
                new ("SubmittedClaim", "ClaimValue"),
                new ("RequestedClaim", string.Empty),
            };
            opt.ExpiresAfter = TimeSpan.FromMinutes(5);
        }));

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        _ = authorization.Scheme.Should().Be(Bearer.ToString());
        _ = authorization.Parameter.Should().NotBeNull();
    }

    [Fact]
    public void UseJsonWebToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseOAuth2Token_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default",],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        _ = authorization.Scheme.Should().Be(Bearer.ToString());
        _ = authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_PreviousTokenStillValid_ReusesSameToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default",],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
            DateTimeProvider = dateTimeProvider,
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((OAuth2TokenAuthenticationOptions)_defaultConfiguration.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseOAuth2Token_PreviousTokenExpired_CreatedNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default",],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
            DateTimeProvider = dateTimeProvider,
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(6));
        ((OAuth2TokenAuthenticationOptions)_defaultConfiguration.Authentication).DateTimeProvider = dateTimeProvider;
        ((OAuth2TokenAuthenticationOptions)_defaultConfiguration.Authentication).AuthenticationResult = GenerateResult("SomeOtherToken", Guid.NewGuid());

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().NotBe(secondToken);
    }

    [Fact]
    public void UseOAuth2Token_PreviousTokenOfDifferentType_CreatedNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultConfiguration.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
            DateTimeProvider = dateTimeProvider,
            ExpiresAfter = TimeSpan.FromMinutes(5),
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        _ = dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(6));
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default",],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
            DateTimeProvider = dateTimeProvider,
        };

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        _ = firstToken.Should().NotBe(secondToken);
    }

    [Fact]
    public void UseOAuth2Token_FromParameter_OverridesConfiguration() {
        // Arrange
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "OtherTenant",
            ClientId = "OtherClient",
            ClientSecret = "OtherSecret",
            Authority = "OtherAuthority",
        };

        // Act
        var result = _provider.GetHttpClient(options => options.UseOAuth2TokenAuthentication(opt => {
            opt.TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.ClientId = "SomeClient";
            opt.ClientSecret = "SomeSecret";
            opt.Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.Scopes = ["https://graph.microsoft.com/Directory.Read",];
            opt.AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid());
        }));

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        _ = authorization.Scheme.Should().Be(Bearer.ToString());
        _ = authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_WithInvalidOptions_Throws() {
        // Arrange
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default",],
        };

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        _ = result.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UseOAuth2Token_WithFailedAuthorization_Throws() {
        // Arrange
        _defaultConfiguration.Authentication = new OAuth2TokenAuthenticationOptions();

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        _ = exception.Errors.Should().HaveCount(3);
    }
}
