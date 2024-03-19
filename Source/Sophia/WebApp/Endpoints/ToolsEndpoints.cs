using Sophia.Models.Tools;

namespace Sophia.WebApp.Endpoints;

internal static class ToolsEndpoints {
    public static IEndpointConventionBuilder MapToolsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/Tools");
        group.MapGet("/", (ToolsService service) => service.GetList());
        group.MapGet("/{id}", (ToolsService service, [FromRoute]int id) => service.GetById(id));
        group.MapPost("/", (ToolsService service, [FromBody] ToolData newValue) => service.Add(newValue));
        group.MapPut("/", (ToolsService service, [FromBody] ToolData updatedValue) => service.Update(updatedValue));
        group.MapDelete("/{id}", (ToolsService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
