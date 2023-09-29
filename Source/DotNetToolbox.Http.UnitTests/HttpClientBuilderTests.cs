namespace DotNetToolbox.Http;

public class HttpClientBuilderTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
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
        var result = new HttpClientBuilder<IdentifiedHttpClientOptions>(factory, options);

        // Assert
        result.Should().NotBeNull();
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
        //// Arrange
        //const string token = "SomeToken";
        //var tokenAcquirer = Substitute.For<ITokenAcquirer>();
        //tokenAcquirer.AcquireTokenAsync(Arg.Any<ConfidentialHttpClientOptions>()).Returns($"Bearer {token}");

        //var factory = Substitute.For<IHttpClientFactory>();
        //factory.CreateClient(Arg.Any<string>()).Returns(new HttpClient());
        //var clientOptions = new ConfidentialHttpClientOptions
        //{
        //    BaseAddress = "http://example.com/api/",
        //    ClientId = "SomeClientId",
        //    Authority = "https://example.com/user",
        //    ClientSecret = "SomeSecret",
        //    Scopes = new[] { "SomeScope" },
        //    CustomHeaders = new()
        //    {
        //        ["x-custom-header"] = "SomeValue",
        //    },
        //};

        //var options = Substitute.For<IOptions<ConfidentialHttpClientOptions>>();
        //options.Value.Returns(clientOptions);

        //// Act
        //var result = await factory.CreateConfidentialHttpClientAsync(options);

        //// Assert
        //result.BaseAddress.Should().Be(clientOptions.BaseAddress);
        //result.DefaultRequestHeaders.Should().ContainKey("x-custom-header");
        //result.DefaultRequestHeaders.GetValues("x-custom-header").Should().Contain("SomeValue");
        //result.DefaultRequestHeaders.Should().ContainKey("Authorization");

        //result.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        //result.DefaultRequestHeaders.Authorization!.Scheme.Should().Be("Bearer");
        //result.DefaultRequestHeaders.Authorization.Parameter.Should().Be(token);
    }
}
