namespace DotNetToolbox.AI.OpenAI;

public class StandardAgent(World world,
                                AgentOptions options,
                                Persona persona,
                                IHttpClientProvider httpClientProvider,
                                ILogger<StandardAgent> logger)
    : StandardAgent<StandardAgent>(world, options, persona, httpClientProvider, logger) {

    public StandardAgent(AgentOptions options,
                       Persona persona,
                       IDateTimeProvider dateTime,
                       IHttpClientProvider httpClientProvider,
                       ILogger<StandardAgent> logger)
        : this(new(dateTime), options, persona, httpClientProvider, logger) {
    }
}

public abstract class StandardAgent<TAgent>(World world,
                                          AgentOptions options,
                                          Persona persona,
                                          IHttpClientProvider httpClientProvider,
                                          ILogger<TAgent> logger)
    : StandardAgent<TAgent,
        AgentOptions,
        Mapper,
        ChatRequest,
        ChatResponse>(world, options, persona, httpClientProvider, logger)
    where TAgent : StandardAgent<TAgent> {
    protected StandardAgent(AgentOptions options,
                          Persona persona,
                          IDateTimeProvider dateTime,
                          IHttpClientProvider httpClientProvider,
                          ILogger<TAgent> logger)
        : this(new(dateTime), options, persona, httpClientProvider, logger) {
    }
}
