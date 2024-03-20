namespace Sophia.WebApp.Endpoints;

internal static class PersonasEndpoints {
    public static IEndpointConventionBuilder MapPersonasEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/personas");
        group.MapGet("/", (PersonasService service) => service.GetList());
        group.MapGet("/{id}", (PersonasService service, [FromRoute] int id) => service.GetById(id));
        group.MapPost("/", (PersonasService service, [FromBody] PersonaData newValue) => service.Add(newValue));
        group.MapPut("/", (PersonasService service, [FromBody] PersonaData updatedValue) => service.Update(updatedValue));
        group.MapDelete("/{id}", (PersonasService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
