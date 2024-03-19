using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.OpenAI;

public class StandardAgent(World world,
                                AgentOptions options,
                                Persona persona,
                                IMapper mapper,
                                IHttpClientProvider httpClientProvider,
                                ILogger<StandardAgent> logger)
    : StandardAgent<StandardAgent>(world, options, persona, mapper, httpClientProvider, logger) {

    public StandardAgent(AgentOptions options,
                       Persona persona,
                       IMapper mapper,
                       IDateTimeProvider dateTime,
                       IHttpClientProvider httpClientProvider,
                       ILogger<StandardAgent> logger)
        : this(new World(dateTime), options, persona, mapper, httpClientProvider, logger) {
    }
}

public abstract class StandardAgent<TAgent>(World world,
                                          AgentOptions options,
                                          Persona persona,
                                          IMapper mapper,
                                          IHttpClientProvider httpClientProvider,
                                          ILogger<TAgent> logger)
    : Agent<TAgent,
        AgentOptions,
        ChatRequest,
        ChatResponse>(world, options, persona, mapper, httpClientProvider, logger)
    where TAgent : StandardAgent<TAgent> {
    protected StandardAgent(AgentOptions options,
                          Persona persona,
                          IMapper mapper,
                          IDateTimeProvider dateTime,
                          IHttpClientProvider httpClientProvider,
                          ILogger<TAgent> logger)
        : this(new World(dateTime), options, persona, mapper, httpClientProvider, logger) {
    }
}
