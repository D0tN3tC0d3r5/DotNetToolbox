namespace DotNetToolbox.AI.Agents;

public class StandardAgentFactory<TStandardAgent>(IHttpClientProviderFactory httpClientProviderFactory, ILoggerFactory loggerFactory)
    : IStandardAgentFactory {
    public IStandardAgent Create(string provider, World world, Persona persona, IAgentOptions options) {
        var httpClientProvider = httpClientProviderFactory.Create(provider);
        return (IStandardAgent)CreateInstance.Of<TStandardAgent>(world,
                                                                 persona,
                                                                 options,
                                                                 httpClientProvider,
                                                                 loggerFactory.CreateLogger<TStandardAgent>())!;
    }
}

public class BackgroundAgentFactory<TBackgroundAgent>(IHttpClientProviderFactory httpClientProviderFactory, ILoggerFactory loggerFactory)
    : IBackgroundAgentFactory {
    public IBackgroundAgent Create(string provider, World world, Persona persona, IAgentOptions options) {
        var httpClientProvider = httpClientProviderFactory.Create(provider);
        return (IBackgroundAgent)CreateInstance.Of<TBackgroundAgent>(world,
                                                                     persona,
                                                                     options,
                                                                     httpClientProvider,
                                                                     loggerFactory.CreateLogger<TBackgroundAgent>())!;
    }
}

public class QueuedAgentFactory<TQueuedAgent>(IHttpClientProviderFactory httpClientProviderFactory, ILoggerFactory loggerFactory)
    : IQueuedAgentFactory {
    public IQueuedAgent Create(string provider, World world, Persona persona, IAgentOptions options) {
        var httpClientProvider = httpClientProviderFactory.Create(provider);
        return (IQueuedAgent)CreateInstance.Of<TQueuedAgent>(world,
                                                             persona,
                                                             options,
                                                             httpClientProvider,
                                                             loggerFactory.CreateLogger<TQueuedAgent>())!;
    }
}
