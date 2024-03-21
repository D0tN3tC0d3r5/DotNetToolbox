namespace Sophia.WebApp.Endpoints;

internal static class ChatsEndpoints {
    public static IEndpointConventionBuilder MapChatsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/chats");
        group.MapGet("/", (IChatsService service) => service.GetList());
        group.MapGet("/{id}", (IChatsService service, [FromRoute] int id) => service.GetById(id));
        group.MapPost("/", (IChatsService service, [FromBody] ChatData newValue) => service.Create(newValue));
        group.MapPatch("/{id}/archive", (IChatsService service, [FromRoute] int id) => service.Archive(id));
        group.MapPatch("/{id}/add-message", (IChatsService service, [FromRoute] int id, [FromBody] MessageData newValue) => service.AddMessage(id, newValue));
        group.MapDelete("/{id}", (IChatsService service, [FromRoute] int id) => service.Delete(id));
        return group;
    }
}
