using Sophia.Models.Worlds;

namespace Sophia.WebApp.Components.World;
internal static class EndpointRouteBuilderExtensions {
    public static IEndpointConventionBuilder MapWorldEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/World");

        group.MapGet("/", (IWorldService service) => service.GetWorld());

        group.MapPut("/", (IWorldService service,
                                  [FromBody] WorldData updatedValue) => service.UpdateWorld(updatedValue));
        return group;
    }
}
