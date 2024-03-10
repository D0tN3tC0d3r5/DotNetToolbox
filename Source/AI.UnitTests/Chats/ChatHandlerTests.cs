using DotNetToolbox.AI.Agents;
using DotNetToolbox.AI.OpenAI.Chats;

using static DotNetToolbox.AI.OpenAI.Chats.OpenAIChatOptions;

namespace DotNetToolbox.AI.Chats;

public class OpenAIChatHandlerFactoryTests {
    private readonly OpenAIChatHandlerFactory _chatHandlerFactory;
    private readonly FakeHttpMessageHandler _httpMessageHandler;
    private readonly ILogger<OpenAIChatHandlerFactory> _logger;

    public OpenAIChatHandlerFactoryTests() {
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
        _logger = new TrackedNullLogger<OpenAIChatHandlerFactory>();
        var environment = Substitute.For<IEnvironment>();
        environment.DateTime.Returns(Substitute.For<IDateTimeProvider>());
        environment.DateTime.Now.Returns(new DateTimeOffset(2001, 01, 01, 00, 00, 00, default));
        var world = new World(environment);
        _chatHandlerFactory = new(world, httpClientProvider, _logger);
    }

    [Fact]
    public void Start_ReturnsChatId() {
        var options = new OpenAIChatOptions();

        // Act
        var result = _chatHandlerFactory.Create(options);

        // Assert
        options.FrequencyPenalty.Should().Be(DefaultFrequencyPenalty);
        options.PresencePenalty.Should().Be(DefaultPresencePenalty);
        options.MaximumOutputTokens.Should().Be(DefaultMaximumOutputTokens);
        options.NumberOfChoices.Should().Be(DefaultNumberOfChoices);
        options.Temperature.Should().Be(DefaultTemperature);
        options.MinimumTokenProbability.Should().Be(DefaultTopProbability);
        options.UseStreaming.Should().BeTrue();
        options.StopSequences.Should().BeEmpty();
        options.Tools.Should().BeEmpty();
        var handler = result.Should().BeOfType<OpenAIChatHandler>().Subject;
        handler.Chat.Id.Should().NotBeNullOrEmpty();
        _logger.Should().Contain(LogLevel.Debug, "Creating new handler...");
        _logger.Should().Contain(LogLevel.Debug, $"Handler for handler '{handler.Chat.Id}' started.");
    }

    [Fact]
    public void Start_WithConfiguration_ReturnsChatId() {
        var options = new OpenAIChatOptions {
            FrequencyPenalty = 1.5m,
            PresencePenalty = 1.1m,
            MaximumOutputTokens = 100000,
            NumberOfChoices = 2,
            Temperature = 0.7m,
            MinimumTokenProbability = 0.5m,
        };
        options.StopSequences.Add("Abort!");
        options.StopSequences.Add("Stop!");
        options.Tools.Add(new("MyFunction1", description: "This is my first custom function"));
        options.Tools.Add(new("MyFunction2", description: "This is my second custom function"));

        // Act
        var result = _chatHandlerFactory.Create(options);

        // Assert
        var handler = result.Should().BeOfType<OpenAIChatHandler>().Subject;
        handler.Chat.Id.Should().NotBeNullOrEmpty();
        options.FrequencyPenalty.Should().Be(1.5m);
        options.PresencePenalty.Should().Be(1.1m);
        options.MaximumOutputTokens.Should().Be(100000);
        options.NumberOfChoices.Should().Be(2);
        options.Temperature.Should().Be(0.7m);
        options.MinimumTokenProbability.Should().Be(0.5m);
        options.UseStreaming.Should().BeTrue();
        options.StopSequences.Should().BeEquivalentTo("Abort!", "Stop!");
        options.Tools.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Creating new handler...");
        _logger.Should().Contain(LogLevel.Debug, $"Chat '{handler.Chat.Id}' started.");
    }

    [Fact]
    public void Start_WithInvalidConfiguration_Throws() {
        var options = new OpenAIChatOptions {
            FrequencyPenalty = 2.5m,
            PresencePenalty = 2.1m,
            NumberOfChoices = 10,
            MaximumOutputTokens = 100,
            Temperature = 2.7m,
            MinimumTokenProbability = 1.5m,
        };
        options.StopSequences.Add("Abort1!");
        options.StopSequences.Add("");
        options.StopSequences.Add(null!);
        options.StopSequences.Add("  ");
        options.StopSequences.Add("Abort5!");

        // Act
        var result = () => _chatHandlerFactory.Create(options);

        // Assert
        result.Should().Throw<ValidationException>();
        _logger.Should().Contain(LogLevel.Debug, "Creating new handler...");
        _logger.Should().Contain(LogLevel.Error, "Failed to Create a new handler.");
    }

