using DotNetToolbox.AI.Actors;

namespace DotNetToolbox.AI.Chats;

public class OpenAIQueuedActorTests {
    private readonly FakeHttpMessageHandler _httpMessageHandler;
    private readonly ILogger<OpenAIQueuedActor> _logger;
    private readonly IEnvironment _environment;
    private readonly IHttpClientProvider _httpClientProvider;

    public OpenAIQueuedActorTests() {
        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection.Value.Returns("SomeAPIKey");
        _httpClientProvider = Substitute.For<IHttpClientProvider>();
        _httpMessageHandler = new();
        var httpClient = new HttpClient(_httpMessageHandler, true) {
            BaseAddress = new("https://somehost.com/"),
        };
        _httpClientProvider.GetHttpClient(Arg.Any<string?>(),
                                         Arg.Any<Action<HttpClientOptionsBuilder>?>())
                          .Returns(httpClient);
        _logger = new TrackedNullLogger<OpenAIQueuedActor>();
        _environment = Substitute.For<IEnvironment>();
        _environment.DateTime.Returns(Substitute.For<IDateTimeProvider>());
        _environment.DateTime.Now.Returns(new DateTimeOffset(2001, 01, 01, 00, 00, 00, default));
    }

    [Fact]
    public async Task Start_ReturnsChatId() {
        var agent = new OpenAIAgent();
        var runner = new OpenAIQueuedActor(agent, _environment, _httpClientProvider, _logger);
        var tokenSource = new CancellationTokenSource();

        // Act
        runner.Run(tokenSource.Token);
        await Task.Delay(200, default);
        await tokenSource.CancelAsync();

        // Assert
        _logger.Should().Contain(LogLevel.Information, "Starting runner...");
        _logger.Should().Contain(LogLevel.Warning, "BackgroundActor cancellation requested!");
        _logger.Should().Contain(LogLevel.Information, "BackgroundActor stopped.");
    }

    [Fact]
    public async Task SendMessage_ReturnsReply() {
        var agent = Substitute.For<IAgent>();
        agent.Options.Returns(new OpenAIAgentOptions());
        var response = new OpenAIChatResponse("chatId") {
            Choices = [new() {
                Index = 0,
                Message = new() { Content = new Message("assistant", "Some message.") },
            }],
        };
        _httpMessageHandler.SetOkResponse(response);
        var runner = new OpenAIQueuedActor(agent, _environment, _httpClientProvider, _logger);
        var tokenSource = new CancellationTokenSource();
        var source = Substitute.For<IRequestSource>();
        var responseReceived = false;
        source.RespondWith(Arg.Any<string>(), Arg.Any<Message>(), Arg.Any<CancellationToken>())
              .Returns(_ => {
                  responseReceived = true;
                  tokenSource.Cancel();
                  return Task.CompletedTask;
              });
        runner.Run(tokenSource.Token);
        var chat = new Chat("chatId");

        // Act
        await runner.RespondTo(source, chat, default);
        while (!tokenSource.IsCancellationRequested) await Task.Delay(100, default);

        // Assert
        responseReceived.Should().BeTrue();
    }
}
