namespace Sophia.WebClient.Account;
internal sealed class PersistentAuthenticationStateProvider : AuthenticationStateProvider {
    private static readonly AuthenticationState _unauthenticated =
        new(new(new ClaimsIdentity()));

    private readonly AuthenticationState _authenticationState = _unauthenticated;

    public PersistentAuthenticationStateProvider(PersistentComponentState state) {
        if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
            return;

        var claims = new Claim[] {
            new(ClaimTypes.NameIdentifier, userInfo.UserId),
            new(ClaimTypes.GivenName, userInfo.Name),
            new(ClaimTypes.Name, userInfo.Email),
            new(ClaimTypes.Email, userInfo.Email),
        };

        var identity = new ClaimsIdentity(claims, authenticationType: nameof(PersistentAuthenticationStateProvider));
        _authenticationState = new(new(identity));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(_authenticationState);
}
