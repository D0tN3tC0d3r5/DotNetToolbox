namespace Sophia.WebApp.Endpoints;

internal static class ToolsEndpoints {
    public static IEndpointConventionBuilder MapToolsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/tools");
        group.MapGet("/", (IToolsService service, [FromQuery] string? filter = null) => service.GetList(filter));
        group.MapGet("/{id}", (IToolsService service, [FromRoute] int id) => service.GetById(id));
        group.MapPost("/", async (IToolsService service, [FromBody] ToolData newValue) => {
            await service.Add(newValue);
            return newValue;
        });
        group.MapPut("/", (IToolsService service, [FromBody] ToolData updatedValue) => service.Update(updatedValue));
        group.MapDelete("/{id}", (IToolsService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
