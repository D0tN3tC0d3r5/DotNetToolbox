using PasswordSignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using SignInResult = WebApi.Model.SignInResult;

namespace WebApi.Services;

public class SignInService(IHostEnvironment environment,
                           IOptions<IdentityProviderWebApiOptions> identityOptions,
                           UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           ITokenFactory tokenFactory,
                           ILogger<SignInService> logger)
    : SignInService<User>(environment,
                          identityOptions,
                          userManager,
                          signInManager,
                          tokenFactory,
                          logger);

public class SignInService<TUser>(IHostEnvironment environment,
                                  IOptions<IdentityProviderWebApiOptions> identityOptions,
                                  UserManager<TUser> userManager,
                                  SignInManager<User> signInManager,
                                  ITokenFactory tokenFactory,
                                  ILogger logger)
    : IAuthenticationService
    where TUser : User, new() {
    private readonly IdentityProviderWebApiOptions _options = identityOptions.Value;

    public async Task<TypedResult<SignInResult, TemporaryToken>> PasswordSignIn(PasswordSignInRequest request) {
        logger.LogInformation("Login attempt for '{Identifier}'.", request.Identifier);
        if (TryValidateMasterUser(request, _options.Master, out var master))
            return await HandleMasterSignIn(master);

        var user = await userManager.FindByNameAsync(request.Identifier);
        if (user is null) {
            logger.LogInformation("Account '{Identifier}' not found.", request.Identifier);
            return TypedResult.As(SignInResult.AccountNotFound).WithNo<TemporaryToken>();
        }

        if (user.IsBlocked) {
            logger.LogInformation("Account '{Identifier}' is blocked.", user.UserName);
            return TypedResult.As(SignInResult.AccountIsBlocked).WithNo<TemporaryToken>();
        }

        return await HandleUserSignIn(user, request.Password);
    }

    public async Task SignOut(SignOutRequest request) {
        var user = await userManager.FindByNameAsync(request.Identifier);
        if (user is null) {
            logger.LogInformation("Account '{Identifier}' not found when signing out.", request.Identifier);
            return;
        }

        logger.LogInformation("Account '{Identifier}' logged out.", request.Identifier);
    }

    public Task<AuthenticationScheme[]> GetSchemes() => Task.FromResult<AuthenticationScheme[]>([]);

    private async Task<TypedResult<SignInResult, TemporaryToken>> HandleMasterSignIn(TUser master) {
        var token = await GenerateUserAccessToken(master);
        logger.LogInformation("Master user logged in.");
        return TypedResult.As(SignInResult.Success, token);
    }

    private async Task<TypedResult<SignInResult, TemporaryToken>> HandleUserSignIn(TUser user, string secret) {
        if (_options.AccountConfirmation.IsRequired && !await userManager.IsEmailConfirmedAsync(user)) {
            logger.LogInformation("Account '{Identifier}' confirmation is pending.", user.UserName);
            var tokenValue = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationToken = tokenFactory.CreateTemporaryToken(_options.AccountConfirmation.Token, AccountManagementTokenType.AccountConfirmation, tokenValue);
            return TypedResult.As(SignInResult.AccountConfirmationRequired, confirmationToken);
        }

        if (_options.TwoFactorAuthentication.IsRequired && !await signInManager.IsTwoFactorEnabledAsync(user)) {
            logger.LogInformation("Account '{Identifier}' two factor configuration is pending or incorrect.", user.UserName);
            return TypedResult.As(SignInResult.TwoFactorIsNotEnabled).WithNo<TemporaryToken>();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, secret, true);
        if (result == PasswordSignInResult.NotAllowed) {
            logger.LogInformation("Account '{Identifier}' is not allowed to sign in.", user.UserName);
            return TypedResult.As(SignInResult.SignInIsNotAllowed).WithNo<TemporaryToken>();
        }

        if (result == PasswordSignInResult.LockedOut) {
            logger.LogInformation("Account '{Identifier}' does not have a local login.", user.UserName);
            return TypedResult.As(SignInResult.AccountIsLocked).WithNo<TemporaryToken>();
        }

        if (result == PasswordSignInResult.Failed) {
            logger.LogInformation("Invalid login for '{Identifier}'.", user.UserName);
            return TypedResult.As(SignInResult.Incorrect).WithNo<TemporaryToken>();
        }

        if (result == PasswordSignInResult.TwoFactorRequired) {
            logger.LogInformation("Account '{Identifier}' requires two factor authentication.", user.UserName);
            var tokenValue = await userManager.GenerateTwoFactorTokenAsync(user, "Default");
            var twoFactorToken = tokenFactory.CreateTemporaryToken(_options.AccountConfirmation.Token, AccountManagementTokenType.TwoFactor, tokenValue);
            return TypedResult.As(SignInResult.TwoFactorRequired, twoFactorToken);
        }

        logger.LogInformation("Account '{Identifier}' logged in.", user.UserName);
        var accessToken = await GenerateUserAccessToken(user);
        return TypedResult.As(SignInResult.Success, accessToken);
    }
    private bool TryValidateMasterUser(PasswordSignInRequest request, MasterIdentityOptions? masterUserOptions, [NotNullWhen(true)] out TUser? user) {
        user = null;
        if (masterUserOptions is null)
            return false;
        if (!masterUserOptions.Identifier.Equals(request.Identifier, StringComparison.OrdinalIgnoreCase))
            return false;
        var hashedSecret = environment.IsDevelopment()
                               ? request.Password
                               : HashSecret(request.Password);
        if (masterUserOptions.HashedSecret != hashedSecret)
            return false;

        user = new() {
            UserName = masterUserOptions.Identifier,
            Email = masterUserOptions.Email,
            PhoneNumber = masterUserOptions.PhoneNumber,
        };
        return true;
    }

    private static string HashSecret(string secret)
        => Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes(secret)));

    private async Task<TemporaryToken> GenerateUserAccessToken(TUser user) {
        var claims = new List<Claim> {
            new(_options.UserClaims.Id, user.Id.ToString()),
            new(_options.UserClaims.Identifier, user.UserName!),
        };
        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new(_options.UserClaims.Email, user.Email));
        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new(_options.UserClaims.PhoneNumber, user.PhoneNumber));
        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(ur => new Claim(_options.UserClaims.Role, ur)));
        if (user.SecurityStamp is not null)
            claims.Add(new(_options.UserClaims.SecurityStamp, user.SecurityStamp));
        var identity = new ClaimsIdentity(claims,
                                          IdentityConstants.ExternalScheme,
                                          _options.UserClaims.Identifier,
                                          _options.UserClaims.Role);
        return tokenFactory.CreateAccessToken(_options.UserAccessToken, identity);
    }
}
