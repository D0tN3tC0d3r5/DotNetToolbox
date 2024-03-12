using DotNetToolbox.AI.Actors;

namespace DotNetToolbox.AI.Anthropic;

public class AnthropicQueuedActor(IAgent agent,
                             World world,
                             IHttpClientProvider httpClientProvider,
                             ILogger<AnthropicQueuedActor> logger)
    : QueuedActor<AnthropicQueuedActor, AnthropicAgentOptions, AnthropicChatRequest, AnthropicChatResponse>(agent, world, httpClientProvider, logger) {

    public AnthropicQueuedActor(World world,
                           IHttpClientProvider httpClientProvider,
                           ILogger<AnthropicQueuedActor> logger)
        : this(new AnthropicAgent(), world, httpClientProvider, logger) {
    }

    public AnthropicQueuedActor(IAgent agent,
                           IEnvironment environment,
                           IHttpClientProvider httpClientProvider,
                           ILogger<AnthropicQueuedActor> logger)
        : this(agent, new World(environment), httpClientProvider, logger) {
    }

    public AnthropicQueuedActor(IEnvironment environment,
                           IHttpClientProvider httpClientProvider,
                           ILogger<AnthropicQueuedActor> logger)
        : this(new World(environment), httpClientProvider, logger) {
    }

    protected override AnthropicChatRequest CreateRequest(IRequestSource source, IChat chat) {
        var options = (AnthropicAgentOptions)Agent.Options;
        return new() {
            Model = options.Model,
            Temperature = options.Temperature,
            MaximumOutputTokens = options.MaximumOutputTokens,
            StopSequences = options.StopSequences.Count == 0 ? null : [.. options.StopSequences],
            MinimumTokenProbability = options.TokenProbabilityCutOff,
            UseStreaming = options.UseStreaming,
            System = CreateSystemMessage(chat),
            Messages = chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),
            MaximumTokenSamples = options.MaximumTokensToSample,
        };
    }

    protected override Message CreateResponse(IChat chat, AnthropicChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
