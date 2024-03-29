namespace DotNetToolbox.OpenAI.Chats;

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
        _logger = new TrackedNullLogger<ChatHandler>();
        _chatHandler = new(_repository, httpClientProvider, _logger);
    }

    [Fact]
    public async Task Create_ReturnsChatId() {
        // Arrange
        _repository.Add(Arg.Any<Chat>()).Returns(Task.CompletedTask);

        // Act
        var result = await _chatHandler.Create();

        // Assert
        var chat = result.Should().BeOfType<Chat>().Subject;
        chat.Id.Should().NotBeNullOrEmpty();
        chat.Options.FrequencyPenalty.Should().Be(DefaultFrequencyPenalty);
        chat.Options.PresencePenalty.Should().Be(DefaultPresencePenalty);
        chat.Options.MaximumTokensPerMessage.Should().Be(DefaultMaximumTokensPerMessage);
        chat.Options.NumberOfChoices.Should().Be(DefaultNumberOfChoices);
        chat.Options.Temperature.Should().Be(DefaultTemperature);
        chat.Options.TopProbability.Should().Be(DefaultTopProbability);
        chat.Options.UseStreaming.Should().BeFalse();
        chat.Options.StopSignals.Should().BeEmpty();
        chat.Options.Tools.Should().BeEmpty();
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Debug, $"Chat '{chat.Id}' created.");
    }

    [Fact]
    public async Task Create_WithConfiguration_ReturnsChatId() {
        // Arrange
        _repository.Add(Arg.Any<Chat>()).Returns(Task.CompletedTask);

        // Act
        var result = await _chatHandler.Create("some-model",
                                               opt => {
                                                   opt.WithFrequencyPenalty(1.5m);
                                                   opt.WithPresencePenalty(1.1m);
                                                   opt.WithMaximumTokensPerMessage(100000);
                                                   opt.WithNumberOfChoices(2);
                                                   opt.WithTemperature(0.7m);
                                                   opt.WithTopProbability(0.5m);
                                                   opt.EnableStreaming();
                                                   opt.AddStopSignal("Abort!");
                                                   opt.AddStopSignal("Stop!");
                                                   opt.AddTool(new() {
                                                       Name = "MyFunction1",
                                                       Description = "This is my first custom function",
                                                   });
                                                   opt.AddTool(new() {
                                                       Name = "MyFunction2",
                                                       Description = "This is my second custom function",
                                                   });
                                               });

        // Assert
        var chat = result.Should().BeOfType<Chat>().Subject;
        chat.Id.Should().NotBeNullOrEmpty();
        chat.Options.FrequencyPenalty.Should().Be(1.5m);
        chat.Options.PresencePenalty.Should().Be(1.1m);
        chat.Options.MaximumTokensPerMessage.Should().Be(100000);
        chat.Options.NumberOfChoices.Should().Be(2);
        chat.Options.Temperature.Should().Be(0.7m);
        chat.Options.TopProbability.Should().Be(0.5m);
        chat.Options.UseStreaming.Should().BeTrue();
        chat.Options.StopSignals.Should().BeEquivalentTo("Abort!", "Stop!");
        chat.Options.Tools.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Debug, $"Chat '{chat.Id}' created.");
    }

    [Fact]
    public async Task Create_WithInvalidConfiguration_Throws() {
        // Arrange
        _repository.Add(Arg.Any<Chat>()).Returns(Task.CompletedTask);

        // Act
        var result = () => _chatHandler.Create("some-model",
                                               opt => {
                                                   opt.WithFrequencyPenalty(2.5m);
                                                   opt.WithPresencePenalty(2.1m);
                                                   opt.WithNumberOfChoices(10);
                                                   opt.WithMaximumTokensPerMessage(100);
                                                   opt.WithTemperature(2.7m);
                                                   opt.WithTopProbability(1.5m);
                                                   opt.AddStopSignal("Abort1!");
                                                   opt.AddStopSignal("");
                                                   opt.AddStopSignal(null!);
                                                   opt.AddStopSignal("  ");
                                                   opt.AddStopSignal("Abort5!");
                                               });

        // Assert
        await result.Should().ThrowAsync<ValidationException>();
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Error, "Failed to create a new chat.");
    }

    [Fact]
    public async Task Create_WithFaultyRepository_Throws() {
        // Arrange
        _repository.Add(Arg.Any<Chat>()).ThrowsAsync(new InvalidOperationException("Break!"));

        // Act
        var result = () => _chatHandler.Create("some-model");

        // Assert
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Error, "Failed to create a new chat.");
    }

    [Fact]
    public async Task SendMessage_WithInvalidChatId_ReturnsReply() {
        // Arrange
        _repository.GetById(Arg.Any<string>()).Returns(default(Chat));

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        result.Should().BeEmpty();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Chat 'testId' not found.");
    }

    [Fact]
    public async Task SendMessage_ReturnsReply() {
        // Arrange
        var builder = new ChatOptionsBuilder();
        builder.WithFrequencyPenalty(1.5m);
        builder.WithPresencePenalty(1.1m);
        builder.WithMaximumTokensPerMessage(100000);
        builder.WithNumberOfChoices(2);
        builder.WithTemperature(0.7m);
        builder.WithTopProbability(0.5m);
        builder.EnableStreaming();
        builder.AddStopSignal("Abort!");
        builder.AddStopSignal("Stop!");
        builder.AddTool(new() {
            Name = "MyFunction1",
            Description = "This is my first custom function",
        });
        builder.AddTool(new() {
            Name = "MyFunction2",
            Description = "This is my second custom function",
        });

        var chat = new Chat(builder.Build());
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        var response = new CompletionResponse {
            Id = "testId",
            Choices = [
                new MessageChoice {
                    Message = new Message {
                        Content = SerializeToElement("testReply"),
                    } with { Name = "SomeName" },
                },
            ],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Options.Model.Should().Be(DefaultChatModel);
        chat.Messages.Should().HaveCount(2);
        result.Should().NotBeNull();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Reply for chat 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_ReturnsInvalidReply() {
        // Arrange
        var chat = new Chat { Id = "testId" };
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        var response = new CompletionResponse {
            Id = "testId",
            Choices = [
                new MessageChoice {
                    Message = new() {
                        Content = SerializeToElement<string?>(null),
                    },
                },
            ],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Options.Model.Should().Be(DefaultChatModel);
        chat.Messages.Should().HaveCount(2);
        result.Should().BeEmpty();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Invalid reply received for chat 'testId'.");
    }

    [Fact]
    public async Task SendMessage_ReturnsDelta() {
        // Arrange
        var chat = new Chat { Id = "testId" };
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        var response = new CompletionResponse {
            Id = "testId",
            Choices = [
                new DeltaChoice {
                    Delta = new() {
                        Content = SerializeToElement("testReply"),
                    },
                },
            ],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Options.Model.Should().Be(DefaultChatModel);
        chat.Messages.Should().HaveCount(2);
        result.Should().NotBeNull();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Reply for chat 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_WithEmptyReply_ReturnsEmptyString() {
        // Arrange
        var chat = new Chat("some-model") {
            Id = "testId",
        };
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        var response = new CompletionResponse {
            Id = "testId",
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        var result = await _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Options.Model.Should().Be("some-model");
        chat.Messages.Should().ContainSingle();
        result.Should().BeEmpty();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Invalid reply received for chat 'testId'.");
    }

    [Fact]
    public async Task SendMessage_WithFaultyConnection_Throws() {
        // Arrange
        var chat = new Chat(new ChatOptions("some-model")) {
            Id = "testId",
        };
        _repository.GetById(Arg.Any<string>()).Returns(chat);
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => _chatHandler.SendMessage("testId", "testMessage");

        // Assert
        chat.Options.Model.Should().Be("some-model");
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Error, "Failed to send a message to 'testId'.");
    }
}
