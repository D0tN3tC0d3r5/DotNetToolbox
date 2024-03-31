namespace Sophia.Data.Identity;

public interface IUserAccessor {
    string Id { get; }
    string BaseFolder { get; }
    string Email { get; }
}