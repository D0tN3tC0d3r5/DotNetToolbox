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

    private HttpClientProvider CreateHttpClientBuilder() {
        var config = Substitute.For<IConfiguration>();
        return new(_clientFactory, config);
    }

    private HttpClientProvider CreateKeyedHttpClientBuilder(string key) {
        var config = Substitute.For<IConfiguration>();
        return new(key, _clientFactory, config);
    }

    private bool _isDisposed;
    public void Dispose() {
        if (_isDisposed) return;
        _provider.RevokeAuthentication();
        _isDisposed = true;
    }

    [Fact]
    public void GetHttpClient_WithDefaultOptions_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder();

        // Act
        var result = () => builder.GetHttpClient();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
        exception.Errors[0].Message.Should().Be("The value is invalid.");
    }

    [Fact]
    public void GetHttpClient_WithInvalidOptions_Throws() {
        // Act
        var result = () => _provider.GetHttpClient(o => o.SetBaseAddress(null!));

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
        // Act
        var result = _provider.GetHttpClient(opt => {
            opt.SetResponseFormat("text/xml");
            opt.AddCustomHeader("x-custom-string", "SomeValue");
            opt.AddCustomHeader("x-custom-int", "42");
        });

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
        var result = _provider.GetHttpClient(configureBuilder: opt => {
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
        var provider = CreateKeyedHttpClientBuilder(null!);

        // Act
        var result = () => provider.GetHttpClient(opt => opt.SetBaseAddress(null!));

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetHttpClient_WithNamedClient_ReturnsHttpClient() {
        // Arrange
        var provider = CreateKeyedHttpClientBuilder("NamedClient1");

        // Act
        var result = provider.GetHttpClient();

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
    public void UseApiKey_FromOptions_AddsApiKeyHeader() {
        // Act
        var result = _provider.GetHttpClient(opt => opt.UseApiKeyAuthentication("abc123"));

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_FromParameter_OverridesOptions() {
        // Act
        var result = _provider.GetHttpClient(configureBuilder: options => options.UseApiKeyAuthentication(opt => opt.ApiKey = "abc123"));

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_WithInvalidOptions_Throws() {
        // Act
        var result = () => _provider.GetHttpClient(opt => opt.UseApiKeyAuthentication(null!));

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

        // Act
        var result = _provider.GetHttpClient(opt => opt.UseTokenAuthentication(tk => tk.Token = "SomeToken"));

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

        // Act
        var result = _provider.GetHttpClient(configureBuilder: options => options.UseTokenAuthentication(opt => {
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

        // Act
        var result = () => _provider.GetHttpClient(opt => opt.UseTokenAuthentication(_ => { }));

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

        // Act
        var result = _provider.GetHttpClient(opt => opt.UseJwtAuthentication(tk => tk.PrivateKey = "ASecretValueWith256BitsOr32Chars"));

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be(Bearer.ToString());
        authorization.Parameter.Should().Be(expectedToken);
    }
}
