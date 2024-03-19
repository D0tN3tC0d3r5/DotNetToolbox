using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.OpenAI;

public class BackgroundAgent(World world,
                                   AgentOptions options,
                                   Persona persona,
                                   IMapper mapper,
                                   IHttpClientProvider httpClientProvider,
                                   ILogger<BackgroundAgent> logger)
    : BackgroundAgent<BackgroundAgent,
        AgentOptions,
        ChatRequest,
        ChatResponse>(world, options, persona, mapper, httpClientProvider, logger) {
    public BackgroundAgent(AgentOptions options,
                                 Persona persona,
                                 IMapper mapper,
                                 IDateTimeProvider dateTime,
                                 IHttpClientProvider httpClientProvider,
                                 ILogger<BackgroundAgent> logger)
        : this(new World(dateTime), options, persona, mapper, httpClientProvider, logger) {
    }
}
