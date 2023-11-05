namespace DotNetToolbox.Security.Auth;

public interface IRolePermission<out TRoleKey, out TActionKey> {
    TRoleKey RoleId { get; }
    TActionKey ActionId { get; }
}