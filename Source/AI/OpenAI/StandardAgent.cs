namespace DotNetToolbox.AI.OpenAI;

public class StandardAgent(World world,
                           Persona persona,
                           IAgentOptions options,
                           IHttpClientProvider httpClientProvider,
                           ILogger<StandardAgent> logger)
    : StandardAgent<StandardAgent>(world, persona, options, httpClientProvider, logger) {

    public StandardAgent(Persona persona,
                         IAgentOptions options,
                         IDateTimeProvider dateTime,
                         IHttpClientProvider httpClientProvider,
                         ILogger<StandardAgent> logger)
        : this(new(dateTime), persona, options, httpClientProvider, logger) {
    }
}

public abstract class StandardAgent<TAgent>(World world,
                                            Persona persona,
                                            IAgentOptions options,
                                            IHttpClientProvider httpClientProvider,
                                            ILogger<TAgent> logger)
    : StandardAgent<TAgent,
        Mapper,
        ChatRequest,
        ChatResponse>(world, persona, options, httpClientProvider, logger)
    where TAgent : StandardAgent<TAgent> {
    protected StandardAgent(Persona persona,
                            IAgentOptions options,
                            IDateTimeProvider dateTime,
                            IHttpClientProvider httpClientProvider,
                            ILogger<TAgent> logger)
        : this(new(dateTime), persona, options, httpClientProvider, logger) {
    }
}
