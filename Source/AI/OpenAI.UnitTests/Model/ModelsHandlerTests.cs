using DotNetToolbox.AI.OpenAI.Models;

namespace DotNetToolbox.AI.OpenAI.Model;

public sealed class ModelsHandlerTests : IDisposable {
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
        httpClientProvider.GetHttpClient().Returns(httpClient);
        _logger = new TrackedLogger<ModelsHandler>();
        _modelsHandler = new(httpClientProvider, _logger);
    }

    public void Dispose() => _httpMessageHandler.Dispose();

    [Fact]
    public async Task Get_ReturnsModels() {
        // Arrange
        var response = new ModelsResponse {
            Data = [
                new() {
                    Id = "ft:model1",
                    Name = "Model 1",
                    MaximumContextSize = 32000,
                    MaximumOutputTokens = 2048,
                    CreatedOn = DateOnly.Parse("2020-01-01"),
                    OwnedBy = "user1",
                },
                new() {
                    Id = "model2",
                    Name = "Model 2",
                    MaximumContextSize = 32000,
                    MaximumOutputTokens = 2048,
                    CreatedOn = DateOnly.Parse("2020-01-01"),
                    OwnedBy = "user1",
                },
            ],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _modelsHandler.GetIds();

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().Be("ft:model1");
        result[1].Should().Be("model2");
        _logger.Should().Have(1).LogsWith(LogLevel.Debug, "Getting list of models...");
        _logger.Should().Have(1).LogsWith(LogLevel.Debug, "A list of 2 models was found.");
    }

    [Fact]
    public async Task Get_WithFaultyConnection_Throws() {
        // Arrange
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => _modelsHandler.GetIds();

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Have(1).LogsWith(LogLevel.Debug, "Getting list of models...");
        _logger.Should().Have(1).LogsWith(LogLevel.Error, "Failed to get list of models.");
    }
}
