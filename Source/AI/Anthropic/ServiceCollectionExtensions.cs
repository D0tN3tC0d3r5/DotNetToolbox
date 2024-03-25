namespace DotNetToolbox.AI.Anthropic;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAnthropic(this IServiceCollection services) {
        services.AddAIProvider();
        services.AddHttpClientProvider<Anthropic>("Anthropic");
        services.AddAIAgent<AnthropicAgent>("Anthropic");
        return services;
    }
}
