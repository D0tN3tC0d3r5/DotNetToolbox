namespace DotNetToolbox.AI.OpenAI;
    public class BackgroundAgent(World world,
                                   AgentOptions options,
                                   Persona persona,
                                   IHttpClientProvider httpClientProvider,
                                   ILogger<BackgroundAgent> logger)
    : BackgroundAgent<BackgroundAgent,
        AgentOptions,
        Mapper,
        ChatRequest,
        ChatResponse>(world, options, persona, httpClientProvider, logger) {
    public BackgroundAgent(AgentOptions options,
                                 Persona persona,
                                 IDateTimeProvider dateTime,
                                 IHttpClientProvider httpClientProvider,
                                 ILogger<BackgroundAgent> logger)
        : this(new(dateTime), options, persona, httpClientProvider, logger) {
    }
}
