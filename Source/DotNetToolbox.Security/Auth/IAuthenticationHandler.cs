namespace DotNetToolbox.Security.Auth;

public interface IAuthenticationHandler<in TUserKey> {
    Task<BoolResult> IsAuthenticatedAsync(IUserId<TUserKey> userId, CancellationToken cancellation = default);
    Task<SignInResult> SignInAsync(ISignIn signIn, CancellationToken cancellation = default);
    Task<SignInResult> SignOutAsync(ISignIn signIn, CancellationToken cancellation = default);
}
