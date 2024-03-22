namespace Sophia.WebApp.Endpoints;

internal static class PersonasEndpoints {
    public static IEndpointConventionBuilder MapPersonasEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/personas");
        group.MapGet("/", (IPersonasService service, [FromQuery] string? filter = null) => service.GetList(filter));
        group.MapGet("/{id}", (IPersonasService service, [FromRoute] int id) => service.GetById(id));
        group.MapPost("/", async (IPersonasService service, [FromBody] PersonaData newValue) => {
            await service.Add(newValue);
            return newValue;
        });
        group.MapPut("/", (IPersonasService service, [FromBody] PersonaData updatedValue) => service.Update(updatedValue));
        group.MapDelete("/{id}", (IPersonasService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
