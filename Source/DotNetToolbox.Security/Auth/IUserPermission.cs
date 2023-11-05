namespace DotNetToolbox.Security.Auth;

public interface IUserPermission<out TUserKey, out TActionKey> {
    TUserKey UserId { get; }
    TActionKey ActionId { get; }
}