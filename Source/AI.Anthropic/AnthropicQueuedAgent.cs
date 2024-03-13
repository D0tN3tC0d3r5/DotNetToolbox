namespace DotNetToolbox.AI.Anthropic;

public class AnthropicQueuedAgent(World world,
                                  AgentOptions options,
                                  Persona persona,
                                  IMapper mapper,
                                  IHttpClientProvider httpClientProvider,
                                  ILogger<AnthropicQueuedAgent> logger)
    : QueuedAgent<
        AnthropicQueuedAgent,
        AgentOptions,
        ChatRequest,
        ChatResponse>(world, options, persona, mapper, httpClientProvider, logger) {
    public AnthropicQueuedAgent(IEnvironment environment,
                                AgentOptions options,
                                Persona persona,
                                IMapper mapper,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(new World(environment), options, persona, mapper, httpClientProvider, logger) {
    }
}
