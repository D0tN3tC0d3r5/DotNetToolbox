namespace Sophia.WebApp.Endpoints;

internal static class ChatsEndpoints {
    public static IEndpointConventionBuilder MapChatsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/chats");
        group.MapGet("/", (IChatsService service) => service.GetList());
        group.MapGet("/{id}", (IChatsService service, [FromRoute] int id) => service.Resume(id));
        group.MapPost("/", (IChatsService service, [FromBody] ChatData newValue) => service.Start(newValue));
        group.MapPatch("/{id}", (IChatsService service, [FromRoute] int id, [FromBody] string _) => service.Archive(id));
        group.MapDelete("/{id}", (IChatsService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
