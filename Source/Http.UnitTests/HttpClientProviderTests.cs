namespace DotNetToolbox.Http;

public sealed class HttpClientProviderTests : IDisposable {
    private readonly HttpClientProvider _provider;
    private readonly IHttpClientFactory _clientFactory;

    public HttpClientProviderTests() {
        _clientFactory = Substitute.For<IHttpClientFactory>();
        var client = new HttpClient();
        _clientFactory.CreateClient(Arg.Any<string>()).Returns(client);
        _provider = CreateHttpClientBuilder();
    }

    private readonly HttpClientOptions _defaultOptions = new() {
        BaseAddress = new("http://example.com/api/"),
    };

    private HttpClientProvider CreateHttpClientBuilder(HttpClientOptions? clientOptions = null) {
        clientOptions ??= _defaultOptions;
        var options = Substitute.For<IOptions<HttpClientOptions>>();
        options.Value.Returns(clientOptions);
        return new(_clientFactory, options);
    }

    private bool _isDisposed;
    public void Dispose() {
        if (_isDisposed) return;
        _provider.RevokeAuthentication();
        _isDisposed = true;
    }

    private static AuthenticationResult GenerateResult(string token, Guid correlationId)
        => new(accessToken: token,
            isExtendedLifeTimeToken: false,
            uniqueId: null,
            expiresOn: DateTime.Parse("2022-01-01").AddMinutes(5),
            extendedExpiresOn: default!,
            tenantId: "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            account: null,
            idToken: Guid.NewGuid().ToString(),
            scopes: ["https://graph.microsoft.com/.default"],
            correlationId: correlationId);

    [Fact]
    public void GetHttpClient_WithDefaultOptions_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder(new());

