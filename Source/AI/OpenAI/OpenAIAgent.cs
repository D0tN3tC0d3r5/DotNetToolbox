namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent([FromKeyedServices("OpenAI")] IHttpClientProviderFactory factory, ILogger<OpenAIAgent> logger)
    : Agent<OpenAIAgent, Mapper, ChatRequest, ChatResponse>("OpenAI", factory, logger);
