namespace Sophia;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddServices(this IServiceCollection services) {
        services.AddScoped<IWorldService, WorldService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProvidersService, ProvidersService>();
        services.AddScoped<IToolsService, ToolsService>();
        services.AddScoped<IPersonasService, PersonasService>();
        services.AddScoped<IChatsService, ChatsService>();
        services.AddScoped<IAgentService, AgentService>();
        return services;
    }
}
