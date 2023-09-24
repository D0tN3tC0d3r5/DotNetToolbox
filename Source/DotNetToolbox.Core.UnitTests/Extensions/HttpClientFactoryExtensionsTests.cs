namespace DotNetToolbox.Extensions;

public class HttpClientFactoryExtensionsTests
{
    [Fact]
    public void CreateIdentifiedHttpClient_SetApiKeyHeader()
    {
        // Arrange
        const string apiKey = "abc123";
        var factory = Substitute.For<IHttpClientFactory>();
        var options = Substitute.For<IOptions<IdentifiedHttpClientOptions>>();
        options.Value.Returns(new IdentifiedHttpClientOptions
        {
            BaseAddress = "http://example.com/api/",
            ApiKey = apiKey,
            ResponseFormat = "application/text",
            CustomHeaders = new()
            {
                ["x-custom-header"] = "SomeValue",
            },
        });

        var client = new HttpClient();
        factory.CreateClient(Arg.Any<string>()).Returns(client);

        // Act
        var result = factory.CreateIdentifiedHttpClient(options);

        // Assert
        result.DefaultRequestHeaders.Contains("x-api-key").Should().BeTrue();
        result.DefaultRequestHeaders.GetValues("x-api-key").Should().Contain(apiKey);
    }

    [Fact]
    public void ConfidentialHttpClientOptions_HasAllProperties()
    {
        var clientOptions = new ConfidentialHttpClientOptions
        {
            BaseAddress = "http://example.com/api/",
            ClientId = "SomeClientId",
            Authority = "https://example.com/user",
            ClientSecret = "SomeSecret",
            Scopes = new[] { "SomeScope" },
            CustomHeaders = new()
            {
                ["x-custom-header"] = "SomeValue",
            },
        };

        // Assert
        clientOptions.BaseAddress.Should().Be("http://example.com/api/");
        clientOptions.ClientId.Should().Be("SomeClientId");
        clientOptions.Authority.Should().Be("https://example.com/user");
        clientOptions.ClientSecret.Should().Be("SomeSecret");
        clientOptions.Scopes.Should().BeEquivalentTo(new[] { "SomeScope" });
        clientOptions.CustomHeaders.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            ["x-custom-header"] = "SomeValue",
        });
    }

    [Fact]
    public async Task CreateConfidentialHttpClient_SetAuthorizationHeader()
    {
        // Arrange
        const string token = "SomeToken";
        var tokenAcquirer = Substitute.For<ITokenAcquirer>();
        tokenAcquirer.AcquireTokenAsync(Arg.Any<ConfidentialHttpClientOptions>()).Returns($"Bearer {token}");

        var factory = Substitute.For<IHttpClientFactory>();
        HttpClientFactoryExtensions.TokenAcquirer = tokenAcquirer;
        factory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());
        var clientOptions = new ConfidentialHttpClientOptions
        {
            BaseAddress = "http://example.com/api/",
            ClientId = "SomeClientId",
            Authority = "https://example.com/user",
            ClientSecret = "SomeSecret",
            Scopes = new[] { "SomeScope" },
            CustomHeaders = new ()
            {
                ["x-custom-header"] = "SomeValue",
            },
        };

        var options = Substitute.For<IOptions<ConfidentialHttpClientOptions>>();
        options.Value.Returns(clientOptions);

        // Act
        var result = await factory.CreateConfidentialHttpClientAsync(options);

        // Assert
        result.BaseAddress.Should().Be(clientOptions.BaseAddress);
        result.DefaultRequestHeaders.Contains("x-custom-header").Should().BeTrue();
        result.DefaultRequestHeaders.GetValues("x-custom-header").Should().Contain("SomeValue");
        result.DefaultRequestHeaders.Contains("Authorization").Should().BeTrue();

        result.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        result.DefaultRequestHeaders.Authorization!.Scheme.Should().Be("Bearer");
        result.DefaultRequestHeaders.Authorization.Parameter.Should().Be(token);
    }
}
