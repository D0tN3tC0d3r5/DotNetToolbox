namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent
    : Agent<OpenAIAgent, Mapper, ChatRequest, ChatResponse> {
    public OpenAIAgent([FromKeyedServices("OpenAI")] IHttpClientProviderFactory httpClientProviderFactory, ILogger<OpenAIAgent> logger)
        : base("OpenAI", httpClientProviderFactory, logger) {
        Options = new AgentOptions("v1/engines");
    }
}
