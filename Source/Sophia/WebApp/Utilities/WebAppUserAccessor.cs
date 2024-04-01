namespace Sophia.WebApp.Utilities;

[ExcludeFromCodeCoverage]
public class WebAppUserAccessor
    : IUserAccessor {
    public WebAppUserAccessor(IHttpContextAccessor accessor) {
        var httpContext = Ensure.IsNotNull(accessor.HttpContext);
        var claims = httpContext.User.Claims.ToArray();
        Id = GetClaimValue(claims, ClaimTypes.NameIdentifier);
        Email = GetClaimValue(claims, ClaimTypes.Email);
        Name = GetClaimValue(claims, ClaimTypes.GivenName);
        Roles = claims.Where(c => c.Type == ClaimTypes.Role)
                      .Select(ToRole)
                      .Union([Role.Guest])
                      .ToArray();
    }

    private static string GetClaimValue(IEnumerable<Claim> claims, string claimType)
        => claims.FirstOrDefault(c => c.Type == claimType)?.Value
        ?? throw new InvalidOperationException($"Identity user's claim '{claimType}' was not found.");

    public string Id { get; }
    public string Email { get; }
    public string Name { get; }
    public ICollection<Role> Roles { get; }

    private static Role ToRole(Claim claim)
        => Enum.TryParse<Role>(claim.Value, true, out var role)
               ? role
               : Role.Guest;
}
