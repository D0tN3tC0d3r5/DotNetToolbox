using System.Validation;

namespace DotNetToolbox.Http;

public class HttpClientBuilderTests {
    private HttpClientOptions _defaultOptions = new() {
        BaseAddress = "http://example.com/api/",
    };

    [Fact]
    public void Build_WithDefaultConstructor_Throws() {
        // Arrange
        _defaultOptions = new();

        // Act
        var builder = CreateHttpClientBuilder();
        var result = () => builder.Build();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().HaveCount(1);
        exception.Errors[0].Message.Should().Be("'BaseAddress' cannot be null.");
    }

    [Fact]
    public void Build_WithBasicOptions_ReturnsHttpClient() {
        // Arrange
        var options = _defaultOptions;
        options.ResponseFormat = "text/xml";
        options.CustomHeaders = new() {
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
    public void UseApiKey_FromConfiguration_AddsApiKeyHeader() {
        // Arrange
        _defaultOptions.Authorization = new() {
            Type = HttpClientAuthorizationType.ApiKey,
            Value = "abc123",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseApiKey();
        var result = builder.Build();

        // Assert
        result.DefaultRequestHeaders.GetValue("x-api-key").Should().Be("abc123");
    }

    [Fact]
    public void UseApiKey_WithInvalidOptions_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseApiKey();
        var result = () => builder.Build();

        // Assert
        result.Should().Throw<ValidationException>();
    }

    [Fact]
    public void UseJsonWebToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        _defaultOptions.Authorization = new() {
            Type = HttpClientAuthorizationType.ApiKey,
            Scheme = HttpClientAuthorizationScheme.Bearer,
            Value = "SomeToken",
        };
        var builder = CreateHttpClientBuilder();

        // Act
        builder.UseJsonWebToken();
        //var result = builder.Build();

        //// Assert
        //var authorization = result.DefaultRequestHeaders.Authorization.Should().BeOfType<AuthenticationHeaderValue>().Subject;
        //authorization.Scheme.Should().Be("Bearer");
        //authorization.Parameter.Should().Be("SomeToken");
    }

    [Fact]
    public void UseJsonWebToken_WithInvalidOptions_Throws() {
        // Arrange
        var builder = CreateHttpClientBuilder();

        // Act
        //var result = builder.UseAuthorization;

        //// Assert
        //result.Should().Throw<ArgumentException>();
    }

    //[Fact]
    //public async Task AcquireTokenAsync_WithTokenSet_AddsAuthenticationHeader() {
    //    // Arrange
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //        Authorization = "Bearer SomeToken"
    //    };
    //    var builder = CreateHttpClientBuilder(options);

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
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //    };
    //    var builder = CreateHttpClientBuilder(options);

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
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //        Authorization = "Bearer SomeToken"
    //    };
    //    var builder = CreateHttpClientBuilder(options);

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
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //    };
    //    var builder = CreateHttpClientBuilder(options);

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
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //        Authorization = "Bearer SomeToken"
    //    };
    //    var builder = CreateHttpClientBuilder(options);
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
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        ClientSecret = "SomeSecret",
    //        Scopes = new[] { "SomeScope" },
    //    };
    //    var builder = CreateHttpClientBuilder(options);
    //    var account = Substitute.For<IAccount>();

    //    // Act
    //    var result = () => builder.AuthorizeAccountAsync(account);

    //    // Assert
    //    (await result.Should()
    //                 .ThrowAsync<InvalidOperationException>())
    //       .WithInnerException<MsalServiceException>();
    //}

    private HttpClientBuilder CreateHttpClientBuilder() {
        var factory = Substitute.For<IHttpClientFactory>();


        var values = new Dictionary<string, string?> {
            ["HttpClientOptions:BaseAddress"] = _defaultOptions.BaseAddress,
            ["HttpClientOptions:ResponseFormat"] = _defaultOptions.ResponseFormat,
        };

        foreach (var customHeader in _defaultOptions.CustomHeaders) {
            var items = customHeader.Value.ToArray();
            for (var i = 0; i < items.Length; i++) {
                values[$"HttpClientOptions:CustomHeaders:{customHeader.Key}:{i+1}"] = items[i];
            }
        }

        if (_defaultOptions.Authorization is not null) {
            values["HttpClientOptions:Authorization:Type"] = _defaultOptions.Authorization?.Type.ToString();
            values["HttpClientOptions:Authorization:Value"] = _defaultOptions.Authorization?.Value;
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
