namespace DotNetToolbox.AI.OpenAI;

public class QueuedAgent(World world,
                         Persona persona,
                         IAgentOptions options,
                         IHttpClientProvider httpClientProvider,
                         ILogger<QueuedAgent> logger)
    : QueuedAgent<QueuedAgent,
        Mapper,
        ChatRequest,
        ChatResponse>(world, persona, options, httpClientProvider, logger) {
    public QueuedAgent(Persona persona,
                       IAgentOptions options,
                       IDateTimeProvider dateTime,
                       IHttpClientProvider httpClientProvider,
                       ILogger<QueuedAgent> logger)
        : this(new(dateTime), persona, options, httpClientProvider, logger) {
    }
}
