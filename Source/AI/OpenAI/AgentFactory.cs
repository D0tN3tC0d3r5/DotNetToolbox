namespace DotNetToolbox.AI.OpenAI;

public class AgentFactory(World world, IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
    : IAgentFactory {
    TAgent IAgentFactory.CreateAgent<TAgent>(IAgentOptions options, Persona persona)
        => Create<TAgent>((AgentOptions)options, persona);
    public TAgent Create<TAgent>(AgentOptions options, Persona persona)
        => CreateInstance.Of<TAgent>(world, options, persona, new Mapper(), httpClientProvider, loggerFactory.CreateLogger<TAgent>());
}
