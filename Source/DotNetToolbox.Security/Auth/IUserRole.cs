namespace DotNetToolbox.Security.Auth;

public interface IUserRole<out TUserKey, out TRoleKey> {
    TUserKey UserId { get; }
    TRoleKey RoleId { get; }
}