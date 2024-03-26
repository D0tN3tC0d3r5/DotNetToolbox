namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent([FromKeyedServices("Anthropic")] IHttpClientProviderFactory factory, ILogger<AnthropicAgent> logger)
    : Agent<AnthropicAgent, Mapper, ChatRequest, ChatResponse>("Anthropic", factory, logger);
