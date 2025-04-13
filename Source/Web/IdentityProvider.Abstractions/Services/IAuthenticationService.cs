using SignInResult = WebApi.Model.SignInResult;

namespace WebApi.Services;

public interface IAuthenticationService {
    Task<TypedResult<SignInResult, TemporaryToken>> PasswordSignIn(PasswordSignInRequest request);
    Task SignOut(SignOutRequest request);
    Task<AuthenticationScheme[]> GetSchemes();
}
