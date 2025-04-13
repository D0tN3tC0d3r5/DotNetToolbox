namespace WebApi.Options;

public class RoleClaimsOptions {
    public string Id { get; set; } = UserClaimTypes.Id;
    public string Name { get; set; } = UserClaimTypes.Identifier;
}
