namespace DotNetToolbox.AI.Anthropic;

public class BackgroundAgent(World world,
                                      AgentOptions options,
                                      Persona persona,
                                      IHttpClientProvider httpClientProvider,
                                      ILogger<BackgroundAgent> logger)
    : BackgroundAgent<
        BackgroundAgent,
        AgentOptions,
        Mapper,
        ChatRequest,
        ChatResponse>(world, options, persona, httpClientProvider, logger) {
    public BackgroundAgent(AgentOptions options,
                                    Persona persona,
                                    IDateTimeProvider dateTime,
                                    IHttpClientProvider httpClientProvider,
                                    ILogger<BackgroundAgent> logger)
        : this(new World(dateTime), options, persona, httpClientProvider, logger) {
    }
}
