using DotNetToolbox.AI.OpenAI.Chats;

using static DotNetToolbox.AI.OpenAI.Chats.OpenAIChatOptions;

namespace DotNetToolbox.AI.Chats;

public class OpenAIChatFactoryTests {
    private readonly OpenAIChatFactory _chatFactory;
    private readonly FakeHttpMessageHandler _httpMessageHandler;
    private readonly ILogger<OpenAIChatFactory> _logger;

    public OpenAIChatFactoryTests() {
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
        _logger = new TrackedNullLogger<OpenAIChatFactory>();
        _chatFactory = new(httpClientProvider, _logger);
    }

    [Fact]
    public void Start_ReturnsChatId() {
        // Act
        var result = _chatFactory.Create();

        // Assert
        var chat = result.Should().BeOfType<OpenAIChat>().Subject;
        chat.Id.Should().NotBeNullOrEmpty();
        chat.Options.FrequencyPenalty.Should().Be(DefaultFrequencyPenalty);
        chat.Options.PresencePenalty.Should().Be(DefaultPresencePenalty);
        chat.Options.MaximumTokensPerMessage.Should().Be(DefaultMaximumTokensPerMessage);
        chat.Options.NumberOfChoices.Should().Be(DefaultNumberOfChoices);
        chat.Options.Temperature.Should().Be(DefaultTemperature);
        chat.Options.MinimumTokenProbability.Should().Be(DefaultTopProbability);
        chat.Options.UseStreaming.Should().BeTrue();
        chat.Options.StopSequences.Should().BeEmpty();
        chat.Options.Tools.Should().BeEmpty();
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Debug, $"Chat '{chat.Id}' started.");
    }

    [Fact]
    public void Start_WithConfiguration_ReturnsChatId() {
        // Act
        var result = _chatFactory.Create(opt => {
            opt.FrequencyPenalty = 1.5m;
            opt.PresencePenalty = 1.1m;
            opt.MaximumTokensPerMessage = 100000;
            opt.NumberOfChoices = 2;
            opt.Temperature = 0.7m;
            opt.MinimumTokenProbability = 0.5m;
            opt.StopSequences.Add("Abort!");
            opt.StopSequences.Add("Stop!");
            opt.Tools.Add(new("tool1", "MyFunction1", description: "This is my first custom function"));
            opt.Tools.Add(new("tool2", "MyFunction2", description: "This is my second custom function"));
        });

        // Assert
        var chat = result.Should().BeOfType<OpenAIChat>().Subject;
        chat.Id.Should().NotBeNullOrEmpty();
        chat.Options.FrequencyPenalty.Should().Be(1.5m);
        chat.Options.PresencePenalty.Should().Be(1.1m);
        chat.Options.MaximumTokensPerMessage.Should().Be(100000);
        chat.Options.NumberOfChoices.Should().Be(2);
        chat.Options.Temperature.Should().Be(0.7m);
        chat.Options.MinimumTokenProbability.Should().Be(0.5m);
        chat.Options.UseStreaming.Should().BeTrue();
        chat.Options.StopSequences.Should().BeEquivalentTo("Abort!", "Stop!");
        chat.Options.Tools.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Debug, $"Chat '{chat.Id}' started.");
    }

    [Fact]
    public void Start_WithInvalidConfiguration_Throws() {
        // Act
        var result = () => _chatFactory.Create(opt => {
            opt.FrequencyPenalty = 2.5m;
            opt.PresencePenalty = 2.1m;
            opt.NumberOfChoices = 10;
            opt.MaximumTokensPerMessage = 100;
            opt.Temperature = 2.7m;
            opt.MinimumTokenProbability = 1.5m;
            opt.StopSequences.Add("Abort1!");
            opt.StopSequences.Add("");
            opt.StopSequences.Add(null!);
            opt.StopSequences.Add("  ");
            opt.StopSequences.Add("Abort5!");
        });

        // Assert
        result.Should().Throw<ValidationException>();
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Error, "Failed to Create a new chat.");
    }

    [Fact]
    public void Start_WithFaultyRepository_Throws() {
        // Act
        var result = () => _chatFactory.Create();

        // Assert
        result.Should().Throw<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Creating new chat...");
        _logger.Should().Contain(LogLevel.Error, "Failed to Create a new chat.");
    }

    [Fact]
    public async Task SendMessage_WithInvalidChatId_ReturnsReply() {
        // Act
        await _chatFactory.SendMessage(default!, new("user", [new("text", "testMessage")]));

        // Assert
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Chat 'testId' not found.");
    }

    [Fact]
    public async Task SendMessage_ReturnsReply() {
        // Arrange
        var options = new OpenAIChatOptions {
            FrequencyPenalty = 1.5m,
            PresencePenalty = 1.1m,
            MaximumTokensPerMessage = 100000,
            NumberOfChoices = 2,
            Temperature = 0.7m,
            MinimumTokenProbability = 0.5m,
        };
        options.StopSequences.Add("Abort!");
        options.StopSequences.Add("Stop!");
        options.Tools.Add(new("tool1", "MyFunction1", description: "This is my first custom function"));
        options.Tools.Add(new("tool2", "MyFunction2", description: "This is my second custom function"));

        var chat = new OpenAIChat(options);
        var message = new OpenAIChatResponseMessage {
            Content = "testReply",
        };
        var choice = new OpenAIChatResponseChoice {
            Message = message,
        };
        var response = new OpenAIChatResponse {
            Id = "testId",
            Choices = [choice],
        };
        _httpMessageHandler.SetOkResponse(response);
        chat.Messages.Add(new("user", [new("text", "testMessage")]));

        // Act
        await chat.Submit();

        // Assert
        chat.Options.Model.Should().Be(DefaultChatModel);
        chat.Messages.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Reply for chat 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_ReturnsInvalidReply() {
        // Arrange
        var chat = new OpenAIChat();
        var choice = new OpenAIChatResponseChoice {
            Message = null!,
        };
        var response = new OpenAIChatResponse {
            Id = "testId",
            Choices = [choice],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        await _chatFactory.SendMessage(chat, new("user", [new("text", "testMessage")]));

        // Assert
        chat.Options.Model.Should().Be(DefaultChatModel);
        chat.Messages.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Invalid reply received for chat 'testId'.");
    }

    [Fact]
    public async Task SendMessage_ReturnsDelta() {
        // Arrange
        var chat = new OpenAIChat();
        var message = new OpenAIChatResponseMessage {
            Content = "testReply",
        };
        var choice = new OpenAIChatResponseChoice {
            Delta = message,
        };
        var response = new OpenAIChatResponse {
            Id = "testId",
            Choices = [choice],
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        await _chatFactory.SendMessage(chat, new("user", [new("text", "testMessage")]));

        // Assert
        chat.Options.Model.Should().Be(DefaultChatModel);
        chat.Messages.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Reply for chat 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_WithEmptyReply_ReturnsEmptyString() {
        // Arrange
        var chat = new OpenAIChat();
        var response = new OpenAIChatResponse {
            Id = "testId",
        };
        _httpMessageHandler.SetOkResponse(response);

        // Act
        await _chatFactory.SendMessage(chat, new("user", [new("text", "testMessage")]));

        // Assert
        chat.Options.Model.Should().Be("some-model");
        chat.Messages.Should().ContainSingle();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Invalid reply received for chat 'testId'.");
    }

    [Fact]
    public async Task SendMessage_WithFaultyConnection_Throws() {
        // Arrange
        var chat = new OpenAIChat();
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => _chatFactory.SendMessage(chat, new("user", [new("text", "testMessage")]));

        // Assert
        chat.Options.Model.Should().Be("gpt-3.5-turbo-1106");
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to chat 'testId'...");
        _logger.Should().Contain(LogLevel.Error, "Failed to send a message to 'testId'.");
    }
}
