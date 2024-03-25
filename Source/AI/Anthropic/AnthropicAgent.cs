namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent([FromKeyedServices("Anthropic")] IHttpClientProviderFactory httpClientProviderFactory, ILogger<AnthropicAgent> logger)
    : Agent<AnthropicAgent, Mapper, ChatRequest, ChatResponse>("Anthropic", httpClientProviderFactory, logger);
