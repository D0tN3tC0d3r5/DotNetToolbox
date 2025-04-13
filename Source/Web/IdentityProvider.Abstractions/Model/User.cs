namespace WebApi.Model;

public class User
    : IdentityUser<Guid> {
    public override Guid Id { get; set; } = Guid.CreateVersion7();
    public bool IsBlocked { get; set; }
}