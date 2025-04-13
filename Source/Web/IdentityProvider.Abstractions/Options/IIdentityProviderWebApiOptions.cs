namespace WebApi.Options;

public interface IIdentityProviderWebApiOptions
    : IIdentityProviderWebApiOptions<IIdentityProviderWebApiOptions>;

public interface IIdentityProviderWebApiOptions<out TOptions>
    : IWebApiOptions<TOptions>
    where TOptions : IIdentityProviderWebApiOptions<TOptions> {
    UserIdentifierField IdentifierField { get; }
    MasterIdentityOptions? Master { get; }
    UserClaimsOptions UserClaims { get;}
    RoleClaimsOptions RoleClaims { get; }
    SignInOptions InternalSignIn { get; }
    Dictionary<string, SignInOptions> ExternalSignInProviders { get; set; }
    AccountConfirmationOptions AccountConfirmation { get;}
    TwoFactorAuthenticationOptions TwoFactorAuthentication { get;}
}
