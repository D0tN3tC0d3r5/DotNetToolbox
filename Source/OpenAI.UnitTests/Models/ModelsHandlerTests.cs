namespace DotNetToolbox.OpenAI.Models;

public class ModelsHandlerTests {
    private readonly ModelsHandler _modelsHandler;
    private readonly ILogger<ModelsHandler> _logger;
    private readonly FakeHttpMessageHandler _httpMessageHandler;

    public ModelsHandlerTests() {
        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection.Value.Returns("SomeAPIKey");
        var httpClientProvider = Substitute.For<IOpenAIHttpClientProvider>();
        _httpMessageHandler = new();
        var httpClient = new HttpClient(_httpMessageHandler, true) {
            BaseAddress = new("https://somehost.com/"),
        };
        httpClientProvider.GetHttpClient(Arg.Any<Action<OpenAIHttpClientOptionsBuilder>?>())
                          .Returns(httpClient);
        _logger = Substitute.For<ILogger<ModelsHandler>>();
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
        result[0].Id.Should().Be("ft:model1");
        result[0].CreatedOn.Should().Be(DateTimeOffset.Parse("2020-01-01 12:34:56"));
        result[0].OwnedBy.Should().Be("user1");
        result[0].Type.Should().Be(ModelType.Chat);
        result[0].Name.Should().Be("model1");
        result[0] = result[0] with { Name = "model1" };
        result[0].IsFineTuned.Should().BeTrue();
        result[1].Id.Should().Be("model2");
        result[1].CreatedOn.Should().Be(DateTimeOffset.Parse("2020-01-01 12:34:56"));
        result[1].OwnedBy.Should().Be("user1");
        result[1].Type.Should().Be(ModelType.Chat);
        result[1].Name.Should().Be("model2");
        result[1].IsFineTuned.Should().BeFalse();
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

    [Fact]
    public async Task GetById_ReturnsModel() {
        // Arrange
        var response = new OpenAIModel {
            Id = "model1",
            Created = DateTimeOffset.Parse("2020-01-01 12:34:56")
                                            .ToUnixTimeSeconds(),
            OwnedBy = "user1",
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _modelsHandler.GetById("testId");

        // Assert
        var subject = result.Should().BeOfType<Model>().Subject;
        subject.Id.Should().Be("model1");
        subject.CreatedOn.Should().Be(DateTimeOffset.Parse("2020-01-01 12:34:56"));
        subject.OwnedBy.Should().Be("user1");
        subject.Type.Should().Be(ModelType.Chat);
        subject.Name.Should().Be("model1");
        subject.IsFineTuned.Should().Be(false);
        _logger.Should().Contain(LogLevel.Debug, "Getting model 'testId' details...");
        _logger.Should().Contain(LogLevel.Debug, "The model 'testId' was found.");
    }

    [Fact]
    public async Task GetById_WhenIdIsNotFound_ReturnsNull() {
        // Arrange
        _httpMessageHandler.SetNotFoundResponse();

        // Act
        var result = await _modelsHandler.GetById("testId");

        // Assert
        result.Should().BeNull();
        _logger.Should().Contain(LogLevel.Debug, "Getting model 'testId' details...");
        _logger.Should().Contain(LogLevel.Debug, "The model 'testId' was not found.");
    }

    [Fact]
    public async Task GetById_WithFaultyConnection_Throws() {
        // Arrange
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => _modelsHandler.GetById("testId");

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Getting model 'testId' details...");
        _logger.Should().Contain(LogLevel.Error, "Failed to get the model 'testId' details.");
    }

    [Fact]
    public async Task Delete_ReturnsTrue() {
        var response = new DeleteResponse() {
            Id = "testId",
            Deleted = true,
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _modelsHandler.Delete("testId");

        // Assert
        result.Should().BeTrue();
        _logger.Should().Contain(LogLevel.Debug, "Deleting the model 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "The model 'testId' was deleted.");
    }

    [Fact]
    public async Task Delete_WithFaultyConnection_Throws() {
        // Arrange
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => _modelsHandler.Delete("testId");

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Deleting the model 'testId'...");
        _logger.Should().Contain(LogLevel.Error, "Failed to delete the model 'testId'.");
    }
}
