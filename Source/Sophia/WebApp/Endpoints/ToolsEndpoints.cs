namespace Sophia.WebApp.Endpoints;

internal static class ToolsEndpoints {
    public static IEndpointConventionBuilder MapToolsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/tools");
        group.MapGet("/", (IToolsService service) => service.GetList());
        group.MapGet("/{id}", (IToolsService service, [FromRoute]int id) => service.GetById(id));
        group.MapPost("/", (IToolsService service, [FromBody] ToolData newValue) => service.Add(newValue));
        group.MapPut("/", (IToolsService service, [FromBody] ToolData updatedValue) => service.Update(updatedValue));
        group.MapDelete("/{id}", (IToolsService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
