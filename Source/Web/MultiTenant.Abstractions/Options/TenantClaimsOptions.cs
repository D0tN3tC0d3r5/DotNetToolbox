namespace WebApi.Options;

public class TenantClaimsOptions {
    public string Id { get; set; } = TenantClaimTypes.Id;
    public string Identifier { get; set; } = TenantClaimTypes.Identifier;
    public string Name { get; set; } = TenantClaimTypes.Name;
}
