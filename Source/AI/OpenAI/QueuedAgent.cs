using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.OpenAI;

public class QueuedAgent(World world,
                               AgentOptions options,
                               Persona persona,
                               IMapper mapper,
                               IHttpClientProvider httpClientProvider,
                               ILogger<QueuedAgent> logger)
    : QueuedAgent<QueuedAgent,
        AgentOptions,
        ChatRequest,
        ChatResponse>(world, options, persona, mapper, httpClientProvider, logger) {
    public QueuedAgent(AgentOptions options,
                             Persona persona,
                             IMapper mapper,
                             IDateTimeProvider dateTime,
                             IHttpClientProvider httpClientProvider,
                             ILogger<QueuedAgent> logger)
        : this(new World(dateTime), options, persona, mapper, httpClientProvider, logger) {
    }
}
