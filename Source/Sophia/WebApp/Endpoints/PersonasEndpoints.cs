namespace Sophia.WebApp.Endpoints;

internal static class PersonasEndpoints {
    public static IEndpointConventionBuilder MapPersonasEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/personas");
        group.MapGet("/", (IPersonasService service) => service.GetList());
        group.MapGet("/{id}", (IPersonasService service, [FromRoute] int id) => service.GetById(id));
        group.MapPost("/", (IPersonasService service, [FromBody] PersonaData newValue) => service.Add(newValue));
        group.MapPut("/", (IPersonasService service, [FromBody] PersonaData updatedValue) => service.Update(updatedValue));
        group.MapDelete("/{id}", (IPersonasService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
