using System.Security.Claims;

namespace WebApi.Model;

public class Tenant {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;

    public virtual AccessToken[] AccessTokens { get; set; } = [];
    public virtual Claim[] Claims { get; set; } = [];
}