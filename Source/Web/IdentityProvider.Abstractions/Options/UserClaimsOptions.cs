namespace WebApi.Options;

public class UserClaimsOptions {
    public string Id { get; set; } = UserClaimTypes.Id;
    public string Identifier { get; set; } = UserClaimTypes.Identifier;
    public string Email { get; set; } = UserClaimTypes.Email;
    public string PhoneNumber { get; set; } = UserClaimTypes.PhoneNumber;
    public string Role { get; set; } = UserClaimTypes.Role;
    public string SecurityStamp { get; set; } = UserClaimTypes.SecurityStamp;
}
