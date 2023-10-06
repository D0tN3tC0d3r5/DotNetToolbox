using System.Security.Claims;

namespace DotNetToolbox.Http.Options;

public class HttpClientAuthorizationOptions {
    public required HttpClientAuthorizationType Type { get; set; }

    public HttpClientAuthorizationScheme? Scheme { get; set; }
    public string? Value { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; }
    public DateTimeOffset? NotBefore { get; set; }

    public IReadOnlyList<Claim> Claims { get; set; } = Array.Empty<Claim>();

    public string? Issuer { get; set; }
    public string? Audience { get; set; }

    public string[] Scopes { get; set; } = Array.Empty<string>();
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Authority { get; set; }
}