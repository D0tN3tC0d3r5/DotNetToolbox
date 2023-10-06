namespace DotNetToolbox.Http;

public class HttpClientBuilderTests {
    private readonly HttpClientOptions _defaultOptions = new() {
        BaseAddress = "http://example.com/api/",
        ResponseFormat = "application/json",
    };

    [Fact]
    public void Build_WithBasicOptions_ReturnsHttpClient() {
        // Arrange
        var clientOptions = _defaultOptions;
        clientOptions.ResponseFormat = "text/xml";
        clientOptions.CustomHeaders = new() {
            ["x-custom-string"] = new[] { "SomeValue" },
            ["x-custom-int"] = new[] { "42" },
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
        var clientOptions = _defaultOptions;
        clientOptions.Authorization = new() {
            Type = HttpClientAuthorizationType.ApiKey,
            Value = "abc123",
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
        builder.UseApiKey();
        var result = () => builder.Build();

        // Assert
        result.Should().Throw<AggregateException>();
    }

    [Fact]
    public void UseAuthorization_AddsApiKeyHeader() {
        // Arrange
        var clientOptions = _defaultOptions;
        clientOptions.Authorization = new() {
            Type = HttpClientAuthorizationType.ApiKey,
            Scheme = HttpClientAuthorizationScheme.Bearer,
            Value = "SomeToken",
        };
        var builder = CreateHttpClientBuilder(clientOptions);

        // Act
        //builder.UseAuthorization();
        //var result = builder.Build();

        //// Assert
        //var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        //authorization.Scheme.Should().Be("Bearer");
        //authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseAuthorization_WithNoValue_AddsApiKeyHeader() {
        // Arrange
        var builder = CreateHttpClientBuilder(_defaultOptions);

        // Act
        //var result = builder.UseAuthorization;

        //// Assert
        //result.Should().Throw<ArgumentException>();
    }

    //[Fact]
    //public async Task AcquireTokenAsync_WithTokenSet_AddsAuthenticationHeader() {
    //    // Arrange
    //    var clientOptions = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //        Authorization = "Bearer SomeToken"
    //    };
    //    var builder = CreateHttpClientBuilder(clientOptions);

    //    // Act
    //    await builder.AuthorizeClientAsync();
    //    var result = builder.Build();

    //    // Assert
    //    var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
    //    authorization.Scheme.Should().Be("Bearer");
    //    authorization.Parameter.Should().Be("SomeToken");
    //}

    //[Fact]
    //public async Task AcquireTokenAsync_WithNoAuthenticationValues_Throws() {
    //    // Arrange
    //    var builder = CreateHttpClientBuilder(_defaultOptions);

    //    // Act
    //    var result = builder.AuthorizeClientAsync;

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //                 .WithInnerException<ArgumentException>();
    //}

    //[Fact]
    //public async Task AcquireTokenAsync_WithFailedTokenAcquisition_Throws() {
    //    // Arrange
    //    var clientOptions = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //    };
    //    var builder = CreateHttpClientBuilder(clientOptions);

    //    // Act
    //    var result = builder.AuthorizeClientAsync;

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //                 .WithInnerException<MsalServiceException>();
    //}

    //[Fact]
    //public async Task AuthorizeByCodeAsync_WithTokenSet_AddsAuthenticationHeader() {
    //    // Arrange
    //    var clientOptions = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //        Authorization = "Bearer SomeToken"
    //    };
    //    var builder = CreateHttpClientBuilder(clientOptions);

    //    // Act
    //    await builder.AuthorizeByCodeAsync("SomeCode");
    //    var result = builder.Build();

    //    // Assert
    //    var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
    //    authorization.Scheme.Should().Be("Bearer");
    //    authorization.Parameter.Should().Be("SomeToken");
    //}

    //[Fact]
    //public async Task AuthorizeByCodeAsync_WithNoAuthenticationValues_Throws() {
    //    // Arrange
    //    var builder = CreateHttpClientBuilder(_defaultOptions);

    //    // Act
    //    var result = () => builder.AuthorizeByCodeAsync("SomeCode");

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //       .WithInnerException<ArgumentException>();
    //}

    //[Fact]
    //public async Task AuthorizeByCodeAsync_WithFailedTokenAcquisition_Throws() {
    //    // Arrange
    //    var clientOptions = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //    };
    //    var builder = CreateHttpClientBuilder(clientOptions);

    //    // Act
    //    var result = () => builder.AuthorizeByCodeAsync("SomeCode");

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //       .WithInnerException<MsalServiceException>();
    //}

    //[Fact]
    //public async Task AuthorizeAccountAsync_WithTokenSet_AddsAuthenticationHeader() {
    //    // Arrange
    //    var clientOptions = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //        Authorization = "Bearer SomeToken"
    //    };
    //    var builder = CreateHttpClientBuilder(clientOptions);
    //    var account = Substitute.For<IAccount>();

    //    // Act
    //    await builder.AuthorizeAccountAsync(account);
    //    var result = builder.Build();

    //    // Assert
    //    var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
    //    authorization.Scheme.Should().Be("Bearer");
    //    authorization.Parameter.Should().Be("SomeToken");
    //}

    //[Fact]
    //public async Task AuthorizeAccountAsync_WithNoAuthenticationValues_Throws() {
    //    // Arrange
    //    var builder = CreateHttpClientBuilder(_defaultOptions);
    //    var account = Substitute.For<IAccount>();

    //    // Act
    //    var result = () => builder.AuthorizeAccountAsync(account);

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //       .WithInnerException<ArgumentException>();
    //}

    //[Fact]
    //public async Task AuthorizeAccountAsync_WithFailedTokenAcquisition_Throws() {
    //    // Arrange
    //    var clientOptions = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //    };
    //    var builder = CreateHttpClientBuilder(clientOptions);
    //    var account = Substitute.For<IAccount>();

    //    // Act
    //    var result = () => builder.AuthorizeAccountAsync(account);

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //       .WithInnerException<MsalServiceException>();
    //}

    private static HttpClientBuilder CreateHttpClientBuilder(HttpClientOptions optionsValue) {
        var factory = Substitute.For<IHttpClientFactory>();


        var values = new Dictionary<string, string?> {
            ["HttpClientOptions:BaseAddress"] = optionsValue.BaseAddress,
            ["HttpClientOptions:ResponseFormat"] = optionsValue.ResponseFormat,
        };

        foreach (var customHeader in optionsValue.CustomHeaders) {
            var items = customHeader.Value.ToArray();
            for (var i = 0; i < items.Length; i++) {
                values[$"HttpClientOptions:CustomHeaders:{customHeader.Key}:{i+1}"] = items[i];
            }
        }

        if (optionsValue.Authorization is not null) {
            values["HttpClientOptions:Authorization:Type"] = optionsValue.Authorization?.Type.ToString();
            values["HttpClientOptions:Authorization:Value"] = optionsValue.Authorization?.Value;
        }

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(values);

        var configuration = configBuilder.Build();

        var client = new HttpClient();
        factory.CreateClient(Arg.Any<string>()).Returns(client);

        // Act
        var result = new HttpClientBuilder(factory, configuration);
        return result;
    }
}
