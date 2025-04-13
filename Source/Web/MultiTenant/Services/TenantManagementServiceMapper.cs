namespace WebApi.Services;

internal static class TenantManagementServiceMapper {
    internal static Tenant ToModel(this AddTenantRequest request, string hashedSecret)
        => new() {
            Name = request.Name,
            Secret = hashedSecret,
        };

    internal static AddTenantResponse ToResponse(this Tenant tenant, string secret)
        => new() {
            Id = Base64UrlEncoder.Encode(tenant.Id.ToByteArray()),
            Secret = secret,
        };

    internal static RenewableTokenResponse ToResponse(this AccessToken accessToken)
        => new() {
            Id = accessToken.Id,
            Expiration = accessToken.ValidUntil,
            Start = accessToken.DelayStartUntil,
            Value = accessToken.Value,
            RenewalEnd = accessToken.RenewableUntil,
        };
}
