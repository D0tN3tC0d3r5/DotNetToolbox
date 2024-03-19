﻿using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.Anthropic;

public class AgentFactory(World world, IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory)
    : IAgentFactory {
    TAgent IAgentFactory.CreateAgent<TAgent>(IAgentOptions options, Persona persona)
        => options is not AgentOptions ao
               ? throw new ArgumentException("Invalid options type.", nameof(options))
               : CreateAgent<TAgent>(ao, persona);

    public TAgent CreateAgent<TAgent>(AgentOptions options, Persona persona)
        => CreateInstance.Of<TAgent>(world, options, persona, httpClientProvider, loggerFactory.CreateLogger<TAgent>());
}
