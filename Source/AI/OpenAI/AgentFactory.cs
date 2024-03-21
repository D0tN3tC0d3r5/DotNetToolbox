using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.OpenAI;

public class AgentFactory([FromKeyedServices("OpenAI")] IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
    : IAgentFactory {
    TAgent IAgentFactory.CreateAgent<TAgent>(World world, IAgentOptions options, Persona persona)
        => Create<TAgent>(world, (AgentOptions)options, persona);
    public TAgent Create<TAgent>(World world, AgentOptions options, Persona persona)
        => CreateInstance.Of<TAgent>(world, options, persona, new Mapper(), httpClientProvider, loggerFactory.CreateLogger<TAgent>());
}
