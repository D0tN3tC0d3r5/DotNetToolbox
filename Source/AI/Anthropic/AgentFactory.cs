using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.Anthropic;

public class AgentFactory([FromKeyedServices("Anthropic")] IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
    : IAgentFactory {
    TAgent IAgentFactory.CreateAgent<TAgent>(World world, IAgentOptions options, Persona persona)
        => CreateAgent<TAgent>(world, (AgentOptions)options, persona);

    public TAgent CreateAgent<TAgent>(World world, AgentOptions options, Persona persona)
        => CreateInstance.Of<TAgent>(world, options, persona, httpClientProvider, loggerFactory.CreateLogger<TAgent>());
}
