namespace DotNetToolbox.AI.Anthropic;

public class AnthropicRunner(IAgent agent,
                             World world,
                             IHttpClientProvider httpClientProvider,
                             ILogger<AnthropicRunner> logger)
    : QueuedRunner<AnthropicRunner, AnthropicAgentOptions, AnthropicChatRequest, AnthropicChatResponse>(agent, world, httpClientProvider, logger) {

    public AnthropicRunner(World world,
                           IHttpClientProvider httpClientProvider,
                           ILogger<AnthropicRunner> logger)
        : this(new AnthropicAgent(), world, httpClientProvider, logger) {
    }

    public AnthropicRunner(IAgent agent,
                           IEnvironment environment,
                           IHttpClientProvider httpClientProvider,
                           ILogger<AnthropicRunner> logger)
        : this(agent, new World(environment), httpClientProvider, logger) {
    }

    public AnthropicRunner(IEnvironment environment,
                           IHttpClientProvider httpClientProvider,
                           ILogger<AnthropicRunner> logger)
        : this(new World(environment), httpClientProvider, logger) {
    }

    protected override AnthropicChatRequest CreateRequest(RequestPackage package) {
        var options = (AnthropicAgentOptions)Agent.Options;
        return new() {
                         Model = options.Model,
                         Temperature = options.Temperature,
                         MaximumOutputTokens = options.MaximumOutputTokens,
                         StopSequences = options.StopSequences.Count == 0
                                             ? null
                                             : [.. options.StopSequences],
                         MinimumTokenProbability = options.TokenProbabilityCutOff,
                         UseStreaming = options.UseStreaming,
                         System = CreateSystemMessage(),
                         Messages = package.Chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),
                         MaximumTokenSamples = options.MaximumTokensToSample,
                     };
    }

    protected override Message CreateResponseMessage(IChat chat, AnthropicChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
