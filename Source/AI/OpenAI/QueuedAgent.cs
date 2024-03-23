namespace DotNetToolbox.AI.OpenAI;

public class QueuedAgent(World world,
                               AgentOptions options,
                               Persona persona,
                               IHttpClientProvider httpClientProvider,
                               ILogger<QueuedAgent> logger)
    : QueuedAgent<QueuedAgent,
        AgentOptions,
        Mapper,
        ChatRequest,
        ChatResponse>(world, options, persona, httpClientProvider, logger) {
    public QueuedAgent(AgentOptions options,
                             Persona persona,
                             IDateTimeProvider dateTime,
                             IHttpClientProvider httpClientProvider,
                             ILogger<QueuedAgent> logger)
        : this(new(dateTime), options, persona, httpClientProvider, logger) {
    }
}
