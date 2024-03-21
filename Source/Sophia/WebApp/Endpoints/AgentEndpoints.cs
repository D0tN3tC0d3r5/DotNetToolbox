namespace Sophia.WebApp.Endpoints;

internal static class AgentEndpoints {
    public static IEndpointConventionBuilder MapAgentsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/agent");
        group.MapPost("/", (IAgentService service, [FromBody] string message) => service.GetResponse(message));
        return group;
    }
}
