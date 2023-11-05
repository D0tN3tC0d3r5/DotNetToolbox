namespace DotNetToolbox.Security.Auth;

public interface IAuthorizationHandler<in TUserKey, in TRoleKey, in TActionKey> {
    Task<BoolResult> IsInRoleAsync(IRoleId<TRoleKey> roleId, CancellationToken cancellation = default);
    Task<Result> GrantRoleToUserAsync(IUserRole<TUserKey, TRoleKey> userRole, CancellationToken cancellation = default);
    Task<Result> RevokeRoleFromUserAsync(IUserRole<TUserKey, TRoleKey> userRole, CancellationToken cancellation = default);

    Task<BoolResult> IsAuthorizedAsync(IUserId<TUserKey> userId, CancellationToken cancellation = default);
    Task<BoolResult> IsAuthorizedAsync(IRoleId<TRoleKey> roleId, CancellationToken cancellation = default);
    Task<Result> GrantPermissionToRoleAsync(IRolePermission<TRoleKey, TActionKey> rolePermission, CancellationToken cancellation = default);
    Task<Result> GrantPermissionToUserAsync(IUserPermission<TUserKey, TActionKey> userPermission, CancellationToken cancellation = default);
    Task<Result> RevokePermissionFromRoleAsync(IUserPermission<TRoleKey, TActionKey> rolePermission, CancellationToken cancellation = default);
    Task<Result> RevokePermissionFromUserAsync(IUserPermission<TUserKey, TActionKey> userPermission, CancellationToken cancellation = default);
}
