namespace Sophia.WebApp.Endpoints;

internal static class WorldEndpoints {
    public static IEndpointConventionBuilder MapWorldEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/world");
        group.MapGet("/", (IWorldService service) => service.GetWorld());
        group.MapPut("/", (IWorldService service, [FromBody] WorldData updatedValue) => service.UpdateWorld(updatedValue));
        return group;
    }
}