        // Act
        var result = () => builder.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("The value is invalid.");
    }

    [Fact]
    public void GetHttpClient_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.BaseAddress = null;

        // Act
        var result = () => _provider.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("The value is invalid.");
    }

    [Fact]
    public void GetHttpClient_MinimumOptions_ReturnsHttpClient() {
        // Act
        var result = _provider.GetHttpClient();

        // Assert
        result.BaseAddress.Should().Be("http://example.com/api/");
        result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Fact]
    public void GetHttpClient_FromOptions_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.ResponseFormat = "text/xml";
        _defaultOptions.CustomHeaders = new() {
            ["x-custom-string"] = ["SomeValue"],
            ["x-custom-int"] = ["42"],
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        result.BaseAddress.Should().Be("http://example.com/api/");
        result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("text/xml"));
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
    public void GetHttpClient_FromParameters_ReturnsHttpClient() {
        // Arrange
        // Act
        var result = _provider.GetHttpClient(opt => {
            opt.SetBaseAddress(new("http://example.com/api/v2/"));
            opt.SetResponseFormat("text/xml");
            opt.AddCustomHeader("x-custom-string", "SomeValue");
            opt.AddCustomHeader("x-custom-string", "SomeValue");
            opt.AddCustomHeader("x-custom-string", "SomeOtherValue");
            opt.AddCustomHeader("x-custom-int", "42");
        });

        // Assert
        result.BaseAddress.Should().Be("http://example.com/api/v2/");
        result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("text/xml"));
        result.DefaultRequestHeaders.GetValues("x-custom-string").Should().BeEquivalentTo("SomeValue", "SomeOtherValue");
    }

    [Fact]
    public void GetHttpClient_WithInvalidName_Throws() {
        // Arrange
        _defaultOptions.BaseAddress = null;

        // Act
        var result = () => _provider.GetHttpClient("Invalid");

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetHttpClient_WithNamedClient_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.Clients = new() {
            ["NamedClient1"] = new() {
                BaseAddress = _defaultOptions.BaseAddress,
                ResponseFormat = "text/xml",
                CustomHeaders = new() {
                    ["x-custom-string"] = ["SomeValue"],
                    ["x-custom-int"] = ["42"],
                },
            },
        };

        // Act
        var result = _provider.GetHttpClient("NamedClient1");

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
    public void GetHttpClient_WithInvalidNamedClient_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.BaseAddress = null;
        _defaultOptions.Clients = new() {
            ["NamedClient1"] = new() {
                BaseAddress = _defaultOptions.BaseAddress,
                ResponseFormat = "text/xml",
                CustomHeaders = new() {
                    ["x-custom-string"] = ["SomeValue"],
                    ["x-custom-int"] = ["42"],
                },
            },
            ["NamedClient2"] = new(),
        };

        // Act
        var result = () => _provider.GetHttpClient("NamedClient2");

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("The value is invalid.");
    }

    [Fact]
    public void UseApiKey_FromOptions_AddsApiKeyHeader() {
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
    public void UseApiKey_FromParameter_OverridesOptions() {
        // Act
        var result = _provider.GetHttpClient(options => options.UseApiKeyAuthentication(opt => opt.ApiKey = "abc123"));

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
    public void UseSimpleToken_FromOptions_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new StaticTokenAuthenticationOptions {
            Token = expectedToken,
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Basic");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_FromParameter_OverridesOptions() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new StaticTokenAuthenticationOptions {
            Token = "OtherToken",
        };

        // Act
        var result = _provider.GetHttpClient(options => options.UseSimpleTokenAuthentication(opt => {
            opt.Token = expectedToken;
            opt.Scheme = Bearer;
        }));

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be(Bearer.ToString());
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
    public void UseJsonWebToken_FromOptions_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - HttpToken
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
                                   + ".e30"
                                   + ".QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be(Bearer.ToString());
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseJsonWebToken_AfterASimpleCall_CreatesNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((JwtAuthenticationOptions)_defaultOptions.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_TokenWithoutExpiration_ReusesSameToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((JwtAuthenticationOptions)_defaultOptions.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_PreviousTokenStillValid_ReusesSameToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
            DateTimeProvider = dateTimeProvider,
            ExpiresAfter = TimeSpan.FromMinutes(5),
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((JwtAuthenticationOptions)_defaultOptions.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_PreviousTokenExpired_CreatedNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
            DateTimeProvider = dateTimeProvider,
            ExpiresAfter = TimeSpan.FromMinutes(5),
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(6));
        ((JwtAuthenticationOptions)_defaultOptions.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().NotBe(secondToken);
    }

    [Fact]
    public void UseJsonWebToken_FromParameter_OverridesOptions() {
        // Arrange
        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "OtherSecretValue256BitsOr32Chars",
        };
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));

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
        authorization.Scheme.Should().Be(Bearer.ToString());
        authorization.Parameter.Should().NotBeNull();
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
    public void UseOAuth2Token_FromOptions_AddsAuthorizationHeader() {
        // Arrange
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default"],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
        };

        // Act
        var result = _provider.GetHttpClient();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be(Bearer.ToString());
        authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_PreviousTokenStillValid_ReusesSameToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default"],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
            DateTimeProvider = dateTimeProvider,
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(2));
        ((OAuth2TokenAuthenticationOptions)_defaultOptions.Authentication).DateTimeProvider = dateTimeProvider;

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().Be(secondToken);
    }

    [Fact]
    public void UseOAuth2Token_PreviousTokenExpired_CreatedNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default"],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
            DateTimeProvider = dateTimeProvider,
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(6));
        ((OAuth2TokenAuthenticationOptions)_defaultOptions.Authentication).DateTimeProvider = dateTimeProvider;
        ((OAuth2TokenAuthenticationOptions)_defaultOptions.Authentication).AuthenticationResult = GenerateResult("SomeOtherToken", Guid.NewGuid());

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().NotBe(secondToken);
    }

    [Fact]
    public void UseOAuth2Token_PreviousTokenOfDifferentType_CreatedNewToken() {
        // Arrange
        var dateTimeProvider = Substitute.For<DateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01"));
        _defaultOptions.Authentication = new JwtAuthenticationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
            DateTimeProvider = dateTimeProvider,
            ExpiresAfter = TimeSpan.FromMinutes(5),
        };
        var firstClient = _provider.GetHttpClient(); // The token is generated on the first call;
        var firstToken = firstClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;

        dateTimeProvider.UtcNow.Returns(DateTime.Parse("2022-01-01").AddMinutes(6));
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = ["https://graph.microsoft.com/.default"],
            AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid()),
            DateTimeProvider = dateTimeProvider,
        };

        // Act
        var secondClient = _provider.GetHttpClient();

        // Assert
        var secondToken = secondClient.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject.Parameter;
        firstToken.Should().NotBe(secondToken);
    }

    [Fact]
    public void UseOAuth2Token_FromParameter_OverridesOptions() {
        // Arrange
        _defaultOptions.Authentication = new OAuth2TokenAuthenticationOptions {
            TenantId = "OtherTenant",
            ClientId = "OtherClient",
            ClientSecret = "OtherSecret",
            Authority = "OtherAuthority",
        };
        var identityFactory = Substitute.For<IMsalHttpClientFactory>();

        var identityClient = new HttpClient();
        identityFactory.GetHttpClient().Returns(identityClient);

        // Act
        var result = _provider.GetHttpClient(options => options.UseOAuth2TokenAuthentication(opt => {
            opt.TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.ClientId = "SomeClient";
            opt.ClientSecret = "SomeSecret";
            opt.Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.Scopes = ["https://graph.microsoft.com/Directory.Read"];
            opt.AuthenticationResult = GenerateResult("SomeToken", Guid.NewGuid());
        }, identityFactory));

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be(Bearer.ToString());
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
            Scopes = ["https://graph.microsoft.com/.default"],
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
        exception.Errors.Should().HaveCount(1);
    }
}
