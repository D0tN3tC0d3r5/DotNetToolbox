namespace DotNetToolbox.AI.OpenAI;

public class OpenAIDefaultAgent(World world,
                                OpenAIAgentOptions options,
                                IPersona persona,
                                IHttpClientProvider httpClientProvider,
                                ILogger<OpenAIDefaultAgent> logger)
    : OpenAIAgent<OpenAIDefaultAgent>(world, options, persona, httpClientProvider, logger) {

    public OpenAIDefaultAgent(IEnvironment environment,
                              OpenAIAgentOptions options,
                              IPersona persona,
                              IHttpClientProvider httpClientProvider,
                              ILogger<OpenAIDefaultAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }
}
