namespace Sophia.Identity;

public interface IUserAccessor {
    string Id { get; }
    string Name { get; }
    string Email { get; }
    ICollection<Role> Roles { get; }
}
