using DotNetToolbox.AI.OpenAI.Models;

namespace DotNetToolbox.AI.Models;

public class ModelsHandlerTests {
    private readonly ModelsHandler _modelsHandler;
    private readonly ILogger<ModelsHandler> _logger;
    private readonly FakeHttpMessageHandler _httpMessageHandler;

    public ModelsHandlerTests() {
        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection.Value.Returns("SomeAPIKey");
        var httpClientProvider = Substitute.For<IHttpClientProvider>();
        _httpMessageHandler = new();
        var httpClient = new HttpClient(_httpMessageHandler, true) {
            BaseAddress = new("https://somehost.com/"),
        };
        httpClientProvider.GetHttpClient(Arg.Any<string?>(),
                                         Arg.Any<Action<HttpClientOptionsBuilder>?>())
                          .Returns(httpClient);
        _logger = new TrackedNullLogger<ModelsHandler>();
        _modelsHandler = new(httpClientProvider, _logger);
    }

    [Fact]
    public async Task Get_ReturnsModels() {
        // Arrange
        var response = new ModelsResponse {
            Data = [
                new() {
                    Id = "ft:model1",
                    Created = DateTimeOffset.Parse("2020-01-01 12:34:56").ToUnixTimeSeconds(),
                    OwnedBy = "user1",
                },
                new() {
                    Id = "model2",
                    Created = DateTimeOffset.Parse("2020-01-01 12:34:56").ToUnixTimeSeconds(),
                    OwnedBy = "user1",
                },
            ],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _modelsHandler.Get();

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().Be("ft:model1");
        result[1].Should().Be("model2");
        _logger.Should().Contain(LogLevel.Debug, "Getting list of models...");
        _logger.Should().Contain(LogLevel.Debug, "A list of 2 models was found.");
    }

    [Fact]
    public async Task Get_WithFaultyConnection_Throws() {
        // Arrange
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => _modelsHandler.Get();

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Getting list of models...");
        _logger.Should().Contain(LogLevel.Error, "Failed to get list of models.");
    }
}
