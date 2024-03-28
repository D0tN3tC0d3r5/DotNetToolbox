//namespace Sophia.WebApp.Endpoints;

//internal static class ProvidersEndpoints {
//    public static IEndpointConventionBuilder MapProvidersEndpoints(this IEndpointRouteBuilder endpoints) {
//        ArgumentNullException.ThrowIfNull(endpoints);

//        var group = endpoints.MapGroup("api/providers");
//        group.MapGet("/", (IProvidersService service, [FromQuery] string? filter = null) => service.GetList(filter));
//        group.MapGet("/{id}", (IProvidersService service, [FromRoute] int id) => service.GetById(id));
//        group.MapPost("/", async (IProvidersService service, [FromBody] ProviderData newValue) => {
//            await service.Add(newValue);
//            return newValue;
//        });
//        group.MapPut("/", (IProvidersService service, [FromBody] ProviderData updatedValue) => service.Update(updatedValue));
//        group.MapDelete("/{id}", (IProvidersService service, [FromRoute] int id) => service.Delete(id));
//        return group;
//    }
//}
