using static DotNetToolbox.Http.Options.HttpClientAuthorizationType;

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
        _defaultOptions.Authorization = new() {
            Type = ApiKey,
            Token = "abc123",
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
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    [Fact]
    public void UseJsonWebToken_FromConfiguration_AddsAuthorizationHeader() {
        // Arrange
        // ReSharper disable StringLiteralTypo - Token
        const string expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.e30.QhB54iWGzYFFKAjbZvfd6OMKYxVpG0wLJgxuI9OICN4";
        // ReSharper restore StringLiteralTypo

        _defaultOptions.Authorization = new() {
            Type = Jwt,
            Secret = "ASecretValueWith256BitsOr32Chars",
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
    public void UseJsonWebToken_WithInvalidOptions_Throws() {
        // Arrange
        _defaultOptions.Authorization = new() {
            Type = Jwt,
        };
        var builder = CreateHttpClientBuilder();

        // Act
        var result = () => builder.UseJsonWebToken();

        // Assert
        var exception = result.Should().Throw<ValidationException>().Subject.First();
        exception.Errors.Should().ContainSingle();
    }

    //[Fact]
    //public async Task AcquireTokenAsync_WithTokenSet_AddsAuthenticationHeader() {
    //    // Arrange
    //    var options = _defaultOptions with {
    //        ClientId = "SomeClientId",
    //        Authority = "https://example.com/user",
    //        Secret = "SomeSecret",
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
    //        Secret = "SomeSecret",
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
    //        Secret = "SomeSecret",
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
    //        Secret = "SomeSecret",
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
    //        Secret = "SomeSecret",
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
    //        Secret = "SomeSecret",
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

        var values = SetOptions(_defaultOptions);
        values = _defaultOptions
            .Clients
            .Select(clientOptions => SetOptions(clientOptions.Value, clientOptions.Key))
            .Aggregate(values, (current, clientValues) => clientValues
            .Aggregate(current, (s, i) => s.Append(i).ToDictionary()));

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(values);

        var configuration = configBuilder.Build();

        var client = new HttpClient();
        factory.CreateClient(Arg.Any<string>()).Returns(client);

        // Act
        var result = new HttpClientBuilder(factory, configuration);
        return result;
    }

    private static Dictionary<string, string?> SetOptions(HttpClientBasicOptions options, string? name = null) {
        var clientPath = name is null ? string.Empty : $":Clients:{name}";
        var values = new Dictionary<string, string?> {
            [$"HttpClientOptions{clientPath}:BaseAddress"] = options.BaseAddress,
            [$"HttpClientOptions{clientPath}:ResponseFormat"] = options.ResponseFormat,
        };

        if (options.Authorization is not null) {
            values[$"HttpClientOptions{clientPath}:Authorization:Type"] = options.Authorization?.Type.ToString();
            values[$"HttpClientOptions{clientPath}:Authorization:Scheme"] = options.Authorization?.Scheme?.ToString();
            values[$"HttpClientOptions{clientPath}:Authorization:Token"] = options.Authorization?.Token;
            values[$"HttpClientOptions{clientPath}:Authorization:Secret"] = options.Authorization?.Secret;
        }

        foreach (var customHeader in options.CustomHeaders) {
            var items = customHeader.Value.ToArray();
            for (var i = 0; i < items.Length; i++) {
                values[$"HttpClientOptions{clientPath}:CustomHeaders:{customHeader.Key}:{i + 1}"] = items[i];
            }
        }

        return values;
    }
}
