namespace WebApi.Utilities;

internal sealed class CurrentTenantAccessor(IHttpContextAccessor httpContextAccessor)
    : ICurrentTenantAccessor {
    public Guid? GetTenantId()
        => httpContextAccessor.HttpContext?.Items[TenantContextMiddleware.TenantIdKey] as Guid?;

    public Tenant? GetTenantInfo()
        => httpContextAccessor.HttpContext?.Items[TenantContextMiddleware.TenantInfoKey] as Tenant;
}
