namespace Sophia.WebApp.Endpoints;

internal static class ChatsEndpoints {
    public static IEndpointConventionBuilder MapChatsEndpoints(this IEndpointRouteBuilder endpoints) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("api/chats");
        group.MapGet("/", (IChatsService service, [FromQuery] string? filter = null) => service.GetList(filter));
        group.MapGet("/{id}", (IChatsService service, [FromRoute] Guid id) => service.GetById(id));
        group.MapPost("/", async (IChatsService service, [FromBody] ChatData newValue) => {
            await service.Create(newValue);
            return newValue;
        });
        group.MapPatch("/{id}/archive", (IChatsService service, [FromRoute] Guid id) => service.Archive(id));
        group.MapPatch("/{id}/unarchive", (IChatsService service, [FromRoute] Guid id) => service.Unarchive(id));
        group.MapPatch("/{id}/rename", (IChatsService service, [FromRoute] Guid id, [FromBody] string newName) => service.Rename(id, newName));
        group.MapPatch("/{id}/add-message", (IChatsService service, [FromRoute] Guid id, [FromBody] MessageData newValue) => service.AddMessage(id, newValue));
        group.MapDelete("/{id}", (IChatsService service, [FromRoute] Guid id) => service.Delete(id));
        return group;
    }
}
