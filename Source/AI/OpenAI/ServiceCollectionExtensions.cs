namespace DotNetToolbox.AI.OpenAI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services) {
        services.AddAIProvider();
        services.AddHttpClientProvider<OpenAI>("OpenAI");
        services.AddAIAgent<OpenAIHttpConnection>("OpenAI");
        return services;
    }
}
