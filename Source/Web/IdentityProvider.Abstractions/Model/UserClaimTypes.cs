namespace WebApi.Model;

public static class UserClaimTypes {
    private static readonly ClaimsIdentityOptions _defaultIdentityClaims = new();

    public const string Id = "WebApi.Identity.User.Id";
    public const string Identifier = "WebApi.Identity.User.ClientIdentifier";
    public const string Email = "WebApi.Identity.User.Email";
    public const string PhoneNumber = "WebApi.Identity.User.PhoneNumber";
    public const string Roles = "WebApi.Identity.User.Roles";
    public const string Role = "WebApi.Identity.User.Role";
    public static readonly string SecurityStamp = _defaultIdentityClaims.SecurityStampClaimType;
}