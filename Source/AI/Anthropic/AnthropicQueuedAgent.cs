namespace DotNetToolbox.AI.Anthropic;

public class AnthropicQueuedAgent(World world,
                                  AnthropicAgentOptions options,
                                  IPersona persona,
                                  IHttpClientProvider httpClientProvider,
                                  ILogger<AnthropicQueuedAgent> logger)
    : QueuedAgent<
        AnthropicQueuedAgent,
        AnthropicAgentOptions,
        AnthropicChatRequest,
        AnthropicChatResponse>(world,
                               options,
                               persona,
                               httpClientProvider,
                               logger) {

    public AnthropicQueuedAgent(IEnvironment environment,
                                AnthropicAgentOptions options,
                                IPersona persona,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    public AnthropicQueuedAgent(World world,
                                AnthropicAgentOptions options,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(world, options, new Persona(), httpClientProvider, logger) {
    }

    public AnthropicQueuedAgent(IEnvironment environment,
                                AnthropicAgentOptions options,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(new World(environment), options, new Persona(), httpClientProvider, logger) {
    }

    public AnthropicQueuedAgent(World world,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(world, new(), new Persona(), httpClientProvider, logger) {
    }

    public AnthropicQueuedAgent(IEnvironment environment,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(new World(environment), new(), new Persona(), httpClientProvider, logger) {
    }

    protected override AnthropicChatRequest CreateRequest(IConsumer source, IChat chat)
        => new() {
            Model = Options.Model,
            Temperature = Options.Temperature,
            MaximumOutputTokens = Options.MaximumOutputTokens,
            StopSequences = Options.StopSequences.Count == 0 ? null : [.. Options.StopSequences],
            MinimumTokenProbability = Options.TokenProbabilityCutOff,
            UseStreaming = Options.UseStreaming,
            System = CreateSystemMessage(chat),
            Messages = chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),
            MaximumTokenSamples = Options.MaximumTokensToSample,
        };

    protected override Message CreateResponse(IChat chat, AnthropicChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
