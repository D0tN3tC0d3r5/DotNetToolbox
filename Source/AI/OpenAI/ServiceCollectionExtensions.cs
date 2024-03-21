﻿using DotNetToolbox.AI.OpenAI.Http;

namespace DotNetToolbox.AI.OpenAI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddAIProvider<AgentHttpClientProvider, AgentFactory>("OpenAI", configuration);
        return services;
    }
}
