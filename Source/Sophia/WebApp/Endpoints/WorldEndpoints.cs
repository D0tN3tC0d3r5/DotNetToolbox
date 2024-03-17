namespace Sophia.WebApp.Endpoints;

internal static class WorldEndpoints {
    public static IEndpointConventionBuilder MapWorldEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/World");
        group.MapGet("/", (IWorldService service)
                              => service.GetWorld());
        group.MapPut("/", (IWorldService service, [FromBody] WorldData updatedValue)
                              => service.UpdateWorld(updatedValue));
        return group;
    }
}

internal static class SkillsEndpoints {
    public static IEndpointConventionBuilder MapSkillsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/Skills");
        group.MapGet("/", (ISkillsService service)
                              => service.GetList());
        group.MapGet("/{id}", (ISkillsService service, [FromRoute]int id)
                              => service.GetById(id));
        group.MapPost("/", (ISkillsService service, [FromBody] SkillData newValue)
                              => service.Add(newValue));
        group.MapPut("/", (ISkillsService service, [FromBody] SkillData updatedValue)
                              => service.Update(updatedValue));
        group.MapDelete("/{id}", (ISkillsService service, [FromRoute] int id)
                              => service.Delete(id));
        return group;
    }
}
