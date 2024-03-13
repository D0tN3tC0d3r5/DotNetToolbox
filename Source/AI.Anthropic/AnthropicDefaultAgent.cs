namespace DotNetToolbox.AI.Anthropic;

public class AnthropicDefaultAgent(World world,
                                   AnthropicAgentOptions options,
                                   IPersona persona,
                                   IHttpClientProvider httpClientProvider,
                                   ILogger<AnthropicDefaultAgent> logger)
    : AnthropicAgent<AnthropicDefaultAgent>(world, options, persona, httpClientProvider, logger) {

    public AnthropicDefaultAgent(IEnvironment environment,
                                 AnthropicAgentOptions options,
                                 IPersona persona,
                                 IHttpClientProvider httpClientProvider,
                                 ILogger<AnthropicDefaultAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }
}
