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
        _provider.RevokeAuthorization();
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
    public void GetHttpClient_MinimumOptions_ReturnsHttpClient() {
        // Act
        var result = _provider.GetHttpClient();

        // Assert
        result.BaseAddress.Should().Be("http://example.com/api/");
        result.DefaultRequestHeaders.Accept.Should().Contain(new MediaTypeWithQualityHeaderValue("application/json"));
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
}
