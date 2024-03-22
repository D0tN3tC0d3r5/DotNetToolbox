namespace Sophia.WebApp.Endpoints;

internal static class AgentEndpoints {
    public static IEndpointConventionBuilder MapAgentEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/agent");
        group.MapPost("/", (IAgentService service, [FromBody] GetResponseRequest request) => service.GetResponse(request));
        return group;
    }
}
