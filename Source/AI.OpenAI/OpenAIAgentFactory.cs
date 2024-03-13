namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgentFactory(World world, IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
    : IAgentFactory {
    TAgent IAgentFactory.CreateAgent<TAgent>(IAgentOptions options, IPersona persona)
        => options is not OpenAIAgentOptions ao
               ? throw new ArgumentException("Invalid options type.", nameof(options))
               : CreateAgent<TAgent>(ao, persona);

    public TAgent CreateAgent<TAgent>(OpenAIAgentOptions options, IPersona persona)
        => CreateInstance.Of<TAgent>(world, options, persona, httpClientProvider, loggerFactory.CreateLogger<TAgent>());
}
