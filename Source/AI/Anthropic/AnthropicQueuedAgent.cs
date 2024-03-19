using DotNetToolbox.AI.Common;

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
    public AnthropicQueuedAgent(AgentOptions options,
                                Persona persona,
                                IMapper mapper,
                                IDateTimeProvider dateTime,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(new World(dateTime), options, persona, mapper, httpClientProvider, logger) {
    }
}
