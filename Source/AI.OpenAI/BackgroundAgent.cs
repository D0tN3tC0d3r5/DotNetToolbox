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
    public BackgroundAgent(IEnvironment environment,
                                 AgentOptions options,
                                 Persona persona,
                                 IMapper mapper,
                                 IHttpClientProvider httpClientProvider,
                                 ILogger<BackgroundAgent> logger)
        : this(new World(environment), options, persona, mapper, httpClientProvider, logger) {
    }
}
