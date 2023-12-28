namespace DotNetToolbox.OpenAI.Models;

public class ChatHandlerTests {
    private readonly ChatHandler _chatHandler;
    private readonly IChatRepository _repository;
    private readonly FakeHttpMessageHandler _httpMessageHandler;
    private readonly ILogger<ChatHandler> _logger;

    public ChatHandlerTests() {
        _repository = Substitute.For<IChatRepository>();
        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection.Value.Returns("SomeAPIKey");
        var httpClientProvider = Substitute.For<IOpenAIHttpClientProvider>();
        _httpMessageHandler = new();
        var httpClient = new HttpClient(_httpMessageHandler, true) {
            BaseAddress = new("https://somehost.com/"),
        };
        httpClientProvider.GetHttpClient(Arg.Any<Action<OpenAIHttpClientOptionsBuilder>?>())
                          .Returns(httpClient);
        _logger = Substitute.For<ILogger<ChatHandler>>();
        _chatHandler = new(_repository, httpClientProvider, _logger);
    }

    [Fact]
    public async Task Create_ReturnsChatId() {
        // Arrange
        _repository.Add(Arg.Any<Chat>()).Returns(Task.CompletedTask);

        // Act
        var result = await _chatHandler.Create(new("some-model"));

        // Assert
        result.Should().NotBeNullOrEmpty();
        _logger.ShouldContain(LogLevel.Debug, "Creating new chat...");
        _logger.ShouldContain(LogLevel.Debug, $"Chat '{result}' created.");
    }

    [Fact]
    public async Task SendMessage_ReturnsReply() {
        // Arrange
        var chat = new Chat {
            Id = "testId",
            Options = new("some-model"),
        };
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        var response = new CompletionResponse {
            Id = "testId",
            Choices = [
                new MessageChoice {
                    Message = new() {
                        Content = SerializeToElement("testReply"),
                    },
                },
            ],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Messages.Should().HaveCount(2);
        result.Should().NotBeNull();
        _logger.ShouldContain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.ShouldContain(LogLevel.Debug, "Reply for chat 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_WithEmptyReply_ReturnsEmptyString() {
        // Arrange
        var chat = new Chat {
            Id = "testId",
            Options = new("some-model"),
        };
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        var response = new CompletionResponse {
            Id = "testId",
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Messages.Should().ContainSingle();
        result.Should().BeEmpty();
        _logger.ShouldContain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.ShouldContain(LogLevel.Debug, "Empty reply received for chat 'testId'.");
    }
}
