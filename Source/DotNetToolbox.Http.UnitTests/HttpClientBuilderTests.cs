namespace DotNetToolbox.Http;

public class HttpClientBuilderTests {
    private readonly HttpClientOptions _defaultOptions = new() {
        BaseAddress = "http://example.com/api/",
    };

    [Fact]
    public void Build_WithBasicOptions_ReturnsHttpClient() {
        // Arrange
        var clientOptions = _defaultOptions with {
            ResponseFormat = "text/xml",
            CustomHeaders = new() {
                ["x-custom-string"] = "SomeValue",
                ["x-custom-int"] = "42",
            },
        };
        var builder = CreateHttpClientBuilder(clientOptions);

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
    public void UseApiKey_AddsApiKeyHeader() {
        // Arrange
        var clientOptions = _defaultOptions with {
            ApiKey = "abc123",
        };
        var builder = CreateHttpClientBuilder(clientOptions);

        // Act
        builder.UseApiKey();
        var result = builder.Build();

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_WithNoApiKey_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder(_defaultOptions);

        // Act
        var result = builder.UseApiKey;

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task AcquireTokenAsync_AddsAuthenticationHeader() {
        // Arrange
        var clientOptions = _defaultOptions with {
            ClientId = "SomeClientId",
            Authority = "https://example.com/user",
            ClientSecret = "SomeSecret",
            Scopes = new[] { "SomeScope" },
        };
        var builder = CreateHttpClientBuilder(clientOptions);
        var tokenAcquirer = Substitute.For<ITokenAcquirer>();
        tokenAcquirer.AcquireTokenAsync().Returns("Bearer SomeToken");

        // Act
        await builder.AcquireTokenAsync(tokenAcquirer);
        var result = builder.Build();

        // Assert
        var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        authorization.Scheme.Should().Be("Bearer");
        authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public async Task AcquireTokenAsync_WithNoAuthenticationValues_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder(_defaultOptions);
        var tokenAcquirer = Substitute.For<ITokenAcquirer>();
        tokenAcquirer.AcquireTokenAsync().Returns("Bearer SomeToken");

        // Act
        var result = () => builder.AcquireTokenAsync(tokenAcquirer);

        // Assert
        await result.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AcquireTokenAsync_WithFailedTokenAcquisition_Throws() {
        // Arrange
        var clientOptions = _defaultOptions with {
            ClientId = "SomeClientId",
            Authority = "https://example.com/user",
            ClientSecret = "SomeSecret",
            Scopes = new[] { "SomeScope" },
        };
        var builder = CreateHttpClientBuilder(clientOptions);

        // Act
        var result = () => builder.AcquireTokenAsync();

        // Assert
        await result.Should().ThrowAsync<MsalServiceException>();
    }

    private static HttpClientBuilder CreateHttpClientBuilder(HttpClientOptions optionsValue) {
        var factory = Substitute.For<IHttpClientFactory>();
        var options = Substitute.For<IOptions<HttpClientOptions>>();
        options.Value.Returns(optionsValue);

        var client = new HttpClient();
        factory.CreateClient(Arg.Any<string>()).Returns(client);

        // Act
        var result = new HttpClientBuilder(factory, options);
        return result;
    }
}
