namespace DotNetToolbox.AI.OpenAI;

public class OpenAIQueuedAgentTests {
    private readonly FakeHttpMessageHandler _httpMessageHandler;
    private readonly ILogger<QueuedAgent> _logger;
    private readonly IDateTimeProvider _dateTime;
    private readonly IHttpClientProvider _httpClientProvider;

    public OpenAIQueuedAgentTests() {
        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection.Value.Returns("SomeAPIKey");
        _httpClientProvider = Substitute.For<IHttpClientProvider>();
        _httpMessageHandler = new();
        var httpClient = new HttpClient(_httpMessageHandler, true) {
            BaseAddress = new("https://somehost.com/"),
        };
        _httpClientProvider.GetHttpClient(Arg.Any<Action<HttpClientOptionsBuilder>?>())
                          .Returns(httpClient);
        _logger = new TrackedNullLogger<QueuedAgent>();
        _dateTime = Substitute.For<IDateTimeProvider>();
        _dateTime.Now.Returns(new DateTimeOffset(2001, 01, 01, 00, 00, 00, default));
    }

    [Fact]
    public async Task Run_RunsUntilCancelled() {
        var options = new AgentOptions();
        var agent = new Persona();
        var runner = new QueuedAgent(agent, options, _dateTime, _httpClientProvider, _logger);
        var tokenSource = new CancellationTokenSource();

        // Act
        runner.Run(tokenSource.Token);
        await Task.Delay(200, default);
        await tokenSource.CancelAsync();

        // Assert
        _logger.Should().Contain(LogLevel.Information, "Start running...");
        _logger.Should().Contain(LogLevel.Warning, "Running cancellation requested!");
        _logger.Should().Contain(LogLevel.Information, "Running stopped.");
    }

    [Fact]
    public async Task RespondTo_ReturnsReply() {
        var options = new AgentOptions();
        var agent = new Persona();
        var response = new ChatResponse("chatId") {
            Choices = [new() {
                Index = 0,
                Message = new() { Content = new Message("assistant", "Some message.") },
            }],
        };
        _httpMessageHandler.SetOkResponse(response);
        var runner = new QueuedAgent(agent, options, _dateTime, _httpClientProvider, _logger);
        var tokenSource = new CancellationTokenSource();
        var source = Substitute.For<IAsyncResponseConsumer>();
        var responseReceived = false;
        source.ResponseApproved(Arg.Any<string>(), Arg.Any<Message>(), Arg.Any<CancellationToken>())
              .Returns(_ => {
                  responseReceived = true;
                  tokenSource.Cancel();
                  return Task.CompletedTask;
              });
        var chat = new Chats.Chat("chatId");

        // Act
        runner.Run(tokenSource.Token);
        await runner.SendRequest(source, chat, default);
        while (!tokenSource.IsCancellationRequested) await Task.Delay(100, default);

        // Assert
        responseReceived.Should().BeTrue();
    }
}
