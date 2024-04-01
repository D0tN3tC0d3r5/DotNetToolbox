namespace Sophia.WebClient;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddRemoteServices(this IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<IWorldRemoteService, WorldRemoteService>();
        services.AddScoped<IUserRemoteService, UserRemoteService>();
        services.AddScoped<IProvidersRemoteService, ProvidersRemoteService>();
        services.AddScoped<IToolsRemoteService, ToolsRemoteService>();
        services.AddScoped<IPersonasRemoteService, PersonasRemoteService>();
        services.AddScoped<IChatsRemoteService, ChatsRemoteService>();
        services.AddScoped<IAgentRemoteService, AgentRemoteService>();
        services.AddScoped(_ => new HttpClient {
            BaseAddress = new(IsNotNull(configuration["ApiAddress"])),
        });
        return services;
    }
}