    [Fact]
    public void Start_WithFaultyRepository_Throws() {
        // Act
        var result = () => _chatHandlerFactory.Create(new());

        // Assert
        result.Should().Throw<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Creating new handler...");
        _logger.Should().Contain(LogLevel.Error, "Failed to Create a new handler.");
    }

    [Fact]
    public async Task SendMessage_ReturnsReply() {
        var opt = new OpenAIChatOptions {
            FrequencyPenalty = 1.5m,
            PresencePenalty = 1.1m,
            MaximumOutputTokens = 100000,
            NumberOfChoices = 2,
            Temperature = 0.7m,
            MinimumTokenProbability = 0.5m,
        };
        opt.StopSequences.Add("Abort!");
        opt.StopSequences.Add("Stop!");
        opt.Tools.Add(new("MyFunction1", description: "This is my first custom function"));
        opt.Tools.Add(new("MyFunction2", description: "This is my second custom function"));
        // Arrange
        var handler = _chatHandlerFactory.Create(opt);
        var agent = new Agent<OpenAIChatOptions>("Agent", new(), _chatHandlerFactory, opt);
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
        handler.Chat.Messages.Add(new("user", [new("text", "testMessage")]));

        // Act
        await handler.Submit(agent);

        // Assert
        opt.Model.Should().Be(DefaultChatModel);
        handler.Chat.Messages.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Sending message to handler 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Reply for handler 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_ReturnsInvalidReply() {
        // Arrange
        var opt = new OpenAIChatOptions();
        var handler = _chatHandlerFactory.Create(opt);
        var agent = new Agent<OpenAIChatOptions>("Agent", new(), _chatHandlerFactory, opt);
        var choice = new OpenAIChatResponseChoice {
            Message = null!,
        };
        var response = new OpenAIChatResponse {
            Id = "testId",
            Choices = [choice],
        };
        _httpMessageHandler.SetOkResponse(response);
        handler.Chat.Messages.Add(new("user", [new("text", "testMessage")]));

        // Act
        await handler.Submit(agent);

        // Assert
        opt.Model.Should().Be(DefaultChatModel);
        handler.Chat.Messages.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Sending message to handler 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Invalid reply received for handler 'testId'.");
    }

    [Fact]
    public async Task SendMessage_ReturnsDelta() {
        // Arrange
        var opt = new OpenAIChatOptions();
        var handler = _chatHandlerFactory.Create(opt);
        var agent = new Agent<OpenAIChatOptions>("Agent", new(), _chatHandlerFactory, opt);
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
        handler.Chat.Messages.Add(new("user", [new("text", "testMessage")]));

        // Act
        await handler.Submit(agent);

        // Assert
        opt.Model.Should().Be(DefaultChatModel);
        handler.Chat.Messages.Should().HaveCount(2);
        _logger.Should().Contain(LogLevel.Debug, "Sending message to handler 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Reply for handler 'testId' received.");
    }

    [Fact]
    public async Task SendMessage_WithEmptyReply_ReturnsEmptyString() {
        // Arrange
        var opt = new OpenAIChatOptions();
        var chat = _chatHandlerFactory.Create(opt);
        var agent = new Agent<OpenAIChatOptions>("Agent", new(), _chatHandlerFactory, opt);
        var response = new OpenAIChatResponse {
            Id = "testId",
        };
        _httpMessageHandler.SetOkResponse(response);
        chat.Chat.Messages.Add(new("user", [new("text", "testMessage")]));

        // Act
        await chat.Submit(agent);

        // Assert
        opt.Model.Should().Be("some-model");
        chat.Chat.Messages.Should().ContainSingle();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to handler 'testId'...");
        _logger.Should().Contain(LogLevel.Debug, "Invalid reply received for handler 'testId'.");
    }

    [Fact]
    public async Task SendMessage_WithFaultyConnection_Throws() {
        // Arrange
        var opt = new OpenAIChatOptions();
        var handler = _chatHandlerFactory.Create(opt);
        var agent = new Agent<OpenAIChatOptions>("Agent", new(), _chatHandlerFactory, opt);
        handler.Chat.Messages.Add(new("user", [new("text", "testMessage")]));
        _httpMessageHandler.ForceException(new InvalidOperationException("Break!"));

        // Act
        var result = () => handler.Submit(agent);

        // Assert
        opt.Model.Should().Be("gpt-3.5-turbo-1106");
        await result.Should().ThrowAsync<InvalidOperationException>();
        _logger.Should().Contain(LogLevel.Debug, "Sending message to handler 'testId'...");
        _logger.Should().Contain(LogLevel.Error, "Failed to send a message to 'testId'.");
    }
}
