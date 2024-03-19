namespace Sophia.WebApp.Endpoints;

internal static class WorldEndpoints {
    public static IEndpointConventionBuilder MapWorldEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/World");
        group.MapGet("/", (WorldService service) => service.GetWorld());
        group.MapPut("/", (WorldService service, [FromBody] WorldData updatedValue) => service.UpdateWorld(updatedValue));
        return group;
    }
}
