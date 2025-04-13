using IAuthenticationService = WebApi.Services.IAuthenticationService;

namespace WebApi.Builders;

public class IdentityProviderWebApiBuilder<TUserDataStore>(string[] args)
    : IdentityProviderWebApiBuilder<TUserDataStore, User>(args)
    where TUserDataStore : class, IUserStore<User>
;

public class IdentityProviderWebApiBuilder<TUserDataStore, TUser>(string[] args)
    : IdentityProviderWebApiBuilder<IdentityProviderWebApiOptions, TUserDataStore, TUser>(args)
    where TUserDataStore : class, IUserStore<TUser>
    where TUser : User, new();

public class IdentityProviderWebApiBuilder<TOptions, TUserDataStore, TUser>
    : WebApiBuilder<TOptions>
    where TOptions : IdentityProviderWebApiOptions<TOptions>, new()
    where TUserDataStore : class, IUserStore<TUser>
    where TUser : User, new() {
    public IdentityProviderWebApiBuilder(string[] args)
        : base(args) {
        Services.AddScoped<IAuthenticationService, SignInService<TUser>>();
        Services.AddIdentity<TUser, Role>(identityOptions => {
            identityOptions.SignIn.RequireConfirmedAccount = Options.AccountConfirmation.IsRequired;
            identityOptions.SignIn.RequireConfirmedEmail = Options.IdentifierField == UserIdentifierField.Email
                                                        || Options.AccountConfirmation is { IsRequired: true, Type: AccountConfirmationType.Email }
                                                        || Options.TwoFactorAuthentication is { IsRequired: true, Type: TwoFactorAuthenticationType.Email };
            identityOptions.SignIn.RequireConfirmedPhoneNumber = Options.TwoFactorAuthentication is { IsRequired: true, Type: TwoFactorAuthenticationType.Phone };
            identityOptions.User.RequireUniqueEmail = true;

            identityOptions.Password = Options.InternalSignIn.Password;
            identityOptions.Lockout = Options.InternalSignIn.Lockout;
            identityOptions.Stores.MaxLengthForKeys = 64;
            identityOptions.Stores.ProtectPersonalData = true;

            identityOptions.ClaimsIdentity.UserIdClaimType = Options.UserClaims.Id;
            identityOptions.ClaimsIdentity.UserNameClaimType = Options.UserClaims.Identifier;
            identityOptions.ClaimsIdentity.EmailClaimType = Options.UserClaims.Email;
            identityOptions.ClaimsIdentity.RoleClaimType = Options.UserClaims.Role;
            identityOptions.ClaimsIdentity.SecurityStampClaimType = Options.UserClaims.SecurityStamp;
        })
        .AddUserStore<TUserDataStore>()
        .AddUserManager<TUser>()
        .AddRoleManager<Role>()
        .AddDefaultTokenProviders();
    }
}
