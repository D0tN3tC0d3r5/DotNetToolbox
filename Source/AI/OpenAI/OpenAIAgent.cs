namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent([FromKeyedServices("OpenAI")] IHttpClientProviderFactory httpClientProviderFactory, ILogger<OpenAIAgent> logger)
    : Agent<OpenAIAgent, Mapper, ChatRequest, ChatResponse>("OpenAI", httpClientProviderFactory, logger);
