namespace DotNetToolbox.Http;

public class HttpClientBuilderTests {
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

    [Fact]
    public void Build_WithDefaultConstructor_Throws() {
        // Arrange
        _defaultOptions = new();

        // Act
        var builder = CreateHttpClientBuilder();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("'BaseAddress' cannot be null or whitespace.");
    }

    [Fact]
    public void Build_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.ResponseFormat = string.Empty;

        // Act
        var builder = CreateHttpClientBuilder();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("'ResponseFormat' cannot be null or whitespace.");
    }

    [Fact]
    public void Build_WithBasicOptions_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.ResponseFormat = "text/xml";
        _defaultOptions.CustomHeaders = new() {
            ["x-custom-string"] = new[] { "SomeValue" },
            ["x-custom-int"] = new[] { "42" },
        };
        var builder = CreateHttpClientBuilder();

        // Act
        var result = builder.Build();

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
    public void Build_WithInvalidName_Throws() {
        // Arrange
        _defaultOptions.ResponseFormat = string.Empty;

        // Act
        var builder = CreateHttpClientBuilder();
        var result = () => builder.Build("Invalid");

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Build_WithNamedClient_ReturnsHttpClient() {
        // Arrange
        _defaultOptions.Clients["NamedClient"] = new() {
            ResponseFormat = "text/xml",
            CustomHeaders = new() {
                ["x-custom-string"] = new[] { "SomeValue" },
                ["x-custom-int"] = new[] { "42" },
            },
        };
        var builder = CreateHttpClientBuilder();

        // Act
        var result = builder.Build("NamedClient");

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
        _defaultOptions.Authorization = new ApiKeyAuthorizationOptions {
            ApiKey = "abc123",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseApiKey();
        var result = builder.Build();

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_FromParameter_OverridesConfiguration() {
        // Arrange
        _defaultOptions.Authorization = new ApiKeyAuthorizationOptions {
            ApiKey = "def456",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseApiKey(opt => opt.ApiKey = "abc123");
        var result = builder.Build();

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authorization = new ApiKeyAuthorizationOptions();
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseApiKey();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseSimpleToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - Token
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authorization = new SimpleTokenAuthorizationOptions {
            Token = "SomeToken",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseSimpleToken();
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Basic");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_FromParameter_OverridesConfiguration() {
        // Arrange
        // ReSharper disable StringLiteralTypo - Token
        const string expectedToken = "SomeToken";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authorization = new SimpleTokenAuthorizationOptions {
            Token = "OtherToken",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseSimpleToken(opt => opt.Token = "SomeToken");
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseSimpleToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authorization = new SimpleTokenAuthorizationOptions();
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseSimpleToken();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseJsonWebToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - Token
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authorization = new JsonWebTokenAuthorizationOptions {
            PrivateKey = "ASecretValueWith256BitsOr32Chars",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseJsonWebToken();
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseJsonWebToken_FromParameter_OverridesConfiguration() {
        // Arrange
        // ReSharper disable StringLiteralTypo - Token
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authorization = new JsonWebTokenAuthorizationOptions {
            PrivateKey = "OtherSecretValue256BitsOr32Chars",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseJsonWebToken(opt => opt.PrivateKey = "ASecretValueWith256BitsOr32Chars");
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be(expectedToken);
    }

    [Fact]
    public void UseJsonWebToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authorization = new JsonWebTokenAuthorizationOptions();
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseJsonWebToken();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseOAuth2Token_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        _defaultOptions.Authorization = new OAuth2TokenAuthorizationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = new[] { "https://graph.microsoft.com/.default" },
        };
        var builder = CreateHttpClientBuilder();
        builder.AuthenticationResult = _fakeAuthenticationResult;

        // Act
        builder.UseOAuth2Token();
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_FromParameter_OverridesConfiguration() {
        // Arrange
        _defaultOptions.Authorization = new OAuth2TokenAuthorizationOptions {
            TenantId = "OtherTenant",
            ClientId = "OtherClient",
            ClientSecret = "OtherSecret",
            Authority = "OtherAuthority",
        };
        var builder = CreateHttpClientBuilder();
        builder.AuthenticationResult = _fakeAuthenticationResult;

        // Act
        builder.UseOAuth2Token(opt => {
            opt.TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.ClientId = "SomeClient";
            opt.ClientSecret = "SomeSecret";
            opt.Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a";
            opt.Scopes = new[] { "https://graph.microsoft.com/Directory.Read" };
        });
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseOAuth2Token_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authorization = new OAuth2TokenAuthorizationOptions {
            TenantId = "a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            ClientId = "SomeClient",
            ClientSecret = "SomeSecret",
            Authority = "https://login.microsoftonline.com/a4d9d2af-cd3d-40de-945f-0be9ad34658a",
            Scopes = new[] { "https://graph.microsoft.com/.default" },
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseOAuth2Token();
        var result = () => builder.Build();

        // Assert
        result.Should().Throw<InvalidOperationException>();
    }


    [Fact]
    public void UseOAuth2Token_WithFailedAuthorization_Throws() {
        // Arrange
        _defaultOptions.Authorization = new OAuth2TokenAuthorizationOptions();
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseOAuth2Token();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().HaveCount(3);
    }

    private HttpClientBuilder CreateHttpClientBuilder() {
        var clientFactory = Substitute.For<IHttpClientFactory>();
        var options = Substitute.For<IOptions<HttpClientOptions>>();
        options.Value.Returns(_defaultOptions);
        var identityFactory = Substitute.For<IMsalHttpClientFactory>();

        var client = new HttpClient();
        clientFactory.CreateClient(Arg.Any<string>()).Returns(client);

        var identityClient = new HttpClient();
        identityFactory.GetHttpClient().Returns(identityClient);

        // Act
        var result = new HttpClientBuilder(clientFactory, options, identityFactory);
        return result;
    }
}
