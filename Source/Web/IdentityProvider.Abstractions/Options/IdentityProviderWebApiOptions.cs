namespace WebApi.Options;

public record IdentityProviderWebApiOptions
    : IdentityProviderWebApiOptions<IdentityProviderWebApiOptions>
    , IIdentityProviderWebApiOptions;

public record IdentityProviderWebApiOptions<TOptions>
    : WebApiOptions<TOptions>
    , IIdentityProviderWebApiOptions<TOptions>
    where TOptions : IdentityProviderWebApiOptions<TOptions>, new() {
    public UserIdentifierField IdentifierField { get; set; }
    public MasterIdentityOptions? Master { get; set; }
    public UserClaimsOptions UserClaims { get; set; } = new();
    public RoleClaimsOptions RoleClaims { get; set; } = new();
    public SignInOptions InternalSignIn { get; set; } = new();
    public Dictionary<string, SignInOptions> ExternalSignInProviders { get; set; } = [];
    public AccessTokenOptions UserAccessToken { get; set; } = new();
    public AccountConfirmationOptions AccountConfirmation { get; set; } = new();
    public TwoFactorAuthenticationOptions TwoFactorAuthentication { get; set; } = new();
}
